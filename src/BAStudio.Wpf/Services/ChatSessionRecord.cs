namespace BAStudio.Wpf.Services;

/// <summary>
/// Stores lightweight metadata used to list saved chat sessions.
/// </summary>
public sealed record ChatSessionIndexRecord(
    string Id,
    string Title,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

/// <summary>
/// Stores a persisted chat session including messages and per-session options.
/// </summary>
public sealed record ChatSessionRecord(
    string Id,
    string Title,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool IsContextRetained,
    IReadOnlyList<ChatMessageRecord> Messages,
    bool IsGeneralQuestionEnabled = false,
    bool IsWebSearchEnabled = false,
    string QuestionType = "ActivityTask");

/// <summary>
/// Stores one persisted chat message.
/// </summary>
public sealed record ChatMessageRecord(
    string Role,
    string Text,
    string DetailsText,
    bool IsUser,
    DateTimeOffset CreatedAt);
