namespace BAStudio.Chatbot.Contracts;

/// <summary>
/// Represents one user question and runtime options for answer generation.
/// </summary>
public sealed record ChatRequest(
    string Question,
    string? ConversationId = null,
    int TopK = 6,
    double MinScore = 0.0,
    bool AllowGeneralQuestion = false,
    bool AllowWebSearch = false);

/// <summary>
/// Identifies the type of streaming event returned by the chat orchestrator.
/// </summary>
public enum ChatStreamEventKind
{
    Status,
    Token,
    Completed,
    Error
}

/// <summary>
/// Carries one status, answer token, completion, or error event to the UI.
/// </summary>
public sealed record ChatStreamEvent(ChatStreamEventKind Kind, string Text)
{
    /// <summary>
    /// Creates a progress/status event.
    /// </summary>
    public static ChatStreamEvent Status(string text) => new(ChatStreamEventKind.Status, text);

    /// <summary>
    /// Creates an answer text event.
    /// </summary>
    public static ChatStreamEvent Token(string text) => new(ChatStreamEventKind.Token, text);

    /// <summary>
    /// Creates a stream completion event.
    /// </summary>
    public static ChatStreamEvent Completed() => new(ChatStreamEventKind.Completed, string.Empty);

    /// <summary>
    /// Creates an error event.
    /// </summary>
    public static ChatStreamEvent Error(string text) => new(ChatStreamEventKind.Error, text);
}

/// <summary>
/// Represents one retrieved knowledge-base chunk with ranking scores.
/// </summary>
public sealed record RetrievedChunk(
    string Id,
    string Source,
    string GroupName,
    string? ActivityName,
    string Title,
    string SectionPath,
    string ChunkType,
    string Content,
    double VectorScore,
    double KeywordScore,
    double FinalScore);

/// <summary>
/// Describes a vector-store search request and optional domain/source preferences.
/// </summary>
public sealed record SearchRequest(
    string Query,
    float[] QueryEmbedding,
    int TopK,
    double MinScore,
    string? PreferredGroup,
    string? ActivityNameHint,
    string? PreferredSource = null);

/// <summary>
/// Stores the rule-based interpretation of a user's question.
/// </summary>
public sealed record DomainIntent(
    string? PreferredGroup,
    string? ActivityNameHint,
    UserIntent Intent,
    IReadOnlyList<string> Signals);

/// <summary>
/// Contains summarized web search output and connectivity state.
/// </summary>
public sealed record WebSearchResult(
    string Query,
    string Summary,
    IReadOnlyList<WebSearchItem> Items,
    bool IsConnected,
    string? ErrorMessage = null);

/// <summary>
/// Represents one web search result item.
/// </summary>
public sealed record WebSearchItem(
    string Title,
    string Snippet,
    string Url);

/// <summary>
/// Classifies the user's broad question type.
/// </summary>
public enum UserIntent
{
    ActivityLookup,
    ActivityRecommendation,
    Troubleshooting,
    HowTo,
    OutOfScope
}
