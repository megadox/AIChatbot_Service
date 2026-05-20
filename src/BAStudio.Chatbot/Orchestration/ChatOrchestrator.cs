using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Policies;

namespace BAStudio.Chatbot.Orchestration;

public sealed class ChatOrchestrator : IChatOrchestrator
{
    private static readonly ConcurrentDictionary<string, ConversationGrounding> ConversationGroundings = new(StringComparer.OrdinalIgnoreCase);

    private readonly IEmbeddingService _embeddings;
    private readonly IVectorStore _vectorStore;
    private readonly IPromptBuilder _promptBuilder;
    private readonly ILlmService _llm;
    private readonly DomainIntentResolver _intentResolver;

    public ChatOrchestrator(
        IEmbeddingService embeddings,
        IVectorStore vectorStore,
        IPromptBuilder promptBuilder,
        ILlmService llm,
        DomainIntentResolver? intentResolver = null)
    {
        _embeddings = embeddings;
        _vectorStore = vectorStore;
        _promptBuilder = promptBuilder;
        _llm = llm;
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
        var contextualQuestion = BuildContextualQuestion(request.Question, intent, previousGrounding);
        var preferredSource = ShouldUsePreviousSource(request.Question, intent, previousGrounding) ? previousGrounding?.Source : null;
        var preferredGroup = intent.PreferredGroup ?? (preferredSource is not null ? previousGrounding?.GroupName : null);
        var activityHint = intent.ActivityNameHint ?? (preferredSource is not null ? previousGrounding?.ActivityName : null);
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
        var chunks = await _vectorStore.SearchAsync(
            new SearchRequest(contextualQuestion, embedding, request.TopK, request.MinScore, preferredGroup, activityHint, preferredSource),
            cancellationToken);
        yield return ChatStreamEvent.Status(BuildSearchStatus(chunks));

        if (chunks.Count == 0)
        {
            yield return ChatStreamEvent.Status("검색 결과 없음");
            yield return ChatStreamEvent.Token("문서에 근거가 부족합니다. BA-Studio의 어떤 도메인이나 액티비티에 대한 질문인지 조금 더 알려주세요.");
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

        yield return ChatStreamEvent.Status("근거 기반 답변을 생성하는 중...");
        var prompt = _promptBuilder.Build(request.Question, chunks);
        await foreach (var token in _llm.StreamAsync(prompt, cancellationToken))
        {
            yield return ChatStreamEvent.Token(token);
        }

        RememberGrounding(conversationId, chunks);
        yield return ChatStreamEvent.Completed();
    }

    private static string BuildSearchStatus(IReadOnlyList<RetrievedChunk> chunks)
    {
        if (chunks.Count == 0)
        {
            return "검색 결과: 0건";
        }

        var top = chunks
            .Take(5)
            .Select((c, index) => $"{index + 1}. {c.Source} / {c.SectionPath} ({c.FinalScore:0.000})");
        return "검색 결과: " + string.Join(" | ", top);
    }

    private static bool ShouldUsePreviousSource(string question, DomainIntent intent, ConversationGrounding? previousGrounding)
    {
        if (previousGrounding is null || intent.PreferredGroup is not null || intent.ActivityNameHint is not null)
        {
            return false;
        }

        return IsFollowUpQuestion(question);
    }

    private static bool IsFollowUpQuestion(string question)
    {
        var q = question.Trim();
        if (q.Length <= 20)
        {
            return q.Contains("속성", StringComparison.OrdinalIgnoreCase) ||
                   q.Contains("기본값", StringComparison.OrdinalIgnoreCase) ||
                   q.Contains("옵션", StringComparison.OrdinalIgnoreCase) ||
                   q.Contains("예시", StringComparison.OrdinalIgnoreCase) ||
                   q.Contains("사용법", StringComparison.OrdinalIgnoreCase) ||
                   q.Contains("그건", StringComparison.OrdinalIgnoreCase) ||
                   q.Contains("그 액티비티", StringComparison.OrdinalIgnoreCase);
        }

        return q.StartsWith("그 ", StringComparison.OrdinalIgnoreCase) ||
               q.StartsWith("이 ", StringComparison.OrdinalIgnoreCase) ||
               q.StartsWith("해당 ", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildContextualQuestion(string question, DomainIntent intent, ConversationGrounding? previousGrounding)
    {
        if (!ShouldUsePreviousSource(question, intent, previousGrounding) || previousGrounding is null)
        {
            return question;
        }

        var activity = string.IsNullOrWhiteSpace(previousGrounding.ActivityName) ? previousGrounding.Source : previousGrounding.ActivityName;
        return $"{previousGrounding.GroupName} {activity} {previousGrounding.Source} {question}";
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
            .Take(4)
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine($"{best.Source} 기준으로 확인한 내용입니다.");
        sb.AppendLine();

        foreach (var c in related)
        {
            if (c.ChunkType == "summary")
            {
                sb.AppendLine(c.Content.Trim());
                sb.AppendLine();
            }
            else if (c.ChunkType == "answer_correction")
            {
                var correction = ParseCorrectionContent(c.Content);
                if (correction.HasChangedAnswer)
                {
                    sb.AppendLine(correction.CorrectedAnswer);
                    sb.AppendLine();
                }
            }
            else if (c.ChunkType is "properties" or "property_note")
            {
                sb.AppendLine(c.Content.Trim());
                sb.AppendLine();
            }
        }

        sb.AppendLine("[근거]");
        foreach (var c in related.Select(c => $"{c.Source} / {c.SectionPath}").Distinct())
        {
            sb.AppendLine($"- {c}");
        }

        return sb.ToString().TrimEnd();
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
}
