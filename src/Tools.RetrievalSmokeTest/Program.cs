using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Infra.Embedding;
using BAStudio.Chatbot.Infra.VectorStore;
using BAStudio.Chatbot.Orchestration;
using BAStudio.Chatbot.Policies;
using BAStudio.Chatbot.Prompting;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

var root = FindRepoRoot();
var casesPath = GetArg(args, "--cases");
var allActivities = HasArg(args, "--all-activities");
var docsPath = GetArg(args, "--docs") ?? Path.Combine(root, "docs", "generated", "commands");
var outputPath = GetArg(args, "--output");
var preferredSource = GetArg(args, "--source");
var questionType = ParseQuestionType(GetArg(args, "--question-type"));
var questionArgs = GetQuestionArgs(args).ToArray();
var question = questionArgs.Length > 0 ? string.Join(' ', questionArgs) : "웹 Click 액티비티 사용법 알려줘";
var dbPath = Path.Combine(root, "ChatBot", "ba_manual_vector.db");

var embeddings = new HashEmbeddingService();
var store = new SqliteVectorStore(dbPath);
var intentResolver = new DomainIntentResolver();

if (!string.IsNullOrWhiteSpace(casesPath))
{
    var resolvedCasesPath = ResolvePath(root, casesPath);
    var resolvedOutputPath = ResolvePath(root, outputPath ?? Path.Combine("qa", $"user_test_result_{DateTime.Now:yyMMdd_HH_mm}.json"));
    var cases = await LoadCasesAsync(resolvedCasesPath);
    await RunCasesAsync(Path.GetRelativePath(Directory.GetCurrentDirectory(), resolvedCasesPath), cases, resolvedOutputPath, embeddings, store, intentResolver);
    return;
}

if (allActivities)
{
    var resolvedDocsPath = ResolvePath(root, docsPath);
    var resolvedOutputPath = ResolvePath(root, outputPath ?? Path.Combine("qa", $"activity_test_result_{DateTime.Now:yyMMdd_HH_mm}.json"));
    var cases = LoadActivityCases(resolvedDocsPath).ToArray();
    await RunCasesAsync(Path.GetRelativePath(Directory.GetCurrentDirectory(), resolvedDocsPath), cases, resolvedOutputPath, embeddings, store, intentResolver);
    return;
}

var intent = intentResolver.Resolve(question);
var searchQuery = intent.RewrittenQuery ?? question;
var vector = embeddings.Embed(searchQuery);

var results = await store.SearchAsync(
    new SearchRequest(
        searchQuery,
        vector,
        TopK: 8,
        MinScore: 0.0,
        intent.PreferredGroup,
        intent.ActivityNameHint,
        preferredSource,
        question,
        intent.RequiredGroup,
        intent.PreferredSourceHint,
        intent.ActionConceptHints,
        ResolveSourceTypes(intent)),
    CancellationToken.None);

Console.WriteLine($"Question: {question}");
Console.WriteLine($"SearchQuery: {searchQuery}");
Console.WriteLine($"PreferredGroup: {intent.PreferredGroup ?? "-"}");
Console.WriteLine($"RequiredGroup: {intent.RequiredGroup ?? "-"}");
Console.WriteLine($"ActivityHint: {intent.ActivityNameHint ?? "-"}");
Console.WriteLine($"PreferredSourceHint: {intent.PreferredSourceHint ?? "-"}");
Console.WriteLine($"PreferredSource: {preferredSource ?? "-"}");
Console.WriteLine($"QuestionType: {questionType}");
Console.WriteLine();

foreach (var item in results)
{
    Console.WriteLine($"{item.FinalScore:0.0000} | vector={item.VectorScore:0.0000} keyword={item.KeywordScore:0.0000} | {item.Source} | {item.SectionPath}");
    Console.WriteLine(FirstLine(item.Content));
    Console.WriteLine();
}

var orchestrator = new ChatOrchestrator(embeddings, store, new PromptBuilder(), new NoopLlmService(), intentResolver: intentResolver);
var answer = await AskAsync(orchestrator, new UserTestCase("single", "-", question, preferredSource ?? ""), questionType);
Console.WriteLine("Answer:");
Console.WriteLine(answer);

static async Task<UserTestCase[]> LoadCasesAsync(string casesPath)
{
    var json = await File.ReadAllTextAsync(casesPath);
    return JsonSerializer.Deserialize<UserTestCase[]>(json, JsonOptions()) ?? [];
}

static IReadOnlyList<string> ResolveSourceTypes(DomainIntent intent)
{
    return intent.Intent == UserIntent.ProductGuide
        ? ["product_guide", "product_manual"]
        : ["activity_manual", "qa_correction"];
}

static async Task RunCasesAsync(
    string caseFile,
    IReadOnlyList<UserTestCase> cases,
    string outputPath,
    IEmbeddingService embeddings,
    IVectorStore store,
    DomainIntentResolver intentResolver)
{
    var startedAt = DateTimeOffset.Now;
    var results = new List<UserTestCaseResult>();
    var orchestrator = new ChatOrchestrator(embeddings, store, new PromptBuilder(), new NoopLlmService(), intentResolver: intentResolver);

    foreach (var testCase in cases)
    {
        var caseQuestionType = ParseQuestionType(testCase.QuestionType);
        var intent = intentResolver.Resolve(testCase.Question);
        var searchQuery = intent.RewrittenQuery ?? testCase.Question;
        var vector = embeddings.Embed(searchQuery);
        var chunks = await store.SearchAsync(
            new SearchRequest(
                searchQuery,
                vector,
                TopK: 8,
                MinScore: 0.0,
                intent.PreferredGroup,
                intent.ActivityNameHint,
                OriginalQuery: testCase.Question,
                RequiredGroup: intent.RequiredGroup,
                TargetSourceHint: intent.PreferredSourceHint,
                ActionConceptHints: intent.ActionConceptHints,
                SourceTypes: ResolveSourceTypes(intent)),
            CancellationToken.None);

        var ranked = chunks
            .Select((chunk, index) => new RankedSource(
                index + 1,
                chunk.Source,
                chunk.SectionPath,
                chunk.ChunkType,
                Math.Round(chunk.FinalScore, 6),
                Math.Round(chunk.VectorScore, 6),
                Math.Round(chunk.KeywordScore, 6),
                FirstLine(chunk.Content)))
            .ToArray();

        var expectedRank = ranked.FirstOrDefault(r => string.Equals(r.Source, testCase.ExpectedSource, StringComparison.OrdinalIgnoreCase))?.Rank;
        var topSource = ranked.FirstOrDefault()?.Source;
        var answer = await AskAsync(orchestrator, testCase, caseQuestionType);
        var hasSourceExpectation = !string.IsNullOrWhiteSpace(testCase.ExpectedSource);
        var answerContainsExpectedSource = !hasSourceExpectation || answer.Contains(testCase.ExpectedSource, StringComparison.OrdinalIgnoreCase);
        var answerContainsExpectedText = string.IsNullOrWhiteSpace(testCase.ExpectedAnswerContains) ||
                                         answer.Contains(testCase.ExpectedAnswerContains, StringComparison.OrdinalIgnoreCase);
        var expectedSourceFound = !hasSourceExpectation || expectedRank is not null;
        var retrievalTop1Passed = !hasSourceExpectation || expectedRank == 1;
        var passed = answerContainsExpectedSource && answerContainsExpectedText;

        results.Add(new UserTestCaseResult(
            testCase.Id,
            testCase.Domain,
            testCase.Question,
            testCase.ExpectedSource,
            topSource,
            expectedRank,
            passed,
            retrievalTop1Passed,
            expectedSourceFound,
            answerContainsExpectedSource,
            intent.PreferredGroup,
            intent.ActivityNameHint,
            answer,
            ranked));
    }

    var report = new UserTestReport(
        StartedAt: startedAt,
        FinishedAt: DateTimeOffset.Now,
        CaseFile: caseFile,
        Total: results.Count,
        Passed: results.Count(r => r.Passed),
        Failed: results.Count(r => !r.Passed),
        HitWithinTopK: results.Count(r => r.ExpectedSourceFound),
        RetrievalTop1Passed: results.Count(r => r.RetrievalTop1Passed),
        Results: results);

    Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
    await File.WriteAllTextAsync(outputPath, JsonSerializer.Serialize(report, JsonOptions()));

    Console.WriteLine($"CaseFile: {caseFile}");
    Console.WriteLine($"Output: {outputPath}");
    Console.WriteLine($"Passed: {report.Passed}/{report.Total}");
    Console.WriteLine($"RetrievalTop1Passed: {report.RetrievalTop1Passed}/{report.Total}");
    Console.WriteLine($"HitWithinTopK: {report.HitWithinTopK}/{report.Total}");

    foreach (var failed in results.Where(r => !r.Passed))
    {
        var rankText = failed.ExpectedRank is null ? "not found" : failed.ExpectedRank.ToString();
        Console.WriteLine($"FAIL {failed.Id}: expected={failed.ExpectedSource}, top={failed.TopSource ?? "-"}, expectedRank={rankText}");
    }
}

static IEnumerable<UserTestCase> LoadActivityCases(string docsRoot)
{
    foreach (var path in Directory.EnumerateFiles(docsRoot, "*.md", SearchOption.AllDirectories).OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
    {
        var relative = Path.GetRelativePath(docsRoot, path).Replace('\\', '/');
        var domain = relative.Split('/')[0];
        var activity = Path.GetFileNameWithoutExtension(path);
        var text = File.ReadAllText(path);
        var summary = ExtractSection(text, "Summary");
        var question = string.IsNullOrWhiteSpace(summary)
            ? $"{domain} {activity} 액티비티는?"
            : $"{summary.Trim()}";

        yield return new UserTestCase(
            $"activity-{relative.Replace('/', '-').Replace(".md", "", StringComparison.OrdinalIgnoreCase)}",
            domain,
            question,
            relative);
    }
}

static async Task<string> AskAsync(IChatOrchestrator orchestrator, UserTestCase testCase, ChatQuestionType questionType = ChatQuestionType.Auto)
{
    var tokens = new List<string>();
    await foreach (var evt in orchestrator.AskAsync(
        new ChatRequest(testCase.Question, ConversationId: $"smoke-{testCase.Id}", TopK: 8, MinScore: 0.0, QuestionType: questionType),
        CancellationToken.None))
    {
        if (evt.Kind == ChatStreamEventKind.Token)
        {
            tokens.Add(evt.Text);
        }
    }

    return string.Concat(tokens);
}

static IEnumerable<string> GetQuestionArgs(string[] args)
{
    var optionsWithValue = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--cases",
        "--docs",
        "--output",
        "--source",
        "--question-type"
    };

    for (var i = 0; i < args.Length; i++)
    {
        var arg = args[i];
        if (optionsWithValue.Contains(arg))
        {
            i++;
            continue;
        }

        if (arg.StartsWith("--", StringComparison.Ordinal))
        {
            continue;
        }

        yield return arg;
    }
}

static ChatQuestionType ParseQuestionType(string? value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return ChatQuestionType.Auto;
    }

    return Enum.TryParse<ChatQuestionType>(value, ignoreCase: true, out var questionType)
        ? questionType
        : ChatQuestionType.Auto;
}

static string FindRepoRoot()
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

static string FirstLine(string text)
{
    return text.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim() ?? string.Empty;
}

static string ExtractSection(string text, string heading)
{
    var match = Regex.Match(
        text,
        $@"^##\s+{Regex.Escape(heading)}\s*$\r?\n(?<content>.*?)(?=^##\s+|\z)",
        RegexOptions.Multiline | RegexOptions.Singleline);
    return match.Success ? match.Groups["content"].Value.Trim() : string.Empty;
}

static string ResolvePath(string root, string path)
{
    return Path.IsPathRooted(path) ? path : Path.GetFullPath(Path.Combine(root, path));
}

static string? GetArg(string[] args, string name)
{
    for (var i = 0; i < args.Length - 1; i++)
    {
        if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
        {
            return args[i + 1];
        }
    }

    return null;
}

static bool HasArg(string[] args, string name)
{
    return args.Any(arg => string.Equals(arg, name, StringComparison.OrdinalIgnoreCase));
}

static JsonSerializerOptions JsonOptions()
{
    return new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}

/// <summary>
/// Defines one retrieval smoke-test case.
/// </summary>
public sealed record UserTestCase(
    string Id,
    string Domain,
    string Question,
    string ExpectedSource,
    string? QuestionType = null,
    string? ExpectedAnswerContains = null);

/// <summary>
/// Stores aggregate smoke-test results and per-case outcomes.
/// </summary>
public sealed record UserTestReport(
    DateTimeOffset StartedAt,
    DateTimeOffset FinishedAt,
    string CaseFile,
    int Total,
    int Passed,
    int Failed,
    int HitWithinTopK,
    int RetrievalTop1Passed,
    IReadOnlyList<UserTestCaseResult> Results);

/// <summary>
/// Stores the retrieval and answer outcome for one smoke-test case.
/// </summary>
public sealed record UserTestCaseResult(
    string Id,
    string Domain,
    string Question,
    string ExpectedSource,
    string? TopSource,
    int? ExpectedRank,
    bool Passed,
    bool RetrievalTop1Passed,
    bool ExpectedSourceFound,
    bool AnswerContainsExpectedSource,
    string? PreferredGroup,
    string? ActivityNameHint,
    string Answer,
    IReadOnlyList<RankedSource> RankedSources);

/// <summary>
/// Represents one ranked source returned during a smoke test.
/// </summary>
public sealed record RankedSource(
    int Rank,
    string Source,
    string SectionPath,
    string ChunkType,
    double FinalScore,
    double VectorScore,
    double KeywordScore,
    string Preview);

/// <summary>
/// Test-only LLM service that prevents nondeterministic model generation.
/// </summary>
public sealed class NoopLlmService : ILlmService
{
    /// <summary>
    /// Streams a fixed marker response if the orchestrator reaches the LLM fallback path.
    /// </summary>
    public async IAsyncEnumerable<string> StreamAsync(string prompt, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        yield return "LLM fallback was not executed for this deterministic smoke test.";
    }
}
