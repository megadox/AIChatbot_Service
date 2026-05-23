namespace BAStudio.Wpf.Services;

public sealed record ChatSessionIndexRecord(
    string Id,
    string Title,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record ChatSessionRecord(
    string Id,
    string Title,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool IsContextRetained,
    IReadOnlyList<ChatMessageRecord> Messages,
    bool IsGeneralQuestionEnabled = false,
    bool IsWebSearchEnabled = false);

public sealed record ChatMessageRecord(
    string Role,
    string Text,
    string DetailsText,
    bool IsUser,
    DateTimeOffset CreatedAt);
