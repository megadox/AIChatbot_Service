using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Policies;

namespace BAStudio.Chatbot.Orchestration;

public sealed class ChatOrchestrator : IChatOrchestrator
{
    private const string DetailsMarker = "\n<<<DETAILS>>>\n";
    private static readonly ConcurrentDictionary<string, ConversationGrounding> ConversationGroundings = new(StringComparer.OrdinalIgnoreCase);

    private readonly IEmbeddingService _embeddings;
    private readonly IVectorStore _vectorStore;
    private readonly IPromptBuilder _promptBuilder;
    private readonly ILlmService _llm;
    private readonly IWebSearchService? _webSearch;
    private readonly DomainIntentResolver _intentResolver;

    public ChatOrchestrator(
        IEmbeddingService embeddings,
        IVectorStore vectorStore,
        IPromptBuilder promptBuilder,
        ILlmService llm,
        IWebSearchService? webSearch = null,
        DomainIntentResolver? intentResolver = null)
    {
        _embeddings = embeddings;
        _vectorStore = vectorStore;
        _promptBuilder = promptBuilder;
        _llm = llm;
        _webSearch = webSearch;
        _intentResolver = intentResolver ?? new DomainIntentResolver();
    }

    public async IAsyncEnumerable<ChatStreamEvent> AskAsync(
        ChatRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            yield return ChatStreamEvent.Error("질문을 입력해주세요.");
            yield break;
        }

        yield return ChatStreamEvent.Status("질문 의도를 분석하는 중...");
        var intent = _intentResolver.Resolve(request.Question);
        yield return ChatStreamEvent.Status(
            $"의도 분석: intent={intent.Intent}, group={intent.PreferredGroup ?? "-"}, activity={intent.ActivityNameHint ?? "-"}");
        var conversationId = string.IsNullOrWhiteSpace(request.ConversationId) ? "default" : request.ConversationId;
        ConversationGroundings.TryGetValue(conversationId, out var previousGrounding);
        var followUpKind = ResolveFollowUpKind(request.Question);
        if (IsOutOfScopeQuestion(request.Question, intent, followUpKind, previousGrounding))
        {
            if (request.AllowWebSearch)
            {
                yield return ChatStreamEvent.Status("웹 검색 중...");
                var webResult = _webSearch is null
                    ? new WebSearchResult(request.Question, "", [], IsConnected: false, "웹 검색 서비스가 설정되어 있지 않습니다.")
                    : await _webSearch.SearchAsync(request.Question, cancellationToken);

                if (!webResult.IsConnected)
                {
                    yield return ChatStreamEvent.Status("웹 검색 실패");
                    yield return ChatStreamEvent.Token($"웹에 연결할 수 없습니다. {webResult.ErrorMessage ?? "네트워크 연결을 확인해주세요."}");
                    yield return ChatStreamEvent.Completed();
                    yield break;
                }

                if (string.IsNullOrWhiteSpace(webResult.Summary) && webResult.Items.Count == 0)
                {
                    yield return ChatStreamEvent.Status("웹 검색 결과 없음");
                    yield return ChatStreamEvent.Token("웹 검색은 수행했지만 답변에 사용할 만한 검색 결과를 찾지 못했습니다.");
                    yield return ChatStreamEvent.Completed();
                    yield break;
                }

                yield return ChatStreamEvent.Status("웹 검색 기반 답변 생성");
                yield return ChatStreamEvent.Token(BuildWebSearchAnswer(webResult));
                yield return ChatStreamEvent.Completed();
                yield break;
            }

            if (!request.AllowGeneralQuestion)
            {
                yield return ChatStreamEvent.Status("범위 밖 질문으로 판단");
                yield return ChatStreamEvent.Token("이 챗봇은 기본적으로 BA-Studio 액티비티 매뉴얼과 솔루션 사용법 질문에 답하도록 설정되어 있습니다. 일반 지식 질문에 답하려면 아래의 `일반 질문` 체크박스를 켜고, 최신 정보가 필요하면 `웹 검색` 체크박스도 켠 뒤 다시 질문해주세요.");
                yield return ChatStreamEvent.Completed();
                yield break;
            }

            yield return ChatStreamEvent.Status("일반 질문 답변 생성");
            await foreach (var token in _llm.StreamAsync(BuildGeneralQuestionPrompt(request.Question), cancellationToken))
            {
                yield return ChatStreamEvent.Token(token);
            }

            yield return ChatStreamEvent.Completed();
            yield break;
        }

        var canUsePreviousContext = CanUsePreviousContext(intent, followUpKind, previousGrounding);
        var effectivePreviousGrounding = canUsePreviousContext ? previousGrounding : null;
        var contextualQuestion = BuildContextualQuestion(request.Question, intent, effectivePreviousGrounding, followUpKind);
        var preferredSource = ShouldUsePreviousSource(request.Question, intent, effectivePreviousGrounding, followUpKind) ? effectivePreviousGrounding?.Source : null;
        var preferredGroup = intent.PreferredGroup ?? (preferredSource is not null || canUsePreviousContext && IsContextualFollowUp(followUpKind) ? effectivePreviousGrounding?.GroupName : null);
        var activityHint = followUpKind == FollowUpKind.Comparison
            ? null
            : intent.ActivityNameHint ?? (preferredSource is not null ? effectivePreviousGrounding?.ActivityName : null);
        if (!string.Equals(contextualQuestion, request.Question, StringComparison.Ordinal))
        {
            yield return ChatStreamEvent.Status($"대화 문맥 적용: {contextualQuestion}");
        }
        if (preferredSource is not null)
        {
            yield return ChatStreamEvent.Status($"이전 근거 우선: {preferredSource}");
        }

        yield return ChatStreamEvent.Status("매뉴얼을 검색하는 중...");
        var embedding = _embeddings.Embed(contextualQuestion);
        var topK = followUpKind == FollowUpKind.Comparison ? Math.Max(request.TopK, 24) : request.TopK;
        var chunks = await _vectorStore.SearchAsync(
            new SearchRequest(contextualQuestion, embedding, topK, request.MinScore, preferredGroup, activityHint, preferredSource),
            cancellationToken);
        yield return ChatStreamEvent.Status(BuildSearchStatus(chunks));

        if (chunks.Count == 0)
        {
            yield return ChatStreamEvent.Status("검색 결과 없음");
            yield return ChatStreamEvent.Token("문서에 근거가 부족합니다. BA-Studio의 어떤 도메인이나 액티비티에 대한 질문인지 조금 더 알려주세요.");
            yield return ChatStreamEvent.Completed();
            yield break;
        }

        chunks = await AddCompanionSourcesAsync(request.Question, followUpKind, chunks, cancellationToken);
        chunks = await EnrichSourceChunksAsync(chunks, cancellationToken);

        var followUpAnswer = TryBuildFollowUpAnswer(request.Question, followUpKind, effectivePreviousGrounding, chunks);
        if (followUpAnswer is not null)
        {
            RememberGrounding(conversationId, chunks.Where(c => !string.Equals(c.Source, previousGrounding?.Source, StringComparison.OrdinalIgnoreCase)).ToArray());
            yield return ChatStreamEvent.Status("후속 질문 답변 생성");
            yield return ChatStreamEvent.Token(followUpAnswer);
            yield return ChatStreamEvent.Completed();
            yield break;
        }

        var clarification = TryBuildClarification(request.Question, intent, chunks);
        if (clarification is not null)
        {
            yield return ChatStreamEvent.Status("동일 액티비티명이 여러 도메인에 있어 확인 질문 생성");
            yield return ChatStreamEvent.Token(clarification);
            yield return ChatStreamEvent.Completed();
            yield break;
        }

        var directAnswer = TryBuildDirectAnswer(request.Question, intent, chunks);
        if (directAnswer is not null)
        {
            RememberGrounding(conversationId, chunks);
            yield return ChatStreamEvent.Status($"직접 답변 생성: {chunks[0].Source}");
            yield return ChatStreamEvent.Token(directAnswer);
            yield return ChatStreamEvent.Completed();
            yield break;
        }

        var correctionOverride = TryBuildCorrectionOverride(request.Question, chunks);
        if (correctionOverride is not null)
        {
            RememberGrounding(conversationId, chunks);
            yield return ChatStreamEvent.Status("저장된 답변 수정 내용을 우선 적용");
            yield return ChatStreamEvent.Token(correctionOverride);
            yield return ChatStreamEvent.Completed();
            yield break;
        }

        yield return ChatStreamEvent.Status("근거 기반 답변을 생성하는 중...");
        var prompt = _promptBuilder.Build(request.Question, chunks);
        await foreach (var token in _llm.StreamAsync(prompt, cancellationToken))
        {
            yield return ChatStreamEvent.Token(token);
        }

        RememberGrounding(conversationId, chunks);
        yield return ChatStreamEvent.Completed();
    }

    private static bool IsOutOfScopeQuestion(
        string question,
        DomainIntent intent,
        FollowUpKind followUpKind,
        ConversationGrounding? previousGrounding)
    {
        if (intent.PreferredGroup is not null || intent.ActivityNameHint is not null || intent.Signals.Count > 0)
        {
            return false;
        }

        if (followUpKind != FollowUpKind.None && previousGrounding is not null)
        {
            return false;
        }

        return !LooksLikeManualQuestion(question) && !LooksLikeSolutionGuideQuestion(question);
    }

    private static bool LooksLikeManualQuestion(string question)
    {
        var q = question.Trim();
        var manualTerms = new[]
        {
            "액티비티", "속성", "기본값", "옵션", "사용법", "비교", "차이", "함께", "대체",
            "스크립트", "selector", "셀렉터", "브라우저", "엑셀", "파일", "폴더", "클립보드",
            "메일", "이메일", "스레드", "멀티스레드", "프로세스", "윈도우", "마우스", "키보드"
        };

        if (manualTerms.Any(term => q.Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return Regex.IsMatch(q, @"\b[A-Za-z][A-Za-z0-9_]*(?:\([^)]+\))?\b", RegexOptions.CultureInvariant);
    }

    private static bool LooksLikeSolutionGuideQuestion(string question)
    {
        var q = question.Trim();
        var guideTerms = new[]
        {
            "챗봇", "프로그램", "솔루션", "실행", "빌드", "KB", "DB", "세션", "탭",
            "저장", "불러", "답변 수정", "가이드", "사용법", "설정", "모델"
        };

        return guideTerms.Any(term => q.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    private static string BuildGeneralQuestionPrompt(string question)
    {
        return $"""
        <|system|>
        You are a helpful assistant. Answer the user's general knowledge question directly and concisely in Korean.
        If you are not sure, say so briefly.
        <|end|>
        <|user|>
        {question}
        <|end|>
        <|assistant|>
        """;
    }

    private static string BuildWebSearchAnswer(WebSearchResult result)
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(result.Summary))
        {
            sb.AppendLine(result.Summary.Trim());
        }
        else
        {
            sb.AppendLine(result.Items[0].Snippet.Trim());
        }

        if (result.Items.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("[웹 검색 결과]");
            foreach (var item in result.Items.Take(3))
            {
                var url = string.IsNullOrWhiteSpace(item.Url) ? "" : $" - {item.Url}";
                sb.AppendLine($"- {item.Snippet}{url}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    private async Task<IReadOnlyList<RetrievedChunk>> EnrichSourceChunksAsync(
        IReadOnlyList<RetrievedChunk> chunks,
        CancellationToken cancellationToken)
    {
        var results = new List<RetrievedChunk>(chunks);
        var resultIds = results.Select(c => c.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var source in chunks.Select(c => c.Source).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var sourceChunks = await _vectorStore.GetChunksBySourceAsync(source, cancellationToken);
            foreach (var chunk in sourceChunks)
            {
                if (resultIds.Add(chunk.Id))
                {
                    results.Add(chunk);
                }
            }
        }

        return results;
    }

    private async Task<IReadOnlyList<RetrievedChunk>> AddCompanionSourcesAsync(
        string question,
        FollowUpKind followUpKind,
        IReadOnlyList<RetrievedChunk> chunks,
        CancellationToken cancellationToken)
    {
        if (followUpKind != FollowUpKind.Companion)
        {
            return chunks;
        }

        var companionSources = ResolveCompanionSources(question, chunks).ToArray();
        if (companionSources.Length == 0)
        {
            return chunks;
        }

        var results = new List<RetrievedChunk>(chunks);
        var resultIds = results.Select(c => c.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var source in companionSources)
        {
            var sourceChunks = await _vectorStore.GetChunksBySourceAsync(source, cancellationToken);
            foreach (var chunk in sourceChunks)
            {
                if (resultIds.Add(chunk.Id))
                {
                    results.Add(chunk);
                }
            }
        }

        return results;
    }

    private static IEnumerable<string> ResolveCompanionSources(string question, IReadOnlyList<RetrievedChunk> chunks)
    {
        var mentionsMultiThread = question.Contains("MultiThread", StringComparison.OrdinalIgnoreCase) ||
                                  question.Contains("Multi Thread", StringComparison.OrdinalIgnoreCase) ||
                                  chunks.Any(c => string.Equals(c.Source, "BuiltIn/MultiThread.md", StringComparison.OrdinalIgnoreCase));
        if (!mentionsMultiThread)
        {
            yield break;
        }

        yield return "COMMON/BreakThread.md";
        yield return "COMMON/GetThreadName.md";
    }

    private static string BuildSearchStatus(IReadOnlyList<RetrievedChunk> chunks)
    {
        if (chunks.Count == 0)
        {
            return "검색 결과: 0건";
        }

        var top = chunks
            .GroupBy(c => c.Source, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .Take(5)
            .Select((c, index) => $"{index + 1}. {c.Source} / {c.SectionPath} ({c.FinalScore:0.000})");
        return "검색 결과: " + string.Join(" | ", top);
    }

    private static bool CanUsePreviousContext(DomainIntent intent, FollowUpKind followUpKind, ConversationGrounding? previousGrounding)
    {
        if (previousGrounding is null || followUpKind == FollowUpKind.None)
        {
            return false;
        }

        return intent.PreferredGroup is null && intent.ActivityNameHint is null;
    }

    private static bool ShouldUsePreviousSource(string question, DomainIntent intent, ConversationGrounding? previousGrounding, FollowUpKind followUpKind)
    {
        if (previousGrounding is null || intent.PreferredGroup is not null || intent.ActivityNameHint is not null)
        {
            return false;
        }

        return followUpKind == FollowUpKind.Detail;
    }

    private static FollowUpKind ResolveFollowUpKind(string question)
    {
        var q = question.Trim();
        if (q.Contains("비교", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("차이", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("다른점", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("다른 점", StringComparison.OrdinalIgnoreCase))
        {
            return FollowUpKind.Comparison;
        }

        if (q.Contains("다른것", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("다른 것", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("비슷", StringComparison.OrdinalIgnoreCase))
        {
            return FollowUpKind.Alternative;
        }

        if (q.Contains("함께", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("같이", StringComparison.OrdinalIgnoreCase))
        {
            return FollowUpKind.Companion;
        }

        if (q.Contains("같은 기능", StringComparison.OrdinalIgnoreCase) ||
            q.Contains("대체", StringComparison.OrdinalIgnoreCase))
        {
            return FollowUpKind.Equivalent;
        }

        if (q.Length <= 20)
        {
            if (q.Contains("속성", StringComparison.OrdinalIgnoreCase) ||
                q.Contains("기본값", StringComparison.OrdinalIgnoreCase) ||
                q.Contains("옵션", StringComparison.OrdinalIgnoreCase) ||
                q.Contains("예시", StringComparison.OrdinalIgnoreCase) ||
                q.Contains("사용법", StringComparison.OrdinalIgnoreCase) ||
                q.Contains("그건", StringComparison.OrdinalIgnoreCase) ||
                q.Contains("그 액티비티", StringComparison.OrdinalIgnoreCase))
            {
                return FollowUpKind.Detail;
            }
        }

        return q.StartsWith("그 ", StringComparison.OrdinalIgnoreCase) ||
               q.StartsWith("이 ", StringComparison.OrdinalIgnoreCase) ||
               q.StartsWith("해당 ", StringComparison.OrdinalIgnoreCase)
            ? FollowUpKind.Detail
            : FollowUpKind.None;
    }

    private static bool IsContextualFollowUp(FollowUpKind followUpKind)
    {
        return followUpKind is FollowUpKind.Alternative or FollowUpKind.Comparison or FollowUpKind.Companion or FollowUpKind.Equivalent;
    }

    private static string BuildContextualQuestion(string question, DomainIntent intent, ConversationGrounding? previousGrounding, FollowUpKind followUpKind)
    {
        if (previousGrounding is null)
        {
            return question;
        }

        var activity = string.IsNullOrWhiteSpace(previousGrounding.ActivityName) ? previousGrounding.Source : previousGrounding.ActivityName;
        if (ShouldUsePreviousSource(question, intent, previousGrounding, followUpKind))
        {
            return $"{previousGrounding.GroupName} {activity} {previousGrounding.Source} {question}";
        }

        return followUpKind switch
        {
            FollowUpKind.Alternative => $"{previousGrounding.GroupName} {BaseActivityName(activity)} {activity} {question} 비슷한 기능 대체 액티비티",
            FollowUpKind.Comparison => $"{previousGrounding.GroupName} {activity} {previousGrounding.Source} {question} 정의 속성 비교",
            FollowUpKind.Companion => $"{previousGrounding.GroupName} {activity} {question} 함께 사용 관련 액티비티",
            FollowUpKind.Equivalent => $"{previousGrounding.GroupName} {activity} {question} 같은 기능 대체 액티비티",
            _ => question
        };
    }

    private static void RememberGrounding(string conversationId, IReadOnlyList<RetrievedChunk> chunks)
    {
        var best = chunks.FirstOrDefault();
        if (best is null)
        {
            return;
        }

        ConversationGroundings[conversationId] = new ConversationGrounding(best.Source, best.GroupName, best.ActivityName);
    }

    private static string? TryBuildClarification(string question, DomainIntent intent, IReadOnlyList<RetrievedChunk> chunks)
    {
        if (intent.PreferredGroup is not null)
        {
            return null;
        }

        var activityName = intent.ActivityNameHint;
        if (string.IsNullOrWhiteSpace(activityName))
        {
            return null;
        }

        var sameActivityGroups = chunks
            .Where(c => string.Equals(c.ActivityName, activityName, StringComparison.OrdinalIgnoreCase))
            .GroupBy(c => c.Source)
            .Select(g => g.First())
            .Take(5)
            .ToArray();

        if (sameActivityGroups.Length <= 1)
        {
            return null;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"어떤 {activityName} 액티비티를 찾으시나요?");
        foreach (var c in sameActivityGroups)
        {
            var summary = c.Content.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? c.Title;
            sb.AppendLine($"- {c.Source}: {summary.Trim()}");
        }

        return sb.ToString().TrimEnd();
    }

    private static string? TryBuildFollowUpAnswer(
        string question,
        FollowUpKind followUpKind,
        ConversationGrounding? previousGrounding,
        IReadOnlyList<RetrievedChunk> chunks)
    {
        if (followUpKind == FollowUpKind.None || followUpKind == FollowUpKind.Detail)
        {
            return null;
        }

        if (followUpKind == FollowUpKind.Comparison)
        {
            if (previousGrounding is null)
            {
                return TryBuildComparisonAnswer(question, chunks);
            }

            return TryBuildComparisonAnswer(previousGrounding, chunks);
        }

        if (followUpKind == FollowUpKind.Companion)
        {
            var companionAnswer = TryBuildCompanionAnswer(question, chunks);
            if (companionAnswer is not null)
            {
                return companionAnswer;
            }
        }

        if (previousGrounding is null)
        {
            return null;
        }

        var sameGroupOnly = followUpKind is FollowUpKind.Alternative or FollowUpKind.Companion;
        var previousBase = BaseActivityName(previousGrounding.ActivityName ?? previousGrounding.Source);
        var candidate = chunks
            .Where(c => c.ChunkType != "answer_correction")
            .Where(c => !string.Equals(c.Source, previousGrounding.Source, StringComparison.OrdinalIgnoreCase))
            .Where(c => !sameGroupOnly || string.Equals(c.GroupName, previousGrounding.GroupName, StringComparison.OrdinalIgnoreCase))
            .GroupBy(c => c.Source, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(c => c.ChunkType == "summary" ? 1 : 0).ThenByDescending(c => c.FinalScore).First())
            .OrderByDescending(c => string.Equals(BaseActivityName(c.ActivityName ?? c.Source), previousBase, StringComparison.OrdinalIgnoreCase) ? 2 : 0)
            .ThenByDescending(c => c.FinalScore)
            .FirstOrDefault();

        if (candidate is null)
        {
            return $"현재 검색 결과 안에서는 {FormatDisplaySource(previousGrounding.Source)}와 비슷한 다른 액티비티를 찾지 못했습니다.";
        }

        var ordered = chunks
            .Where(c => string.Equals(c.Source, candidate.Source, StringComparison.OrdinalIgnoreCase))
            .Concat(chunks.Where(c => !string.Equals(c.Source, candidate.Source, StringComparison.OrdinalIgnoreCase)))
            .ToArray();
        return TryBuildDirectAnswer("액티비티", new DomainIntent(candidate.GroupName, candidate.ActivityName, UserIntent.ActivityLookup, []), ordered);
    }

    private static string? TryBuildCompanionAnswer(string question, IReadOnlyList<RetrievedChunk> chunks)
    {
        var companionSources = ResolveCompanionSources(question, chunks).ToArray();
        if (companionSources.Length == 0)
        {
            return null;
        }

        var companions = companionSources
            .Select(source => chunks.FirstOrDefault(c => string.Equals(c.Source, source, StringComparison.OrdinalIgnoreCase) && c.ChunkType == "summary")
                              ?? chunks.FirstOrDefault(c => string.Equals(c.Source, source, StringComparison.OrdinalIgnoreCase)))
            .Where(c => c is not null)
            .Select(c => c!)
            .ToArray();
        if (companions.Length == 0)
        {
            return null;
        }

        var anchor = chunks.FirstOrDefault(c => string.Equals(c.Source, "BuiltIn/MultiThread.md", StringComparison.OrdinalIgnoreCase) && c.ChunkType == "summary")
                     ?? chunks.FirstOrDefault(c => string.Equals(c.Source, "BuiltIn/MultiThread.md", StringComparison.OrdinalIgnoreCase));
        var title = anchor is null
            ? "함께 사용할 수 있는 액티비티"
            : $"{FormatDisplaySource(anchor.Source)}와 함께 사용할 수 있는 액티비티";

        var sb = new StringBuilder();
        sb.AppendLine(title);
        foreach (var companion in companions)
        {
            sb.AppendLine($"- {FormatDisplaySource(companion.Source)} : {GetSummary(chunks.Where(c => string.Equals(c.Source, companion.Source, StringComparison.OrdinalIgnoreCase)).ToArray())}");
        }

        if (anchor is not null)
        {
            sb.AppendLine();
            sb.AppendLine("- 사용 맥락 :");
            sb.AppendLine($"  - {FormatDisplaySource(anchor.Source)} : {GetSummary(chunks.Where(c => string.Equals(c.Source, anchor.Source, StringComparison.OrdinalIgnoreCase)).ToArray())}");
            sb.AppendLine("  - `BreakThread`는 멀티 스레딩 실행을 중단할 때 사용합니다.");
            sb.AppendLine("  - `GetThreadName`은 멀티 스레딩 실행 중 현재 스레드 이름이 필요할 때 사용합니다.");
        }

        var related = chunks
            .Where(c => companionSources.Contains(c.Source, StringComparer.OrdinalIgnoreCase) ||
                        string.Equals(c.Source, anchor?.Source, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        var details = new StringBuilder();
        details.AppendLine("[근거]");
        foreach (var c in related.Select(c => $"{c.Source} / {c.SectionPath}").Distinct())
        {
            details.AppendLine($"- {c}");
        }

        return sb.ToString().TrimEnd() + DetailsMarker + details.ToString().TrimEnd();
    }

    private static string? TryBuildComparisonAnswer(string question, IReadOnlyList<RetrievedChunk> chunks)
    {
        var pair = chunks
            .Where(c => c.ChunkType != "answer_correction")
            .GroupBy(c => c.Source, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(c => c.ChunkType == "summary" ? 1 : 0).ThenByDescending(c => c.FinalScore).First())
            .OrderByDescending(c => ScoreActivityMention(question, c))
            .ThenByDescending(c => c.FinalScore)
            .Take(2)
            .ToArray();

        return pair.Length < 2 ? null : BuildComparisonAnswer(pair[0], pair[1], chunks);
    }

    private static string? TryBuildComparisonAnswer(ConversationGrounding previousGrounding, IReadOnlyList<RetrievedChunk> chunks)
    {
        var left = chunks.FirstOrDefault(c => string.Equals(c.Source, previousGrounding.Source, StringComparison.OrdinalIgnoreCase));
        var right = chunks
            .Where(c => c.ChunkType != "answer_correction")
            .Where(c => !string.Equals(c.Source, previousGrounding.Source, StringComparison.OrdinalIgnoreCase))
            .GroupBy(c => c.Source, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(c => c.ChunkType == "summary" ? 1 : 0).ThenByDescending(c => c.FinalScore).First())
            .OrderByDescending(c => c.FinalScore)
            .FirstOrDefault();

        if (left is null || right is null)
        {
            return null;
        }

        return BuildComparisonAnswer(left, right, chunks);
    }

    private static string BuildComparisonAnswer(RetrievedChunk left, RetrievedChunk right, IReadOnlyList<RetrievedChunk> chunks)
    {
        var leftChunks = chunks.Where(c => string.Equals(c.Source, left.Source, StringComparison.OrdinalIgnoreCase)).ToArray();
        var rightChunks = chunks.Where(c => string.Equals(c.Source, right.Source, StringComparison.OrdinalIgnoreCase)).ToArray();
        var leftSummary = GetSummary(leftChunks);
        var rightSummary = GetSummary(rightChunks);
        var leftProperties = BuildPropertySummaries(leftChunks).ToArray();
        var rightProperties = BuildPropertySummaries(rightChunks).ToArray();
        var leftPropertyNames = leftProperties.Select(p => p.Name).ToArray();
        var rightPropertyNames = rightProperties.Select(p => p.Name).ToArray();
        var commonProperties = leftPropertyNames
            .Intersect(rightPropertyNames, StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var leftOnlyProperties = leftPropertyNames
            .Except(rightPropertyNames, StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var rightOnlyProperties = rightPropertyNames
            .Except(leftPropertyNames, StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine($"{FormatDisplaySource(left.Source)} 와 {FormatDisplaySource(right.Source)} 비교");
        sb.AppendLine();
        sb.AppendLine("| 구분 | " + FormatDisplaySource(left.Source) + " | " + FormatDisplaySource(right.Source) + " |");
        sb.AppendLine("|---|---|---|");
        sb.AppendLine($"| 핵심 용도 | {EscapeTableCell(leftSummary)} | {EscapeTableCell(rightSummary)} |");
        sb.AppendLine($"| 선택 기준 | {EscapeTableCell(BuildUsageHint(left.Source, leftSummary, leftOnlyProperties))} | {EscapeTableCell(BuildUsageHint(right.Source, rightSummary, rightOnlyProperties))} |");
        sb.AppendLine($"| 공통 속성 | {EscapeTableCell(FormatPropertyNames(commonProperties))} | {EscapeTableCell(FormatPropertyNames(commonProperties))} |");
        sb.AppendLine($"| 전용 속성 | {EscapeTableCell(FormatPropertyNames(leftOnlyProperties))} | {EscapeTableCell(FormatPropertyNames(rightOnlyProperties))} |");
        sb.AppendLine($"| 전체 속성 | {EscapeTableCell(FormatPropertyNames(leftPropertyNames))} | {EscapeTableCell(FormatPropertyNames(rightPropertyNames))} |");
        sb.AppendLine();
        sb.AppendLine("- 쉽게 고르면 :");
        sb.AppendLine($"  - {FormatDisplaySource(left.Source)} : {BuildUsageHint(left.Source, leftSummary, leftOnlyProperties)}");
        sb.AppendLine($"  - {FormatDisplaySource(right.Source)} : {BuildUsageHint(right.Source, rightSummary, rightOnlyProperties)}");

        var notes = BuildComparisonNotes(left.Source, right.Source, leftOnlyProperties, rightOnlyProperties).ToArray();
        if (notes.Length > 0)
        {
            sb.AppendLine("- 주의할 점 :");
            foreach (var note in notes)
            {
                sb.AppendLine($"  - {note}");
            }
        }

        var details = new StringBuilder();
        details.AppendLine("[근거]");
        foreach (var c in leftChunks.Concat(rightChunks).Select(c => $"{c.Source} / {c.SectionPath}").Distinct())
        {
            details.AppendLine($"- {c}");
        }

        details.AppendLine();
        details.AppendLine("[원문]");
        foreach (var c in leftChunks.Concat(rightChunks))
        {
            details.AppendLine($"## {c.Source} / {c.SectionPath}");
            details.AppendLine(c.Content.Trim());
            details.AppendLine();
        }

        return sb.ToString().TrimEnd() + DetailsMarker + details.ToString().TrimEnd();
    }

    private static string BuildUsageHint(string source, string summary, IReadOnlyList<string> exclusiveProperties)
    {
        var text = $"{source} {summary} {string.Join(' ', exclusiveProperties)}";
        if (text.Contains("SMTP", StringComparison.OrdinalIgnoreCase) ||
            exclusiveProperties.Any(p => p.StartsWith("smtp", StringComparison.OrdinalIgnoreCase)))
        {
            return "Outlook 클라이언트에 의존하지 않고 SMTP 서버 정보와 계정으로 메일을 보낼 때 사용합니다.";
        }

        if (text.Contains("Outlook", StringComparison.OrdinalIgnoreCase))
        {
            return "PC에 설정된 Outlook 계정을 통해 메일을 보낼 때 사용합니다.";
        }

        return summary;
    }

    private static IEnumerable<string> BuildComparisonNotes(
        string leftSource,
        string rightSource,
        IReadOnlyList<string> leftOnlyProperties,
        IReadOnlyList<string> rightOnlyProperties)
    {
        foreach (var note in BuildSourceNotes(leftSource, leftOnlyProperties))
        {
            yield return note;
        }

        foreach (var note in BuildSourceNotes(rightSource, rightOnlyProperties))
        {
            yield return note;
        }
    }

    private static IEnumerable<string> BuildSourceNotes(string source, IReadOnlyList<string> exclusiveProperties)
    {
        var displaySource = FormatDisplaySource(source);
        if (exclusiveProperties.Any(p => p.StartsWith("smtp", StringComparison.OrdinalIgnoreCase)))
        {
            yield return $"{displaySource}는 `smtphost`, `smtpport`, `smtpuser`, `smtppassword`, `smtptype` 같은 SMTP 접속 정보가 필요합니다.";
        }

        if (exclusiveProperties.Any(p => p.StartsWith("imap", StringComparison.OrdinalIgnoreCase)))
        {
            yield return $"{displaySource}는 보낸 메일함 저장이 필요하면 IMAP 관련 속성도 설정해야 합니다.";
        }

        if (exclusiveProperties.Contains("cc", StringComparer.OrdinalIgnoreCase) ||
            exclusiveProperties.Contains("bcc", StringComparer.OrdinalIgnoreCase))
        {
            yield return $"{displaySource}는 `cc`, `bcc`를 별도 속성으로 제공합니다.";
        }

        if (exclusiveProperties.Contains("waiting", StringComparer.OrdinalIgnoreCase))
        {
            yield return $"{displaySource}는 Outlook 전송 완료 대기 시간(`waiting`)을 조정할 수 있습니다.";
        }
    }

    private static string EscapeTableCell(string value)
    {
        return value
            .Replace("\r", " ", StringComparison.Ordinal)
            .Replace("\n", " ", StringComparison.Ordinal)
            .Replace("|", "\\|", StringComparison.Ordinal);
    }

    private static string? TryBuildCorrectionOverride(string question, IReadOnlyList<RetrievedChunk> chunks)
    {
        var correction = chunks
            .Where(c => c.ChunkType == "answer_correction")
            .Select(c => new
            {
                Chunk = c,
                Correction = ParseCorrectionContent(c.Content)
            })
            .Where(c => c.Correction.HasChangedAnswer)
            .Select(c => new
            {
                c.Chunk,
                c.Correction,
                QuestionScore = ScoreQuestionSimilarity(question, c.Correction.Question)
            })
            .Where(c => c.QuestionScore >= 0.65)
            .OrderByDescending(c => c.QuestionScore)
            .ThenByDescending(c => c.Chunk.FinalScore)
            .FirstOrDefault();

        if (correction is null)
        {
            return null;
        }

        return correction.Correction.CorrectedAnswer;
    }

    private static string? TryBuildDirectAnswer(string question, DomainIntent intent, IReadOnlyList<RetrievedChunk> chunks)
    {
        var best = SelectBestAnswerChunk(chunks);
        var direct = intent.Intent is UserIntent.ActivityLookup or UserIntent.HowTo ||
                     question.Contains("액티비티", StringComparison.OrdinalIgnoreCase) ||
                     question.Contains("속성", StringComparison.OrdinalIgnoreCase) ||
                     question.Contains("기본값", StringComparison.OrdinalIgnoreCase) ||
                     question.Contains("옵션", StringComparison.OrdinalIgnoreCase);

        if (!direct)
        {
            return null;
        }

        var related = chunks
            .Where(c => c.Source == best.Source)
            .OrderByDescending(c => c.ChunkType is "summary" ? 1 : 0)
            .ThenByDescending(c => c.FinalScore)
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine(FormatDisplaySource(best.Source));
        var summary = related.FirstOrDefault(c => c.ChunkType == "summary")?.Content.Trim();
        if (!string.IsNullOrWhiteSpace(summary))
        {
            sb.AppendLine($"- 정의 : {summary}");
        }

        var properties = BuildPropertySummaries(related).ToArray();
        sb.AppendLine("- 속성 :");
        if (properties.Length > 0)
        {
            foreach (var property in properties)
            {
                sb.AppendLine($"  - `{property.Name}` : {property.Description}");
            }
        }
        else
        {
            sb.AppendLine("  - (속성이 없습니다)");
        }

        var details = new StringBuilder();
        details.AppendLine($"source: {best.Source}");
        details.AppendLine();
        details.AppendLine("[근거]");
        foreach (var c in related.Select(c => $"{c.Source} / {c.SectionPath}").Distinct())
        {
            details.AppendLine($"- {c}");
        }

        details.AppendLine();
        details.AppendLine("[원문]");
        foreach (var c in related)
        {
            details.AppendLine($"## {c.SectionPath}");
            details.AppendLine(c.Content.Trim());
            details.AppendLine();
        }

        return sb.ToString().TrimEnd() + DetailsMarker + details.ToString().TrimEnd();
    }

    private static string FormatDisplaySource(string source)
    {
        return source.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
            ? source[..^3]
            : source;
    }

    private static string BaseActivityName(string activityNameOrSource)
    {
        var fileName = activityNameOrSource.Split('/', '\\').Last();
        if (fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            fileName = fileName[..^3];
        }

        var paren = fileName.IndexOf('(', StringComparison.Ordinal);
        return paren > 0 ? fileName[..paren] : fileName;
    }

    private static string GetSummary(IReadOnlyList<RetrievedChunk> chunks)
    {
        return chunks.FirstOrDefault(c => c.ChunkType == "summary")?.Content.Trim()
            ?? chunks.FirstOrDefault()?.Content.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim()
            ?? "-";
    }

    private static int ScoreActivityMention(string question, RetrievedChunk chunk)
    {
        var activity = chunk.ActivityName ?? BaseActivityName(chunk.Source);
        var candidates = new[]
        {
            activity,
            BaseActivityName(activity),
            activity.EndsWith("s", StringComparison.OrdinalIgnoreCase) ? activity[..^1] : activity
        };

        return candidates
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Any(c => question.Contains(c, StringComparison.OrdinalIgnoreCase))
            ? 1
            : 0;
    }

    private static string FormatPropertyNames(IReadOnlyList<string> names)
    {
        return names.Count == 0 ? "(속성이 없습니다)" : string.Join(", ", names.Select(n => $"`{n}`"));
    }

    private static IEnumerable<PropertySummary> BuildPropertySummaries(IReadOnlyList<RetrievedChunk> chunks)
    {
        var noteProperties = chunks
            .Where(c => c.ChunkType == "property_note")
            .Select(ParsePropertyNote)
            .Where(p => p is not null)
            .Select(p => p!)
            .ToArray();
        if (noteProperties.Length > 0)
        {
            return noteProperties;
        }

        var propertiesChunk = chunks.FirstOrDefault(c => c.ChunkType == "properties");
        return propertiesChunk is null
            ? []
            : ParsePropertiesTable(propertiesChunk.Content);
    }

    private static PropertySummary? ParsePropertyNote(RetrievedChunk chunk)
    {
        var lines = chunk.Content
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();
        if (lines.Length == 0)
        {
            return null;
        }

        var name = lines[0].TrimStart('#').Trim().Trim('`');
        if (string.IsNullOrWhiteSpace(name))
        {
            name = chunk.SectionPath.Split('/').LastOrDefault() ?? "property";
        }

        var description = string.Join(" ", lines.Skip(1)).Trim();
        return string.IsNullOrWhiteSpace(description)
            ? null
            : new PropertySummary(name, description);
    }

    private static IEnumerable<PropertySummary> ParsePropertiesTable(string content)
    {
        foreach (var line in content.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!line.StartsWith('|') || line.Contains("---", StringComparison.Ordinal) || line.Contains("Name", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var cells = line.Trim('|').Split('|').Select(c => c.Trim()).ToArray();
            if (cells.Length < 5)
            {
                continue;
            }

            var name = cells[0].Trim('`');
            var description = cells[4];
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description) && description != "-")
            {
                yield return new PropertySummary(name, description);
            }
        }
    }

    private static RetrievedChunk SelectBestAnswerChunk(IReadOnlyList<RetrievedChunk> chunks)
    {
        var best = chunks[0];
        if (best.ChunkType != "answer_correction")
        {
            return best;
        }

        var manualMatch = chunks
            .Where(c => c.ChunkType != "answer_correction")
            .OrderByDescending(c => c.FinalScore)
            .FirstOrDefault();

        return manualMatch ?? best;
    }

    private static CorrectionContent ParseCorrectionContent(string content)
    {
        var question = ExtractCorrectionField(content, "Question:", "Corrected Answer:");
        var correctedAnswer = ExtractCorrectionField(content, "Corrected Answer:", "Expected Source or Search Hint:");
        var expectedSourceOrHint = ExtractCorrectionField(content, "Expected Source or Search Hint:", "Has Changed Answer:");
        var hasChangedAnswerText = ExtractCorrectionField(content, "Has Changed Answer:", "Memo:");
        var originalAnswer = ExtractCorrectionField(content, "Original Answer:", "Memo:");
        var hasChangedAnswer = bool.TryParse(hasChangedAnswerText, out var parsed)
            ? parsed
            : !string.IsNullOrWhiteSpace(correctedAnswer) &&
              !string.Equals(correctedAnswer.Trim(), originalAnswer.Trim(), StringComparison.Ordinal);

        return new CorrectionContent(question.Trim(), correctedAnswer.Trim(), expectedSourceOrHint.Trim(), originalAnswer.Trim(), hasChangedAnswer);
    }

    private static string ExtractCorrectionField(string content, string startMarker, string endMarker)
    {
        var start = content.IndexOf(startMarker, StringComparison.OrdinalIgnoreCase);
        if (start < 0)
        {
            return "";
        }

        start += startMarker.Length;
        var end = content.IndexOf(endMarker, start, StringComparison.OrdinalIgnoreCase);
        return (end < 0 ? content[start..] : content[start..end]).Trim();
    }

    private static bool LooksLikeSourcePath(string value)
    {
        return Regex.IsMatch(value.Trim(), @"^[A-Za-z0-9_가-힣-]+/[A-Za-z0-9_가-힣./-]+\.md$", RegexOptions.IgnoreCase);
    }

    private static double ScoreQuestionSimilarity(string left, string right)
    {
        var leftTokens = Tokenize(left).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var rightTokens = Tokenize(right).ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (leftTokens.Count == 0 || rightTokens.Count == 0)
        {
            return 0;
        }

        var intersection = leftTokens.Count(t => rightTokens.Contains(t));
        var union = leftTokens.Union(rightTokens, StringComparer.OrdinalIgnoreCase).Count();
        return union == 0 ? 0 : (double)intersection / union;
    }

    private static IEnumerable<string> Tokenize(string text)
    {
        foreach (Match match in Regex.Matches(text.ToLowerInvariant(), @"[A-Za-z0-9_()]+|[가-힣]+"))
        {
            yield return match.Value;
        }
    }

    private sealed record CorrectionContent(
        string Question,
        string CorrectedAnswer,
        string ExpectedSourceOrHint,
        string OriginalAnswer,
        bool HasChangedAnswer);

    private sealed record ConversationGrounding(string Source, string GroupName, string? ActivityName);

    private sealed record PropertySummary(string Name, string Description);

    private enum FollowUpKind
    {
        None,
        Detail,
        Alternative,
        Comparison,
        Companion,
        Equivalent
    }
}
