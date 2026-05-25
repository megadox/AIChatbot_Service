using System.IO;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using BAStudio.Chatbot.Infra.Configuration;
using BAStudio.Wpf.ViewModels;

namespace BAStudio.Wpf;

/// <summary>
/// Main WPF window that hosts the chatbot UI.
/// </summary>
public partial class MainWindow : Window
{
    private static readonly string RepoRoot = FindRepoRoot();
    private ChatViewModel? _viewModel;
    private ChatSessionViewModel? _observedSession;

    /// <summary>
    /// Initializes the window and creates the main chat view model.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = ChatViewModel.Create(DefaultOptions(), RepoRoot);
        _viewModel.PropertyChanged += ViewModel_OnPropertyChanged;
        DataContext = _viewModel;
        ObserveSelectedSession();
    }

    private void InputBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift && DataContext is ChatViewModel viewModel)
        {
            e.Handled = true;
            if (viewModel.SendCommand.CanExecute(null))
            {
                viewModel.SendCommand.Execute(null);
            }
        }
    }

    private void ChatScrollViewer_OnLoaded(object sender, RoutedEventArgs e)
    {
        ScrollChatToBottom();
    }

    private void ChatScrollViewer_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ScrollChatToBottom();
    }

    private void ViewModel_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ChatViewModel.SelectedSession))
        {
            ObserveSelectedSession();
            ScrollChatToBottom();
        }
    }

    private void ObserveSelectedSession()
    {
        if (_observedSession is not null)
        {
            _observedSession.Messages.CollectionChanged -= Messages_OnCollectionChanged;
        }

        _observedSession = _viewModel?.SelectedSession;
        if (_observedSession is not null)
        {
            _observedSession.Messages.CollectionChanged += Messages_OnCollectionChanged;
        }
    }

    private void Messages_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ScrollChatToBottom();
    }

    private void ScrollChatToBottom()
    {
        if (!IsLoaded)
        {
            return;
        }

        Dispatcher.BeginInvoke(
            () => ChatScrollViewer.ScrollToEnd(),
            DispatcherPriority.ContextIdle);
    }

    private static ChatbotOptions DefaultOptions()
    {
        return new ChatbotOptions
        {
            KbPath = Path.Combine(RepoRoot, "ChatBot", "ba_manual_vector.db"),
            ModelPath = Path.Combine(RepoRoot, "model", "microsoft_Phi-4-mini-instruct-Q4_K_M.gguf")
        };
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "commands.json")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
