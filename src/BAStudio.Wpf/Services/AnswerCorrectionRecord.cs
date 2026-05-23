namespace BAStudio.Wpf.Services;

/// <summary>
/// Stores one user evaluation or correction for a chatbot answer.
/// </summary>
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
