using System.Windows;

namespace BAStudio.Wpf;

public partial class AnswerCorrectionWindow : Window
{
    public AnswerCorrectionWindow()
    {
        InitializeComponent();
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.AnswerCorrectionViewModel viewModel &&
            viewModel.Rating != "좋음" &&
            string.Equals(viewModel.CorrectedAnswer.Trim(), viewModel.OriginalAnswer.Trim(), StringComparison.Ordinal))
        {
            MessageBox.Show(
                this,
                "수정 또는 나쁨으로 저장하려면 '수정된 답변' 내용을 실제 정답으로 변경해주세요.",
                "수정된 답변 확인",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        DialogResult = true;
    }
}
