using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace BAStudio.Wpf.ViewModels;

/// <summary>
/// Represents one chat bubble and its visual state in the WPF UI.
/// </summary>
public sealed class ChatMessageViewModel : INotifyPropertyChanged
{
    private string _text;
    private string _detailsText = "";

    /// <summary>
    /// Creates a chat message with the current timestamp.
    /// </summary>
    public ChatMessageViewModel(string role, string text, bool isUser)
        : this(role, text, isUser, DateTimeOffset.Now)
    {
    }

    /// <summary>
    /// Creates a chat message with an explicit timestamp.
    /// </summary>
    public ChatMessageViewModel(string role, string text, bool isUser, DateTimeOffset createdAt)
    {
        Role = role;
        IsUser = isUser;
        CreatedAt = createdAt;
        _text = text;
        Background = isUser ? new SolidColorBrush(Color.FromRgb(239, 246, 255)) : Brushes.White;
        BorderBrush = isUser ? new SolidColorBrush(Color.FromRgb(191, 219, 254)) : new SolidColorBrush(Color.FromRgb(229, 231, 235));
        CopyCommand = new RelayCommand(CopyText, () => HasText);
    }

    public string Role { get; }
    public bool IsUser { get; }
    public DateTimeOffset CreatedAt { get; }
    public string DisplayRole => IsUser ? "나" : "BA Chatbot";
    public string DisplayCreatedAt => CreatedAt.ToString("HH:mm");
    public HorizontalAlignment BubbleAlignment => IsUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    public double BubbleMaxWidth => IsUser ? 560 : 680;

    public string Text
    {
        get => _text;
        set
        {
            if (_text == value)
            {
                return;
            }

            _text = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasText));
            CopyCommand.RaiseCanExecuteChanged();
        }
    }

    public string DetailsText
    {
        get => _detailsText;
        set
        {
            if (_detailsText == value)
            {
                return;
            }

            _detailsText = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasDetails));
        }
    }

    public bool HasDetails => !string.IsNullOrWhiteSpace(DetailsText);
    public bool HasText => !string.IsNullOrWhiteSpace(Text);
    public string CopyToolTip => IsUser ? "질문 복사" : "답변 복사";

    public Brush Background { get; }
    public Brush BorderBrush { get; }
    public RelayCommand CopyCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void CopyText()
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {
            Clipboard.SetText(Text);
        }
    }
}
