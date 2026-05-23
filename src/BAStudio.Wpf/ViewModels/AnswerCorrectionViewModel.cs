using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BAStudio.Wpf.ViewModels;

/// <summary>
/// Holds editable answer feedback fields for the correction dialog.
/// </summary>
public sealed class AnswerCorrectionViewModel : INotifyPropertyChanged
{
    private string _rating = "수정";
    private string _correctedAnswer;
    private string _expectedSource = "";
    private string _memo = "";

    /// <summary>
    /// Creates a correction form initialized with the original question and answer.
    /// </summary>
    public AnswerCorrectionViewModel(string question, string originalAnswer)
    {
        Question = question;
        OriginalAnswer = originalAnswer;
        _correctedAnswer = originalAnswer;
    }

    public string Question { get; }
    public string OriginalAnswer { get; }
    public string[] Ratings { get; } = ["좋음", "수정", "나쁨"];

    public string Rating
    {
        get => _rating;
        set => SetField(ref _rating, value);
    }

    public string CorrectedAnswer
    {
        get => _correctedAnswer;
        set => SetField(ref _correctedAnswer, value);
    }

    public string ExpectedSource
    {
        get => _expectedSource;
        set => SetField(ref _expectedSource, value);
    }

    public string Memo
    {
        get => _memo;
        set => SetField(ref _memo, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField(ref string field, string value, [CallerMemberName] string? propertyName = null)
    {
        if (field == value)
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
