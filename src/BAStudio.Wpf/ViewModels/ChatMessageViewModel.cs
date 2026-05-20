using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace BAStudio.Wpf.ViewModels;

public sealed class ChatMessageViewModel : INotifyPropertyChanged
{
    private string _text;

    public ChatMessageViewModel(string role, string text, bool isUser)
    {
        Role = role;
        IsUser = isUser;
        _text = text;
        Background = isUser ? new SolidColorBrush(Color.FromRgb(239, 246, 255)) : Brushes.White;
        BorderBrush = isUser ? new SolidColorBrush(Color.FromRgb(191, 219, 254)) : new SolidColorBrush(Color.FromRgb(229, 231, 235));
    }

    public string Role { get; }
    public bool IsUser { get; }

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

    public Brush Background { get; }
    public Brush BorderBrush { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
