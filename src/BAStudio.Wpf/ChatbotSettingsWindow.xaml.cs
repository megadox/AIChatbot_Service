using System.Windows;
using BAStudio.Wpf.ViewModels;

namespace BAStudio.Wpf;

/// <summary>
/// Dialog window used to edit local or online chatbot settings.
/// </summary>
public partial class ChatbotSettingsWindow : Window
{
    public ChatbotSettingsWindow()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            if (DataContext is ChatbotSettingsViewModel viewModel)
            {
                ApiTokenBox.Password = viewModel.ApiToken;
            }
        };
    }

    private void ApiTokenBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ChatbotSettingsViewModel viewModel)
        {
            viewModel.ApiToken = ApiTokenBox.Password;
        }
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is ChatbotSettingsViewModel viewModel &&
            viewModel.IsRemoteMode &&
            string.IsNullOrWhiteSpace(viewModel.ApiBaseUrl))
        {
            MessageBox.Show(
                this,
                "온라인 모드에서는 API 주소를 입력해야 합니다.",
                "챗봇 설정 확인",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        DialogResult = true;
    }
}
