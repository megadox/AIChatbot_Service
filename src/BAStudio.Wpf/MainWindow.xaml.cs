using System.IO;
using System.Windows;
using System.Windows.Input;
using BAStudio.Chatbot.Infra.Configuration;
using BAStudio.Wpf.ViewModels;

namespace BAStudio.Wpf;

public partial class MainWindow : Window
{
    private static readonly string RepoRoot = FindRepoRoot();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = ChatViewModel.Create(DefaultOptions(), RepoRoot);
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
