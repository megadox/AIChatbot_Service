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
    private const string DetailsMarker = "\n<<<DETAILS>>>\n";
    private const string NewSessionTitle = "새 대화";

    private readonly IChatOrchestrator _orchestrator;
    private readonly ChatbotOptions _options;
    private readonly AnswerCorrectionStore _correctionStore;
    private readonly ChatSessionStore _sessionStore;
    private CancellationTokenSource? _cts;
    private ChatSessionViewModel? _selectedSession;
    private string _statusText = "준비됨";
    private bool _isBusy;

    private ChatViewModel(
        IChatOrchestrator orchestrator,
        ChatbotOptions options,
        AnswerCorrectionStore correctionStore,
        ChatSessionStore sessionStore,
        IReadOnlyList<ChatSessionRecord> savedSessions)
    {
        _orchestrator = orchestrator;
        _options = options;
        _correctionStore = correctionStore;
        _sessionStore = sessionStore;

        SendCommand = new RelayCommand(SendAsync, () => CanSend);
        CorrectAnswerCommand = new RelayCommand(CorrectAnswerAsync, () => CanCorrectAnswer);
        CancelCommand = new RelayCommand(Cancel, () => IsBusy);
        NewSessionCommand = new RelayCommand(NewSessionAsync, () => !IsBusy);
        DeleteSessionCommand = new RelayCommand(DeleteSelectedSessionAsync, () => !IsBusy && SelectedSession is not null);

        foreach (var session in savedSessions.Select(ChatSessionViewModel.FromRecord))
        {
            Sessions.Add(session);
        }

        if (Sessions.Count == 0)
        {
            var session = CreateNewSession();
            Sessions.Add(session);
            _ = SaveSessionAsync(session);
        }

        SelectedSession = Sessions.FirstOrDefault();
    }

    public ObservableCollection<ChatSessionViewModel> Sessions { get; } = new();

    public ChatSessionViewModel? SelectedSession
    {
        get => _selectedSession;
        set
        {
            if (ReferenceEquals(_selectedSession, value))
            {
                return;
            }

            if (_selectedSession is not null)
            {
                _selectedSession.PropertyChanged -= SelectedSession_OnPropertyChanged;
            }

            _selectedSession = value;
            if (_selectedSession is not null)
            {
                _selectedSession.PropertyChanged += SelectedSession_OnPropertyChanged;
            }

            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSend));
            OnPropertyChanged(nameof(CanCorrectAnswer));
            RaiseCommandStates();
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
            RaiseCommandStates();
        }
    }

    public bool CanSend => !IsBusy && SelectedSession is not null && !string.IsNullOrWhiteSpace(SelectedSession.InputText);
    public bool CanCorrectAnswer => !IsBusy && GetLastQuestionAndAnswer(SelectedSession) is not null;

    public RelayCommand SendCommand { get; }
    public RelayCommand CorrectAnswerCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand NewSessionCommand { get; }
    public RelayCommand DeleteSessionCommand { get; }

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
        var sessionStore = new ChatSessionStore(repoRoot);
        var savedSessions = sessionStore.LoadAll();
        return new ChatViewModel(orchestrator, options, new AnswerCorrectionStore(repoRoot), sessionStore, savedSessions);
    }

    private async Task NewSessionAsync()
    {
        var session = CreateNewSession();
        Sessions.Insert(0, session);
        SelectedSession = session;
        StatusText = "새 대화";
        await SaveSessionAsync(session);
    }

    private async Task DeleteSelectedSessionAsync()
    {
        var session = SelectedSession;
        if (session is null)
        {
            return;
        }

        var index = Sessions.IndexOf(session);
        Sessions.Remove(session);
        await _sessionStore.DeleteAsync(session.Id);

        if (Sessions.Count == 0)
        {
            var newSession = CreateNewSession();
            Sessions.Add(newSession);
            await SaveSessionAsync(newSession);
        }

        SelectedSession = Sessions[Math.Clamp(index, 0, Sessions.Count - 1)];
        StatusText = "대화 삭제됨";
    }

    private async Task SendAsync()
    {
        var session = SelectedSession;
        if (!CanSend || session is null)
        {
            return;
        }

        var question = session.InputText.Trim();
        var conversationId = session.IsContextRetained ? session.Id : Guid.NewGuid().ToString("N");
        session.InputText = "";
        IsBusy = true;
        _cts = new CancellationTokenSource();

        if (session.Title == NewSessionTitle)
        {
            session.Title = BuildSessionTitle(question);
        }

        session.AddMessage(new ChatMessageViewModel("User", question, isUser: true));
        session.ClearProcessLogs();
        AddProcessLog(session, $"질문 입력: {question}");
        AddProcessLog(session, session.IsContextRetained ? "문맥 유지: 켜짐" : "문맥 유지: 꺼짐");
        var assistant = new ChatMessageViewModel("Assistant", "", isUser: false);
        session.AddMessage(assistant);
        await SaveSessionAsync(session);

        OnPropertyChanged(nameof(CanCorrectAnswer));
        CorrectAnswerCommand.RaiseCanExecuteChanged();

        try
        {
            var buffer = new StringBuilder();
            var lastFlush = Environment.TickCount64;
            await foreach (var evt in _orchestrator.AskAsync(new ChatRequest(question, ConversationId: conversationId, TopK: _options.TopK, MinScore: _options.MinScore), _cts.Token))
            {
                if (evt.Kind == ChatStreamEventKind.Status)
                {
                    StatusText = evt.Text;
                    AddProcessLog(session, evt.Text);
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
            SplitDetails(assistant);
            StatusText = "완료";
            AddProcessLog(session, "완료");
        }
        catch (OperationCanceledException)
        {
            assistant.Text += "\n\n(생성이 취소되었습니다)";
            StatusText = "취소됨";
            AddProcessLog(session, "취소됨");
        }
        catch (Exception ex)
        {
            assistant.Text = $"오류가 발생했습니다.\n{ex.Message}";
            StatusText = "오류";
            AddProcessLog(session, $"오류: {ex.Message}");
        }
        finally
        {
            session.Touch();
            await SaveSessionAsync(session);
            SortSessionsByUpdatedAt(session);
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

    private static void SplitDetails(ChatMessageViewModel target)
    {
        var index = target.Text.IndexOf(DetailsMarker, StringComparison.Ordinal);
        if (index < 0)
        {
            return;
        }

        var main = target.Text[..index].TrimEnd();
        var details = target.Text[(index + DetailsMarker.Length)..].Trim();
        Application.Current.Dispatcher.Invoke(() =>
        {
            target.Text = main;
            target.DetailsText = details;
        });
    }

    private static void AddProcessLog(ChatSessionViewModel session, string text)
    {
        Application.Current.Dispatcher.Invoke(() => session.AddProcessLog(text));
    }

    private void Cancel()
    {
        _cts?.Cancel();
    }

    private async Task CorrectAnswerAsync()
    {
        var session = SelectedSession;
        var last = GetLastQuestionAndAnswer(session);
        if (session is null || last is null)
        {
            return;
        }

        var (lastQuestion, lastAssistantMessage) = last.Value;
        var viewModel = new AnswerCorrectionViewModel(lastQuestion, lastAssistantMessage.Text);
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
            session.Id,
            lastQuestion,
            lastAssistantMessage.Text,
            corrected,
            viewModel.Rating,
            string.IsNullOrWhiteSpace(viewModel.ExpectedSource) ? null : viewModel.ExpectedSource.Trim(),
            string.IsNullOrWhiteSpace(viewModel.Memo) ? null : viewModel.Memo.Trim());

        await _correctionStore.AppendAsync(record);

        if (!string.IsNullOrWhiteSpace(corrected))
        {
            lastAssistantMessage.Text = corrected;
        }

        session.Touch();
        await SaveSessionAsync(session);
        StatusText = "답변 평가/수정 저장됨";
    }

    private ChatSessionViewModel CreateNewSession()
    {
        var now = DateTimeOffset.Now;
        var session = new ChatSessionViewModel(Guid.NewGuid().ToString("N"), NewSessionTitle, now, now);
        session.AddMessage(new ChatMessageViewModel(
            "Assistant",
            File.Exists(_options.KbPath)
                ? "BA-Studio 매뉴얼 KB가 준비되었습니다. 액티비티 사용법이나 속성을 질문해보세요."
                : $"KB 파일이 없습니다. 먼저 Tools.KbBuilder로 생성하세요.\n{_options.KbPath}",
            isUser: false));
        return session;
    }

    private async Task SaveSessionAsync(ChatSessionViewModel session)
    {
        await _sessionStore.SaveAsync(session.ToRecord());
    }

    private void SortSessionsByUpdatedAt(ChatSessionViewModel sessionToKeepSelected)
    {
        var ordered = Sessions.OrderByDescending(s => s.UpdatedAt).ToArray();
        for (var i = 0; i < ordered.Length; i++)
        {
            var currentIndex = Sessions.IndexOf(ordered[i]);
            if (currentIndex >= 0 && currentIndex != i)
            {
                Sessions.Move(currentIndex, i);
            }
        }

        SelectedSession = sessionToKeepSelected;
    }

    private static string BuildSessionTitle(string question)
    {
        var title = question.ReplaceLineEndings(" ").Trim();
        return title.Length <= 28 ? title : title[..28] + "...";
    }

    private static (string Question, ChatMessageViewModel Answer)? GetLastQuestionAndAnswer(ChatSessionViewModel? session)
    {
        if (session is null)
        {
            return null;
        }

        var answerIndex = -1;
        for (var i = session.Messages.Count - 1; i >= 0; i--)
        {
            if (!session.Messages[i].IsUser && !string.IsNullOrWhiteSpace(session.Messages[i].Text))
            {
                answerIndex = i;
                break;
            }
        }

        if (answerIndex < 0)
        {
            return null;
        }

        for (var i = answerIndex - 1; i >= 0; i--)
        {
            if (session.Messages[i].IsUser && !string.IsNullOrWhiteSpace(session.Messages[i].Text))
            {
                return (session.Messages[i].Text, session.Messages[answerIndex]);
            }
        }

        return null;
    }

    private void SelectedSession_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ChatSessionViewModel.InputText))
        {
            OnPropertyChanged(nameof(CanSend));
            SendCommand.RaiseCanExecuteChanged();
        }
        else if (e.PropertyName is nameof(ChatSessionViewModel.IsContextRetained))
        {
            if (sender is ChatSessionViewModel session)
            {
                _ = SaveSessionAsync(session);
            }
        }
    }

    private void RaiseCommandStates()
    {
        SendCommand.RaiseCanExecuteChanged();
        CorrectAnswerCommand.RaiseCanExecuteChanged();
        CancelCommand.RaiseCanExecuteChanged();
        NewSessionCommand.RaiseCanExecuteChanged();
        DeleteSessionCommand.RaiseCanExecuteChanged();
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
