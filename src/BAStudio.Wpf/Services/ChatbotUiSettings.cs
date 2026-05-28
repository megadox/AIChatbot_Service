namespace BAStudio.Wpf.Services;

/// <summary>
/// Stores user-editable chatbot runtime settings for the WPF shell.
/// </summary>
public sealed class ChatbotUiSettings
{
    public string Mode { get; init; } = "Local";
    public string ApiBaseUrl { get; init; } = "";
    public string ApiToken { get; init; } = "";
}
