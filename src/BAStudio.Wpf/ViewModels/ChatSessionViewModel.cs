using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BAStudio.Wpf.Services;

namespace BAStudio.Wpf.ViewModels;

public sealed class ChatSessionViewModel : INotifyPropertyChanged
{
    private string _title;
    private string _inputText = "";
    private string _processLogText = "";
    private bool _isContextRetained = true;

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

    public string DisplayUpdatedAt => UpdatedAt.ToString("MM-dd HH:mm");

    public void AddMessage(ChatMessageViewModel message)
    {
        Messages.Add(message);
        Touch();
    }

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

    public void ClearProcessLogs()
    {
        ProcessLogs.Clear();
        ProcessLogText = "";
    }

    public void Touch()
    {
        UpdatedAt = DateTimeOffset.Now;
        OnPropertyChanged(nameof(UpdatedAt));
        OnPropertyChanged(nameof(DisplayUpdatedAt));
    }

    public ChatSessionRecord ToRecord()
    {
        return new ChatSessionRecord(
            Id,
            Title,
            CreatedAt,
            UpdatedAt,
            IsContextRetained,
            Messages.Select(m => new ChatMessageRecord(m.Role, m.Text, m.DetailsText, m.IsUser, m.CreatedAt)).ToArray());
    }

    public static ChatSessionViewModel FromRecord(ChatSessionRecord record)
    {
        var session = new ChatSessionViewModel(record.Id, record.Title, record.CreatedAt, record.UpdatedAt)
        {
            _isContextRetained = record.IsContextRetained
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
