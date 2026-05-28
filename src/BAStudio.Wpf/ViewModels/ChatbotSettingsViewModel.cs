using System.ComponentModel;
using System.Runtime.CompilerServices;
using BAStudio.Wpf.Services;

namespace BAStudio.Wpf.ViewModels;

/// <summary>
/// View model for editing local or online chatbot runtime settings.
/// </summary>
public sealed class ChatbotSettingsViewModel : INotifyPropertyChanged
{
    private string _mode;
    private string _apiBaseUrl;
    private string _apiToken;

    public ChatbotSettingsViewModel(ChatbotUiSettings settings)
    {
        _mode = string.Equals(settings.Mode, "Remote", StringComparison.OrdinalIgnoreCase) ? "Remote" : "Local";
        _apiBaseUrl = settings.ApiBaseUrl;
        _apiToken = settings.ApiToken;
    }

    public IReadOnlyList<ChatbotModeOption> Modes { get; } =
    [
        new("Local", "오프라인"),
        new("Remote", "온라인")
    ];

    public string Mode
    {
        get => _mode;
        set
        {
            if (_mode == value)
            {
                return;
            }

            _mode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsRemoteMode));
        }
    }

    public string ApiBaseUrl
    {
        get => _apiBaseUrl;
        set
        {
            if (_apiBaseUrl == value)
            {
                return;
            }

            _apiBaseUrl = value;
            OnPropertyChanged();
        }
    }

    public string ApiToken
    {
        get => _apiToken;
        set
        {
            if (_apiToken == value)
            {
                return;
            }

            _apiToken = value;
            OnPropertyChanged();
        }
    }

    public bool IsRemoteMode => string.Equals(Mode, "Remote", StringComparison.OrdinalIgnoreCase);

    public ChatbotUiSettings ToSettings()
    {
        return new ChatbotUiSettings
        {
            Mode = IsRemoteMode ? "Remote" : "Local",
            ApiBaseUrl = ApiBaseUrl.Trim(),
            ApiToken = ApiToken.Trim()
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed record ChatbotModeOption(string Value, string Label);
