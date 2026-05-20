using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using BAStudio.Chatbot.Contracts;
using BAStudio.Chatbot.Infra.Configuration;
using BAStudio.Chatbot.Infra.Embedding;
using BAStudio.Chatbot.Infra.Inference;
using BAStudio.Chatbot.Infra.VectorStore;
using BAStudio.Chatbot.Orchestration;
using BAStudio.Chatbot.Policies;
using BAStudio.Chatbot.Prompting;
using BAStudio.Wpf.Services;

namespace BAStudio.Wpf.ViewModels;

public sealed class ChatViewModel : INotifyPropertyChanged
{
    private readonly IChatOrchestrator _orchestrator;
    private readonly ChatbotOptions _options;
    private readonly AnswerCorrectionStore _correctionStore;
    private readonly string _conversationId = Guid.NewGuid().ToString("N");
    private CancellationTokenSource? _cts;
    private string _inputText = "";
    private string _statusText = "준비됨";
    private bool _isBusy;
    private string? _lastQuestion;
    private ChatMessageViewModel? _lastAssistantMessage;
    private string _processLogText = "";

    private ChatViewModel(IChatOrchestrator orchestrator, ChatbotOptions options, AnswerCorrectionStore correctionStore)
    {
        _orchestrator = orchestrator;
        _options = options;
        _correctionStore = correctionStore;
        SendCommand = new RelayCommand(SendAsync, () => CanSend);
        CorrectAnswerCommand = new RelayCommand(CorrectAnswerAsync, () => CanCorrectAnswer);
        CancelCommand = new RelayCommand(Cancel, () => IsBusy);

        Messages.Add(new ChatMessageViewModel(
            "Assistant",
            File.Exists(_options.KbPath)
                ? "BA-Studio 매뉴얼 KB가 준비되었습니다. 액티비티 사용법이나 속성을 질문해보세요."
                : $"KB 파일이 없습니다. 먼저 Tools.KbBuilder로 생성하세요.\n{_options.KbPath}",
            isUser: false));
    }

    public ObservableCollection<ChatMessageViewModel> Messages { get; } = new();
    public ObservableCollection<string> ProcessLogs { get; } = new();

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
            OnPropertyChanged(nameof(CanSend));
            OnPropertyChanged(nameof(CanCorrectAnswer));
            SendCommand.RaiseCanExecuteChanged();
            CorrectAnswerCommand.RaiseCanExecuteChanged();
        }
    }

    public string StatusText
    {
        get => _statusText;
        private set
        {
            if (_statusText == value)
            {
                return;
            }

            _statusText = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (_isBusy == value)
            {
                return;
            }

            _isBusy = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSend));
            OnPropertyChanged(nameof(CanCorrectAnswer));
            SendCommand.RaiseCanExecuteChanged();
            CorrectAnswerCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }
    }

    public bool CanSend => !IsBusy && !string.IsNullOrWhiteSpace(InputText);
    public bool CanCorrectAnswer => !IsBusy && _lastAssistantMessage is not null && !string.IsNullOrWhiteSpace(_lastQuestion);
    public RelayCommand SendCommand { get; }
    public RelayCommand CorrectAnswerCommand { get; }
    public RelayCommand CancelCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public static ChatViewModel Create(ChatbotOptions options, string repoRoot)
    {
        var embeddings = new HashEmbeddingService();
        var store = new SqliteVectorStore(options.KbPath);
        var promptBuilder = new PromptBuilder();
        ILlmService llm = File.Exists(options.ModelPath)
            ? new LlamaSharpPhi4LlmService(options)
            : new GroundedLlmService();
        var orchestrator = new ChatOrchestrator(embeddings, store, promptBuilder, llm, new DomainIntentResolver());
        return new ChatViewModel(orchestrator, options, new AnswerCorrectionStore(repoRoot));
    }

    private async Task SendAsync()
    {
        if (!CanSend)
        {
            return;
        }

        var question = InputText.Trim();
        _lastQuestion = question;
        InputText = "";
        IsBusy = true;
        _cts = new CancellationTokenSource();

        Messages.Add(new ChatMessageViewModel("User", question, isUser: true));
        ProcessLogs.Clear();
        AddProcessLog($"질문 입력: {question}");
        var assistant = new ChatMessageViewModel("Assistant", "", isUser: false);
        _lastAssistantMessage = assistant;
        OnPropertyChanged(nameof(CanCorrectAnswer));
        CorrectAnswerCommand.RaiseCanExecuteChanged();
        Messages.Add(assistant);

        try
        {
            var buffer = new StringBuilder();
            var lastFlush = Environment.TickCount64;
            await foreach (var evt in _orchestrator.AskAsync(new ChatRequest(question, ConversationId: _conversationId, TopK: _options.TopK, MinScore: _options.MinScore), _cts.Token))
            {
                if (evt.Kind == ChatStreamEventKind.Status)
                {
                    StatusText = evt.Text;
                    AddProcessLog(evt.Text);
                    continue;
                }

                if (evt.Kind == ChatStreamEventKind.Token)
                {
                    buffer.Append(evt.Text);
                    var now = Environment.TickCount64;
                    if (now - lastFlush >= 40)
                    {
                        Flush(assistant, buffer);
                        lastFlush = now;
                    }
                }
            }

            Flush(assistant, buffer);
            StatusText = "완료";
            AddProcessLog("완료");
        }
        catch (OperationCanceledException)
        {
            assistant.Text += "\n\n(생성이 취소되었습니다)";
            StatusText = "취소됨";
            AddProcessLog("취소됨");
        }
        catch (Exception ex)
        {
            assistant.Text = $"오류가 발생했습니다.\n{ex.Message}";
            StatusText = "오류";
            AddProcessLog($"오류: {ex.Message}");
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
            IsBusy = false;
        }
    }

    private static void Flush(ChatMessageViewModel target, StringBuilder buffer)
    {
        if (buffer.Length == 0)
        {
            return;
        }

        var text = buffer.ToString();
        buffer.Clear();
        Application.Current.Dispatcher.Invoke(() => target.Text += text);
    }

    private void AddProcessLog(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            ProcessLogs.Add($"[{DateTime.Now:HH:mm:ss}] {text}");
            while (ProcessLogs.Count > 20)
            {
                ProcessLogs.RemoveAt(0);
            }

            ProcessLogText = string.Join(Environment.NewLine, ProcessLogs);
        });
    }

    private void Cancel()
    {
        _cts?.Cancel();
    }

    private async Task CorrectAnswerAsync()
    {
        if (!CanCorrectAnswer || _lastQuestion is null || _lastAssistantMessage is null)
        {
            return;
        }

        var viewModel = new AnswerCorrectionViewModel(_lastQuestion, _lastAssistantMessage.Text);
        var window = new AnswerCorrectionWindow
        {
            Owner = Application.Current.MainWindow,
            DataContext = viewModel
        };

        if (window.ShowDialog() != true)
        {
            return;
        }

        var corrected = viewModel.CorrectedAnswer.Trim();
        var record = new AnswerCorrectionRecord(
            Guid.NewGuid().ToString("N"),
            DateTimeOffset.Now,
            _conversationId,
            _lastQuestion,
            _lastAssistantMessage.Text,
            corrected,
            viewModel.Rating,
            string.IsNullOrWhiteSpace(viewModel.ExpectedSource) ? null : viewModel.ExpectedSource.Trim(),
            string.IsNullOrWhiteSpace(viewModel.Memo) ? null : viewModel.Memo.Trim());

        await _correctionStore.AppendAsync(record);

        if (!string.IsNullOrWhiteSpace(corrected))
        {
            _lastAssistantMessage.Text = corrected;
        }

        StatusText = "답변 평가/수정 저장됨";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
