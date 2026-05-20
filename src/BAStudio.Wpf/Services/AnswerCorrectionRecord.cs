namespace BAStudio.Wpf.Services;

public sealed record AnswerCorrectionRecord(
    string Id,
    DateTimeOffset CreatedAt,
    string ConversationId,
    string Question,
    string OriginalAnswer,
    string CorrectedAnswer,
    string Rating,
    string? ExpectedSource,
    string? Memo);
