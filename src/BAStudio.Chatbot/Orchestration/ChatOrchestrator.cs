using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Policies;

namespace BAStudio.Chatbot.Orchestration;

/// <summary>
/// Orchestrates question handling across intent resolution, retrieval, answer generation, and web fallback.
/// </summary>
public sealed class ChatOrchestrator : IChatOrchestrator
{
    private const string DetailsMarker = "\n<<<DETAILS>>>\n";
    private static readonly ConcurrentDictionary<string, ConversationGrounding> ConversationGroundings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly string[] BAStudioGuideSources =
    [
        "docs/product-manuals/guides/ba-studio/2.6.0/activity-add-edit.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/breakpoint-use.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/code-editor-use.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/debug-task-project.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/image-automation.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/library-create.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/logs-debug-window.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/package-export.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/predefined-process-use.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/project-create.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/project-library-use.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/project-run.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/properties-edit.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/resource-manage.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/start-task-set.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/task-create.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/task-run.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/variables-use.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/web-selector-use.md",
        "docs/product-manuals/guides/ba-studio/2.6.0/window-selector-use.md"
    ];

    private readonly IEmbeddingService _embeddings;
    private readonly IVectorStore _vectorStore;
    private readonly IPromptBuilder _promptBuilder;
    private readonly ILlmService _llm;
    private readonly IWebSearchService? _webSearch;
    private readonly DomainIntentResolver _intentResolver;

    /// <summary>
    /// Creates a chat orchestrator with retrieval, prompt, LLM, and optional web search services.
    /// </summary>
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

    /// <summary>
    /// Handles a user request and streams process updates plus answer text.
    /// </summary>
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
        intent = ApplyQuestionTypeOverride(request.Question, request.QuestionType, intent);
        yield return ChatStreamEvent.Status(
            $"의도 분석: type={request.QuestionType}, intent={intent.Intent}, group={intent.PreferredGroup ?? "-"}, activity={intent.ActivityNameHint ?? "-"}");
        var conversationId = string.IsNullOrWhiteSpace(request.ConversationId) ? "default" : request.ConversationId;
        ConversationGroundings.TryGetValue(conversationId, out var previousGrounding);
        var followUpKind = ResolveFollowUpKind(request.Question);
        if (request.QuestionType == ChatQuestionType.General)
        {
            if (request.AllowWebSearch)
            {
                yield return ChatStreamEvent.Status("일반 질문 웹 검색 중...");
                var webResult = _webSearch is null
                    ? new WebSearchResult(request.Question, "", [], IsConnected: false, "웹 검색 서비스가 설정되어 있지 않습니다.")
                    : await _webSearch.SearchAsync(request.Question, cancellationToken);

                if (webResult.IsConnected && (!string.IsNullOrWhiteSpace(webResult.Summary) || webResult.Items.Count > 0))
                {
                    yield return ChatStreamEvent.Status("웹 검색 기반 답변 생성");
                    yield return ChatStreamEvent.Token(BuildWebSearchAnswer(webResult));
                    yield return ChatStreamEvent.Completed();
                    yield break;
                }

                yield return ChatStreamEvent.Status("웹 검색 실패 또는 결과 없음");
            }

            yield return ChatStreamEvent.Status("일반 질문 답변 생성");
            await foreach (var token in _llm.StreamAsync(BuildGeneralQuestionPrompt(request.Question), cancellationToken))
            {
                yield return ChatStreamEvent.Token(token);
            }

            yield return ChatStreamEvent.Completed();
            yield break;
        }

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
        var searchQuery = string.IsNullOrWhiteSpace(intent.RewrittenQuery)
            ? contextualQuestion
            : BuildSearchQuery(contextualQuestion, intent.RewrittenQuery);
        if (!string.Equals(contextualQuestion, request.Question, StringComparison.Ordinal))
        {
            yield return ChatStreamEvent.Status($"대화 문맥 적용: {contextualQuestion}");
        }
        if (!string.Equals(searchQuery, contextualQuestion, StringComparison.Ordinal))
        {
            yield return ChatStreamEvent.Status($"검색어 재작성: {searchQuery}");
        }
        if (preferredSource is not null)
        {
            yield return ChatStreamEvent.Status($"이전 근거 우선: {preferredSource}");
        }
        if (intent.PreferredSourceHint is not null)
        {
            yield return ChatStreamEvent.Status($"추천 후보 힌트: {intent.PreferredSourceHint}");
        }

        yield return ChatStreamEvent.Status("매뉴얼을 검색하는 중...");
        var embedding = _embeddings.Embed(searchQuery);
        var topK = intent.Intent == UserIntent.ProductGuide
            ? Math.Max(request.TopK, 30)
            : followUpKind == FollowUpKind.Comparison ? Math.Max(request.TopK, 24) : request.TopK;
        var sourceTypes = ResolveSourceTypes(intent);
        yield return ChatStreamEvent.Status($"검색 범위: {FormatSourceTypes(sourceTypes)}");
        var chunks = await _vectorStore.SearchAsync(
            new SearchRequest(
                searchQuery,
                embedding,
                topK,
                request.MinScore,
                preferredGroup,
                activityHint,
                preferredSource,
                request.Question,
                intent.RequiredGroup,
                intent.PreferredSourceHint,
                intent.ActionConceptHints,
                sourceTypes),
            cancellationToken);
        yield return ChatStreamEvent.Status(BuildSearchStatus(chunks));

        if (chunks.Count == 0)
        {
            yield return ChatStreamEvent.Status("검색 결과 없음");
            yield return ChatStreamEvent.Token("문서에 근거가 부족합니다. BA-Studio의 어떤 도메인이나 액티비티에 대한 질문인지 조금 더 알려주세요.");
            yield return ChatStreamEvent.Completed();
            yield break;
        }

        chunks = await AddProductGuideSourcesAsync(intent, chunks, cancellationToken);
        chunks = await AddCompanionSourcesAsync(request.Question, followUpKind, chunks, cancellationToken);
        chunks = await EnrichSourceChunksAsync(chunks, cancellationToken);

        var productGuideAnswer = TryBuildProductGuideAnswer(request.Question, intent, chunks);
        if (productGuideAnswer is not null)
        {
            RememberGrounding(conversationId, chunks);
            yield return ChatStreamEvent.Status("제품 매뉴얼 답변 생성");
            yield return ChatStreamEvent.Token(productGuideAnswer);
            yield return ChatStreamEvent.Completed();
            yield break;
        }

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

        var productTerms = new[] { "BA-Studio", "BA Studio", "스튜디오", "BA-Assist", "BA Assist", "어시스트", "BA-Server", "BA-Worker" };
        return guideTerms.Any(term => q.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
               productTerms.Any(term => q.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    private static IReadOnlyList<string> ResolveSourceTypes(DomainIntent intent)
    {
        return intent.Intent == UserIntent.ProductGuide
            ? ["product_guide", "product_manual"]
            : ["activity_manual", "qa_correction"];
    }

    private static DomainIntent ApplyQuestionTypeOverride(string question, ChatQuestionType questionType, DomainIntent intent)
    {
        return questionType switch
        {
            ChatQuestionType.ActivityTask when intent.Intent == UserIntent.ProductGuide => new DomainIntent(
                PreferredGroup: null,
                ActivityNameHint: null,
                Intent: UserIntent.ActivityLookup,
                Signals: [],
                RewrittenQuery: question,
                RequiredGroup: null,
                PreferredSourceHint: null,
                ActionConceptHints: []),
            ChatQuestionType.BAStudioGuide => BuildProductGuideIntent(question, "BA-Studio", null),
            ChatQuestionType.BAAssistGuide => BuildProductGuideIntent(question, "BA-Assist", ResolveBAAssistGuideSource(question)),
            ChatQuestionType.General => new DomainIntent(null, null, UserIntent.OutOfScope, [], question),
            ChatQuestionType.Auto => intent,
            _ => intent
        };
    }

    private static DomainIntent BuildProductGuideIntent(string question, string product, string? source)
    {
        return new DomainIntent(
            PreferredGroup: null,
            ActivityNameHint: null,
            Intent: UserIntent.ProductGuide,
            Signals: ["ProductManual", product],
            RewrittenQuery: $"{question} {product} 제품 매뉴얼 사용자 매뉴얼",
            RequiredGroup: null,
            PreferredSourceHint: source,
            ActionConceptHints: []);
    }

    private static string ResolveBAAssistGuideSource(string question)
    {
        return question.Contains("BA-Server", StringComparison.OrdinalIgnoreCase) ||
               question.Contains("BA Server", StringComparison.OrdinalIgnoreCase) ||
               question.Contains("BA-Worker", StringComparison.OrdinalIgnoreCase) ||
               question.Contains("BA Worker", StringComparison.OrdinalIgnoreCase) ||
               question.Contains("Queue", StringComparison.OrdinalIgnoreCase) ||
               question.Contains("Sequential", StringComparison.OrdinalIgnoreCase) ||
               question.Contains("logs.db", StringComparison.OrdinalIgnoreCase)
            ? "docs/product-manuals/normalized/ba-assist/2.5.0-appendix.md"
            : "docs/product-manuals/normalized/ba-assist/2.5.0.md";
    }

    private static string FormatSourceTypes(IReadOnlyList<string> sourceTypes)
    {
        return string.Join(", ", sourceTypes);
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

    private static string BuildSearchQuery(string contextualQuestion, string rewrittenQuery)
    {
        if (string.IsNullOrWhiteSpace(rewrittenQuery) ||
            string.Equals(contextualQuestion, rewrittenQuery, StringComparison.Ordinal))
        {
            return contextualQuestion;
        }

        return $"{contextualQuestion} {rewrittenQuery}";
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

    private async Task<IReadOnlyList<RetrievedChunk>> AddProductGuideSourcesAsync(
        DomainIntent intent,
        IReadOnlyList<RetrievedChunk> chunks,
        CancellationToken cancellationToken)
    {
        if (intent.Intent != UserIntent.ProductGuide ||
            !intent.Signals.Any(signal => string.Equals(signal, "BA-Studio", StringComparison.OrdinalIgnoreCase)))
        {
            return chunks;
        }

        var results = new List<RetrievedChunk>(chunks);
        var resultIds = results.Select(c => c.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var source in BAStudioGuideSources)
        {
            var sourceChunks = await _vectorStore.GetChunksBySourceAsync(source, cancellationToken);
            if (sourceChunks.Count == 0)
            {
                sourceChunks = LoadGuideChunkFromFile(source);
            }

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

    private static IReadOnlyList<RetrievedChunk> LoadGuideChunkFromFile(string source)
    {
        var root = FindRepoRoot();
        var path = Path.Combine(root, source.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(path))
        {
            return [];
        }

        var text = File.ReadAllText(path);
        var title = Regex.Match(text, @"^#\s+(.+)$", RegexOptions.Multiline).Groups[1].Value.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            title = Path.GetFileNameWithoutExtension(path);
        }

        var product = ExtractGuideMetadataValue(text, "Product") ?? "BA-Studio";
        var version = ExtractGuideMetadataValue(text, "Version") ?? "2.6.0";
        var topic = ExtractGuideMetadataValue(text, "Topic") ?? Path.GetFileNameWithoutExtension(path);
        var content = $"""
        Product: {product}
        Version: {version}
        Topic: {topic}
        Source: {source}

        {text.Trim()}
        """;

        return
        [
            new RetrievedChunk(
                Id: $"guide-file:{source}",
                Source: source,
                GroupName: product,
                ActivityName: null,
                Title: title,
                SectionPath: title,
                ChunkType: "product_guide",
                Content: content,
                VectorScore: 0,
                KeywordScore: 0,
                FinalScore: 0,
                SourceType: "product_guide")
        ];
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "commands.json")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return Directory.GetCurrentDirectory();
    }

    private static string? ExtractGuideMetadataValue(string text, string name)
    {
        var match = Regex.Match(text, $@"^(?:-\s*)?{Regex.Escape(name)}:\s*(?<value>.+?)\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        return match.Success ? match.Groups["value"].Value.Trim() : null;
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

    private static string? TryBuildProductGuideAnswer(string question, DomainIntent intent, IReadOnlyList<RetrievedChunk> chunks)
    {
        if (intent.Intent != UserIntent.ProductGuide)
        {
            return null;
        }

        var includeSourceSupplement = LooksLikeSourceSupplementQuestion(question);
        var guideAnswer = TryBuildProductGuideDocumentAnswer(question, chunks);
        if (guideAnswer is not null)
        {
            return guideAnswer;
        }

        var related = chunks
            .Where(c => string.Equals(c.SourceType, "product_manual", StringComparison.OrdinalIgnoreCase))
            .GroupBy(c => c.SectionPath, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(c => c.FinalScore).First())
            .Where(c => includeSourceSupplement || !IsSourceSupplementSection(c))
            .OrderByDescending(c => ScoreProductGuideSection(question, c, includeSourceSupplement))
            .ThenByDescending(c => c.FinalScore)
            .Take(3)
            .ToArray();
        if (related.Length == 0)
        {
            related = chunks
                .Where(c => string.Equals(c.SourceType, "product_manual", StringComparison.OrdinalIgnoreCase))
                .GroupBy(c => c.SectionPath, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.OrderByDescending(c => c.FinalScore).First())
                .OrderByDescending(c => ScoreProductGuideSection(question, c, includeSourceSupplement: true))
                .ThenByDescending(c => c.FinalScore)
                .Take(3)
                .ToArray();
            if (related.Length == 0)
            {
                return null;
            }
        }

        var best = related[0];
        var directProcedure = TryBuildProductProcedureAnswer(question, best, related);
        if (directProcedure is not null)
        {
            return directProcedure;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"{best.GroupName} 제품 매뉴얼 기준으로 보면:");
        foreach (var chunk in related)
        {
            var summary = BuildProductSectionSummary(chunk.Content);
            if (string.IsNullOrWhiteSpace(summary))
            {
                continue;
            }

            sb.AppendLine($"- {FormatProductSectionPath(chunk.SectionPath)}: {summary}");
        }

        var imageRefs = related
            .SelectMany(c => ExtractMarkdownImages(c.Content))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(3)
            .ToArray();
        if (imageRefs.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine("관련 화면:");
            foreach (var image in imageRefs)
            {
                sb.AppendLine($"- {image}");
            }
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
            details.AppendLine($"## {c.Source} / {c.SectionPath}");
            details.AppendLine(c.Content.Trim());
            details.AppendLine();
        }

        return sb.ToString().TrimEnd() + DetailsMarker + details.ToString().TrimEnd();
    }

    private static string? TryBuildProductGuideDocumentAnswer(string question, IReadOnlyList<RetrievedChunk> chunks)
    {
        var guide = chunks
            .Where(c => string.Equals(c.SourceType, "product_guide", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(c => ScoreProductGuideDocument(question, c))
            .FirstOrDefault();
        if (guide is null)
        {
            return null;
        }

        var shortAnswer = ExtractMarkdownSection(guide.Content, "Short Answer");
        var steps = ExtractMarkdownSection(guide.Content, "Steps");
        var notes = ExtractMarkdownSection(guide.Content, "Notes");
        var exampleAnswer = ExtractMarkdownSection(guide.Content, "Example Answer");
        if (string.IsNullOrWhiteSpace(shortAnswer) && string.IsNullOrWhiteSpace(steps) && string.IsNullOrWhiteSpace(exampleAnswer))
        {
            return null;
        }

        var sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(shortAnswer))
        {
            sb.AppendLine(shortAnswer.Trim());
        }

        if (!string.IsNullOrWhiteSpace(steps))
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            sb.AppendLine("절차:");
            foreach (var line in NormalizeGuideListLines(steps).Take(8))
            {
                sb.AppendLine(line);
            }
        }
        else if (!string.IsNullOrWhiteSpace(exampleAnswer))
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            sb.AppendLine(exampleAnswer.Trim());
        }

        var noteLines = NormalizeGuideListLines(notes)
            .Where(line => !line.Contains(".cs", StringComparison.OrdinalIgnoreCase))
            .Take(4)
            .ToArray();
        if (noteLines.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine("참고:");
            foreach (var line in noteLines)
            {
                sb.AppendLine(line);
            }
        }

        var related = chunks
            .Where(c => string.Equals(c.Source, guide.Source, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(c.SourceType, "product_manual", StringComparison.OrdinalIgnoreCase))
            .Take(4)
            .ToArray();

        var details = new StringBuilder();
        details.AppendLine($"source: {guide.Source}");
        details.AppendLine();
        details.AppendLine("[근거]");
        details.AppendLine($"- {guide.Source} / {guide.SectionPath}");
        foreach (var c in related.Where(c => !string.Equals(c.Source, guide.Source, StringComparison.OrdinalIgnoreCase)).Select(c => $"{c.Source} / {c.SectionPath}").Distinct())
        {
            details.AppendLine($"- {c}");
        }

        details.AppendLine();
        details.AppendLine("[원문]");
        details.AppendLine($"## {guide.Source} / {guide.SectionPath}");
        details.AppendLine(guide.Content.Trim());

        return sb.ToString().TrimEnd() + DetailsMarker + details.ToString().TrimEnd();
    }

    private static string? TryBuildProductProcedureAnswer(string question, RetrievedChunk best, IReadOnlyList<RetrievedChunk> related)
    {
        if (!LooksLikeProjectCreationQuestion(question, best))
        {
            return null;
        }

        var source = related.FirstOrDefault(c => c.Content.Contains("File > New > New Project", StringComparison.OrdinalIgnoreCase)) ?? best;
        var evidence = new[] { source };
        var sb = new StringBuilder();
        sb.AppendLine($"{source.GroupName}에서 프로젝트를 생성하는 기본 절차는 다음과 같습니다.");
        sb.AppendLine();
        sb.AppendLine("1. 시작 페이지의 `New Project`를 선택하거나, 메인 메뉴에서 `File > New > New Project`를 선택합니다.");
        sb.AppendLine("2. 프로젝트 이름과 저장 경로를 지정합니다.");
        sb.AppendLine("3. 프로젝트가 생성되면 Project Tree에 기본 `Task1`이 만들어집니다.");
        sb.AppendLine("4. 작업을 나누어 구성하려면 Project Tree에서 프로젝트를 우클릭한 뒤 `Add Task`로 태스크를 추가합니다.");
        sb.AppendLine("5. 필요한 경우 각 Task를 우클릭해 `Rename`으로 이름을 바꾸고, 시작 지점으로 사용할 Task를 `Set Start Task`로 지정합니다.");
        sb.AppendLine();
        sb.AppendLine("프로젝트 생성 자체만 보면 1-2번이 핵심이고, 3번 이후는 생성된 프로젝트를 실제 자동화 작업으로 구성하는 단계입니다.");

        var imageRefs = evidence
            .SelectMany(c => ExtractMarkdownImages(c.Content))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(2)
            .ToArray();
        if (imageRefs.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine("관련 화면:");
            foreach (var image in imageRefs)
            {
                sb.AppendLine($"- {image}");
            }
        }

        var details = new StringBuilder();
        details.AppendLine($"source: {source.Source}");
        details.AppendLine();
        details.AppendLine("[근거]");
        foreach (var c in evidence.Select(c => $"{c.Source} / {c.SectionPath}").Distinct())
        {
            details.AppendLine($"- {c}");
        }

        details.AppendLine();
        details.AppendLine("[원문]");
        foreach (var c in evidence)
        {
            details.AppendLine($"## {c.Source} / {c.SectionPath}");
            details.AppendLine(c.Content.Trim());
            details.AppendLine();
        }

        return sb.ToString().TrimEnd() + DetailsMarker + details.ToString().TrimEnd();
    }

    private static double ScoreProductGuideDocument(string question, RetrievedChunk chunk)
    {
        var queryTokens = Tokenize(question)
            .Select(NormalizeGuideToken)
            .Where(t => t.Length > 1)
            .Where(t => !IsGuideStopToken(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (queryTokens.Length == 0)
        {
            return chunk.FinalScore;
        }

        var haystack = $"{chunk.Title} {chunk.SectionPath} {ExtractMarkdownSection(chunk.Content, "User Intent")} {ExtractMarkdownSection(chunk.Content, "Related Keywords")}";
        haystack = NormalizeGuideToken(haystack);
        var hits = queryTokens.Count(token => haystack.Contains(token, StringComparison.OrdinalIgnoreCase));
        var coverage = (double)hits / queryTokens.Length;
        var titleHits = queryTokens.Count(token => chunk.Title.Contains(token, StringComparison.OrdinalIgnoreCase));
        return coverage * 3.0 + titleHits * 0.5 + ScoreGuideTopicConcept(question, chunk) + chunk.FinalScore * 0.03;
    }

    private static double ScoreGuideTopicConcept(string question, RetrievedChunk chunk)
    {
        var q = question.ToLowerInvariant();
        var topic = $"{chunk.Source} {chunk.Title} {ExtractGuideMetadataValue(chunk.Content, "Topic")}".ToLowerInvariant();
        var concepts = new (string[] QueryTerms, string[] TopicTerms)[]
        {
            (["디버그", "디버깅", "debug"], ["debug"]),
            (["중단점", "breakpoint", "break point"], ["breakpoint"]),
            (["라이브러리", "library"], ["library"]),
            (["프로젝트", "project"], ["project"]),
            (["태스크", "task"], ["task"]),
            (["변수", "variable"], ["variable"]),
            (["이미지", "image", "캡처", "capture"], ["image"]),
            (["셀렉터", "selector"], ["selector"]),
            (["패키지", "package", "내보내"], ["package", "export"]),
            (["속성", "property", "properties"], ["properties"]),
            (["리소스", "resource"], ["resource"])
        };

        var score = 0.0;
        foreach (var (queryTerms, topicTerms) in concepts)
        {
            if (queryTerms.Any(term => q.Contains(term, StringComparison.OrdinalIgnoreCase)) &&
                topicTerms.Any(term => topic.Contains(term, StringComparison.OrdinalIgnoreCase)))
            {
                score += 1.4;
            }
        }

        return score;
    }

    private static string NormalizeGuideToken(string value)
    {
        var normalized = value.ToLowerInvariant();
        var endings = new[] { "시켜주는", "시키는", "하려고", "되는", "하는", "한다", "하기", "하려", "된", "할" };
        foreach (var ending in endings)
        {
            normalized = Regex.Replace(normalized, $"{Regex.Escape(ending)}(?=\\s|$)", "", RegexOptions.CultureInvariant);
        }

        var particles = new[] { "으로부터", "로부터", "에서는", "에서", "에게", "으로", "로", "의", "을", "를", "은", "는", "이", "가", "에" };
        foreach (var particle in particles)
        {
            normalized = Regex.Replace(normalized, $"{Regex.Escape(particle)}(?=\\s|$)", "", RegexOptions.CultureInvariant);
        }

        return normalized.Trim();
    }

    private static bool IsGuideStopToken(string token)
    {
        return token is "방법" or "사용법" or "알려줘" or "어떻게" or "제품" or "매뉴얼" or "사용자" or "ba" or "studio";
    }

    private static string ExtractMarkdownSection(string content, string heading)
    {
        var match = Regex.Match(
            content,
            $@"^##\s+{Regex.Escape(heading)}\s*$\r?\n(?<content>.*?)(?=^##\s+|\z)",
            RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        return match.Success ? match.Groups["content"].Value.Trim() : string.Empty;
    }

    private static IEnumerable<string> NormalizeGuideListLines(string content)
    {
        foreach (var raw in content.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            if (Regex.IsMatch(line, @"^\d+\.\s+"))
            {
                yield return line;
                continue;
            }

            var bullet = Regex.Replace(line, @"^[-*]\s+", "- ");
            yield return bullet.StartsWith("- ", StringComparison.Ordinal) ? bullet : $"- {bullet}";
        }
    }

    private static string BuildProductSectionSummary(string content)
    {
        var lines = content
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(line => !line.StartsWith("Product:", StringComparison.OrdinalIgnoreCase))
            .Where(line => !line.StartsWith("Version:", StringComparison.OrdinalIgnoreCase))
            .Where(line => !line.StartsWith("Section:", StringComparison.OrdinalIgnoreCase))
            .Where(line => !line.StartsWith("![", StringComparison.Ordinal))
            .Where(line => !line.StartsWith("|", StringComparison.Ordinal))
            .Where(line => !line.Contains(".cs", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        var text = string.Join(" ", lines).Trim();
        if (text.Length <= 260)
        {
            return text;
        }

        var boundary = text.LastIndexOfAny(['.', '?', '!'], Math.Min(text.Length - 1, 260));
        return text[..(boundary > 80 ? boundary + 1 : 260)].Trim();
    }

    private static double ScoreProductGuideSection(string question, RetrievedChunk chunk, bool includeSourceSupplement)
    {
        var queryTokens = Tokenize(question)
            .Where(t => t.Length > 1)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (queryTokens.Length == 0)
        {
            return chunk.FinalScore;
        }

        var haystack = $"{chunk.SectionPath} {chunk.Content}";
        var hits = queryTokens.Count(token => haystack.Contains(token, StringComparison.OrdinalIgnoreCase));
        var score = (double)hits / queryTokens.Length + chunk.FinalScore * 0.05;

        if (IsSourceSupplementSection(chunk))
        {
            score += includeSourceSupplement ? 0.15 : -0.4;
        }

        var section = chunk.SectionPath;
        if (LooksLikeProjectCreationQuestion(question, chunk))
        {
            if (section.Contains("Project 구현하기", StringComparison.OrdinalIgnoreCase))
            {
                score += 0.45;
            }

            if (chunk.Content.Contains("File > New > New Project", StringComparison.OrdinalIgnoreCase) ||
                chunk.Content.Contains("New Project", StringComparison.OrdinalIgnoreCase))
            {
                score += 0.35;
            }
        }

        if (section.Contains("목차", StringComparison.OrdinalIgnoreCase))
        {
            score -= 0.35;
        }

        return score;
    }

    private static bool LooksLikeProjectCreationQuestion(string question, RetrievedChunk chunk)
    {
        var q = question.ToLowerInvariant();
        var asksProject = q.Contains("프로젝트", StringComparison.OrdinalIgnoreCase) ||
                          q.Contains("project", StringComparison.OrdinalIgnoreCase);
        var asksCreation = q.Contains("생성", StringComparison.OrdinalIgnoreCase) ||
                           q.Contains("만들", StringComparison.OrdinalIgnoreCase) ||
                           q.Contains("create", StringComparison.OrdinalIgnoreCase) ||
                           q.Contains("new project", StringComparison.OrdinalIgnoreCase);
        return asksProject && asksCreation;
    }

    private static bool LooksLikeSourceSupplementQuestion(string question)
    {
        var q = question.ToLowerInvariant();
        var sourceDetailTerms = new[]
        {
            "파일", "폴더", "경로", "저장", "로드", "불러", "설정값", "config", "json",
            "생성되", "만들어지", "위치", "구조", "다중 인스턴스", "parallel", "queue"
        };
        return sourceDetailTerms.Any(term => q.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsSourceSupplementSection(RetrievedChunk chunk)
    {
        return chunk.SectionPath.Contains("Source Code Supplement", StringComparison.OrdinalIgnoreCase) ||
               chunk.SectionPath.Contains("Source-Confirmed", StringComparison.OrdinalIgnoreCase);
    }

    private static string FormatProductSectionPath(string sectionPath)
    {
        return sectionPath
            .Replace("Source Code Supplement > ", "", StringComparison.OrdinalIgnoreCase)
            .Replace("Source-Confirmed ", "", StringComparison.OrdinalIgnoreCase);
    }

    private static IEnumerable<string> ExtractMarkdownImages(string content)
    {
        foreach (Match match in Regex.Matches(content, @"!\[(?<alt>[^\]]*)\]\((?<path>[^)]+)\)"))
        {
            var alt = match.Groups["alt"].Value.Trim();
            var path = match.Groups["path"].Value.Trim();
            yield return string.IsNullOrWhiteSpace(alt) ? path : $"{alt} ({path})";
        }
    }

    private static string? TryBuildDirectAnswer(string question, DomainIntent intent, IReadOnlyList<RetrievedChunk> chunks)
    {
        if (intent.Intent == UserIntent.ProductGuide)
        {
            return null;
        }

        var best = SelectBestAnswerChunk(chunks);
        var recommendationSupported =
            intent.Intent == UserIntent.ActivityRecommendation &&
            (intent.PreferredSourceHint is not null && chunks.Any(c => string.Equals(c.Source, intent.PreferredSourceHint, StringComparison.OrdinalIgnoreCase)) ||
             intent.ActionConceptHints is { Count: > 0 } && string.Equals(best.GroupName, intent.PreferredGroup, StringComparison.OrdinalIgnoreCase) ||
             intent.RequiredGroup is not null && string.Equals(best.GroupName, intent.RequiredGroup, StringComparison.OrdinalIgnoreCase));
        var direct = intent.Intent is UserIntent.ActivityLookup or UserIntent.HowTo ||
                     recommendationSupported ||
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
