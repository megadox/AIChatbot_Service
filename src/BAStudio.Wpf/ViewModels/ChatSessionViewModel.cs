using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BAStudio.Chatbot.Contracts;
using BAStudio.Wpf.Services;

namespace BAStudio.Wpf.ViewModels;

/// <summary>
/// Represents one chat session tab, including messages, options, and process logs.
/// </summary>
public sealed class ChatSessionViewModel : INotifyPropertyChanged
{
    private string _title;
    private string _inputText = "";
    private string _processLogText = "";
    private bool _isContextRetained = true;
    private bool _isGeneralQuestionEnabled;
    private bool _isWebSearchEnabled;
    private ChatQuestionType _questionType = ChatQuestionType.ActivityTask;

    /// <summary>
    /// Creates a session view model with persisted identity and timestamps.
    /// </summary>
    public ChatSessionViewModel(string id, string title, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Id = id;
        _title = title;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public string Id { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public ObservableCollection<ChatMessageViewModel> Messages { get; } = new();
    public ObservableCollection<string> ProcessLogs { get; } = new();

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
            {
                return;
            }

            _title = value;
            Touch();
            OnPropertyChanged();
        }
    }

    public string InputText
    {
        get => _inputText;
        set
        {
            if (_inputText == value)
            {
                return;
            }

            _inputText = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasInputText));
            OnPropertyChanged(nameof(HasNoInputText));
        }
    }

    public string ProcessLogText
    {
        get => _processLogText;
        private set
        {
            if (_processLogText == value)
            {
                return;
            }

            _processLogText = value;
            OnPropertyChanged();
        }
    }

    public bool IsContextRetained
    {
        get => _isContextRetained;
        set
        {
            if (_isContextRetained == value)
            {
                return;
            }

            _isContextRetained = value;
            Touch();
            OnPropertyChanged();
        }
    }

    public bool IsGeneralQuestionEnabled
    {
        get => _isGeneralQuestionEnabled;
        set
        {
            if (_isGeneralQuestionEnabled == value)
            {
                return;
            }

            _isGeneralQuestionEnabled = value;
            Touch();
            OnPropertyChanged();
        }
    }

    public bool IsWebSearchEnabled
    {
        get => _isWebSearchEnabled;
        set
        {
            if (_isWebSearchEnabled == value)
            {
                return;
            }

            _isWebSearchEnabled = value;
            Touch();
            OnPropertyChanged();
        }
    }

    public ChatQuestionType QuestionType
    {
        get => _questionType;
        set
        {
            if (_questionType == value)
            {
                return;
            }

            _questionType = value;
            if (value == ChatQuestionType.General)
            {
                _isGeneralQuestionEnabled = true;
                OnPropertyChanged(nameof(IsGeneralQuestionEnabled));
            }

            Touch();
            OnPropertyChanged();
        }
    }

    public string DisplayUpdatedAt => UpdatedAt.ToString("MM-dd HH:mm");
    public string MessageCountText => Messages.Count == 0 ? "메시지 없음" : $"메시지 {Messages.Count}개";
    public bool HasInputText => !string.IsNullOrWhiteSpace(InputText);
    public bool HasNoInputText => string.IsNullOrWhiteSpace(InputText);

    /// <summary>
    /// Adds a message to the session and updates its modified timestamp.
    /// </summary>
    public void AddMessage(ChatMessageViewModel message)
    {
        Messages.Add(message);
        Touch();
        OnPropertyChanged(nameof(MessageCountText));
    }

    /// <summary>
    /// Adds one timestamped process-log entry and keeps only recent entries.
    /// </summary>
    public void AddProcessLog(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        ProcessLogs.Add($"[{DateTime.Now:HH:mm:ss}] {text}");
        while (ProcessLogs.Count > 20)
        {
            ProcessLogs.RemoveAt(0);
        }

        ProcessLogText = string.Join(Environment.NewLine, ProcessLogs);
    }

    /// <summary>
    /// Clears the visible process logs for this session.
    /// </summary>
    public void ClearProcessLogs()
    {
        ProcessLogs.Clear();
        ProcessLogText = "";
    }

    /// <summary>
    /// Updates the session modified timestamp and related display bindings.
    /// </summary>
    public void Touch()
    {
        UpdatedAt = DateTimeOffset.Now;
        OnPropertyChanged(nameof(UpdatedAt));
        OnPropertyChanged(nameof(DisplayUpdatedAt));
    }

    /// <summary>
    /// Converts this view model into a persistable session record.
    /// </summary>
    public ChatSessionRecord ToRecord()
    {
        return new ChatSessionRecord(
            Id,
            Title,
            CreatedAt,
            UpdatedAt,
            IsContextRetained,
            Messages.Select(m => new ChatMessageRecord(m.Role, m.Text, m.DetailsText, m.IsUser, m.CreatedAt)).ToArray(),
            IsGeneralQuestionEnabled,
            IsWebSearchEnabled,
            QuestionType.ToString());
    }

    /// <summary>
    /// Rehydrates a session view model from a persisted session record.
    /// </summary>
    public static ChatSessionViewModel FromRecord(ChatSessionRecord record)
    {
        var session = new ChatSessionViewModel(record.Id, record.Title, record.CreatedAt, record.UpdatedAt)
        {
            _isContextRetained = record.IsContextRetained,
            _isGeneralQuestionEnabled = record.IsGeneralQuestionEnabled,
            _isWebSearchEnabled = record.IsWebSearchEnabled,
            _questionType = Enum.TryParse<ChatQuestionType>(record.QuestionType, out var parsed)
                ? parsed
                : ChatQuestionType.ActivityTask
        };

        foreach (var message in record.Messages)
        {
            var viewModel = new ChatMessageViewModel(message.Role, message.Text, message.IsUser, message.CreatedAt)
            {
                DetailsText = message.DetailsText
            };
            session.Messages.Add(viewModel);
        }

        return session;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
