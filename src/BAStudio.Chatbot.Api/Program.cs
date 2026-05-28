using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Infra.Configuration;
using BAStudio.Chatbot.Infra.Embedding;
using BAStudio.Chatbot.Infra.Inference;
using BAStudio.Chatbot.Infra.VectorStore;
using BAStudio.Chatbot.Orchestration;
using BAStudio.Chatbot.Policies;
using BAStudio.Chatbot.Prompting;

var builder = WebApplication.CreateBuilder(args);
Directory.SetCurrentDirectory(ResolveRuntimeRoot());

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new ChatbotOptions
    {
        KbPath = configuration["Chatbot:KbPath"] ?? "ChatBot/ba_manual_vector.db",
        ModelPath = configuration["Chatbot:ModelPath"] ?? "",
        TopK = int.TryParse(configuration["Chatbot:TopK"], out var topK) ? topK : 6,
        MinScore = double.TryParse(configuration["Chatbot:MinScore"], out var minScore) ? minScore : 0.0
    };
});
builder.Services.AddSingleton<IEmbeddingService, HashEmbeddingService>();
builder.Services.AddSingleton<IVectorStore>(sp =>
{
    var options = sp.GetRequiredService<ChatbotOptions>();
    return new SqliteVectorStore(options.KbPath);
});
builder.Services.AddSingleton<IPromptBuilder, PromptBuilder>();
builder.Services.AddSingleton<ILlmService>(sp =>
{
    var options = sp.GetRequiredService<ChatbotOptions>();
    return File.Exists(options.ModelPath)
        ? new LlamaSharpPhi4LlmService(options)
        : new GroundedLlmService();
});
builder.Services.AddSingleton<IWebSearchService, NaverWebSearchService>();
builder.Services.AddSingleton<DomainIntentResolver>();
builder.Services.AddSingleton<IChatOrchestrator, ChatOrchestrator>();

var app = builder.Build();
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
jsonOptions.Converters.Add(new JsonStringEnumConverter());

app.UseMiddleware<ApiTokenMiddleware>();

app.MapGet("/health", (ChatbotOptions options) => Results.Ok(new
{
    status = "ok",
    version = Environment.GetEnvironmentVariable("CHATBOT_APP_VERSION") ?? ResolveAssemblyVersion(),
    imageTag = Environment.GetEnvironmentVariable("CHATBOT_IMAGE_TAG") ?? "",
    buildDate = Environment.GetEnvironmentVariable("CHATBOT_BUILD_DATE") ?? "",
    gitSha = Environment.GetEnvironmentVariable("CHATBOT_GIT_SHA") ?? "",
    kb = File.Exists(options.KbPath) ? "loaded" : "missing",
    options.KbPath,
    timestamp = DateTimeOffset.UtcNow
}));

app.MapPost("/api/chat", async (
    ChatRequest request,
    IChatOrchestrator orchestrator,
    CancellationToken cancellationToken) =>
{
    var events = new List<ChatStreamEvent>();
    await foreach (var evt in orchestrator.AskAsync(request, cancellationToken))
    {
        events.Add(evt);
    }

    var fullText = string.Concat(events
        .Where(e => e.Kind == ChatStreamEventKind.Token)
        .Select(e => e.Text));
    var (answer, details) = SplitDetails(fullText);

    return Results.Ok(new ChatResponse(answer, details, events));
});

app.MapPost("/api/chat/stream", async (
    HttpContext context,
    ChatRequest request,
    IChatOrchestrator orchestrator,
    CancellationToken cancellationToken) =>
{
    context.Response.Headers.CacheControl = "no-cache";
    context.Response.Headers.Connection = "keep-alive";
    context.Response.ContentType = "text/event-stream; charset=utf-8";

    await foreach (var evt in orchestrator.AskAsync(request, cancellationToken))
    {
        await WriteSseAsync(context.Response, evt, jsonOptions, cancellationToken);
    }
});

app.Run();

static string ResolveRuntimeRoot()
{
    var candidates = new[]
    {
        new DirectoryInfo(Directory.GetCurrentDirectory()),
        new DirectoryInfo(AppContext.BaseDirectory)
    };

    foreach (var candidate in candidates)
    {
        var dir = candidate;
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "commands.json")) ||
                File.Exists(Path.Combine(dir.FullName, "ChatBot", "ba_manual_vector.db")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }
    }

    return Directory.GetCurrentDirectory();
}

static string ResolveAssemblyVersion()
{
    return Assembly.GetExecutingAssembly()
               .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
               ?.InformationalVersion ??
           Assembly.GetExecutingAssembly().GetName().Version?.ToString() ??
           "dev";
}

static (string Answer, string Details) SplitDetails(string text)
{
    const string marker = "\n<<<DETAILS>>>\n";
    var index = text.IndexOf(marker, StringComparison.Ordinal);
    return index < 0
        ? (text.TrimEnd(), "")
        : (text[..index].TrimEnd(), text[(index + marker.Length)..].Trim());
}

static async Task WriteSseAsync(
    HttpResponse response,
    ChatStreamEvent evt,
    JsonSerializerOptions jsonOptions,
    CancellationToken cancellationToken)
{
    var eventName = evt.Kind.ToString().ToLowerInvariant();
    var payload = JsonSerializer.Serialize(evt.Text, jsonOptions);
    await response.WriteAsync($"event: {eventName}\n", cancellationToken);
    await response.WriteAsync($"data: {payload}\n\n", cancellationToken);
    await response.Body.FlushAsync(cancellationToken);
}

internal sealed record ChatResponse(
    string Answer,
    string Details,
    IReadOnlyList<ChatStreamEvent> Events);

internal sealed class ApiTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _tokens;

    public ApiTokenMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        var configured = configuration["Chatbot:ApiTokens"] ??
                         Environment.GetEnvironmentVariable("CHATBOT_API_TOKENS") ??
                         "";
        _tokens = configured
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.Ordinal);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_tokens.Count == 0 || context.Request.Path == "/health")
        {
            await _next(context);
            return;
        }

        var authorization = context.Request.Headers.Authorization.ToString();
        const string bearerPrefix = "Bearer ";
        var token = authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
            ? authorization[bearerPrefix.Length..].Trim()
            : "";

        if (!_tokens.Contains(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
            return;
        }

        await _next(context);
    }
}
