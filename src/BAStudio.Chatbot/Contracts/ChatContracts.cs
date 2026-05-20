namespace BAStudio.Chatbot.Contracts;

public sealed record ChatRequest(
    string Question,
    string? ConversationId = null,
    int TopK = 6,
    double MinScore = 0.0);

public enum ChatStreamEventKind
{
    Status,
    Token,
    Completed,
    Error
}

public sealed record ChatStreamEvent(ChatStreamEventKind Kind, string Text)
{
    public static ChatStreamEvent Status(string text) => new(ChatStreamEventKind.Status, text);
    public static ChatStreamEvent Token(string text) => new(ChatStreamEventKind.Token, text);
    public static ChatStreamEvent Completed() => new(ChatStreamEventKind.Completed, string.Empty);
    public static ChatStreamEvent Error(string text) => new(ChatStreamEventKind.Error, text);
}

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

public sealed record SearchRequest(
    string Query,
    float[] QueryEmbedding,
    int TopK,
    double MinScore,
    string? PreferredGroup,
    string? ActivityNameHint,
    string? PreferredSource = null);

public sealed record DomainIntent(
    string? PreferredGroup,
    string? ActivityNameHint,
    UserIntent Intent,
    IReadOnlyList<string> Signals);

public enum UserIntent
{
    ActivityLookup,
    ActivityRecommendation,
    Troubleshooting,
    HowTo,
    OutOfScope
}
