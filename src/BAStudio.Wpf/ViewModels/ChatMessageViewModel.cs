using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace BAStudio.Wpf.ViewModels;

public sealed class ChatMessageViewModel : INotifyPropertyChanged
{
    private string _text;
    private string _detailsText = "";

    public ChatMessageViewModel(string role, string text, bool isUser)
        : this(role, text, isUser, DateTimeOffset.Now)
    {
    }

    public ChatMessageViewModel(string role, string text, bool isUser, DateTimeOffset createdAt)
    {
        Role = role;
        IsUser = isUser;
        CreatedAt = createdAt;
        _text = text;
        Background = isUser ? new SolidColorBrush(Color.FromRgb(239, 246, 255)) : Brushes.White;
        BorderBrush = isUser ? new SolidColorBrush(Color.FromRgb(191, 219, 254)) : new SolidColorBrush(Color.FromRgb(229, 231, 235));
    }

    public string Role { get; }
    public bool IsUser { get; }
    public DateTimeOffset CreatedAt { get; }

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

    public Brush Background { get; }
    public Brush BorderBrush { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
