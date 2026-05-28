using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Wpf.Services;

/// <summary>
/// Calls the online chatbot API while preserving the local orchestrator streaming contract.
/// </summary>
public sealed class RemoteChatOrchestratorClient : IChatOrchestrator, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public RemoteChatOrchestratorClient(string apiBaseUrl, string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiBaseUrl))
        {
            throw new ArgumentException("API base URL is required for remote chatbot mode.", nameof(apiBaseUrl));
        }

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/"),
            Timeout = TimeSpan.FromSeconds(120)
        };

        if (!string.IsNullOrWhiteSpace(apiToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
        }

        _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async IAsyncEnumerable<ChatStreamEvent> AskAsync(
        ChatRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/chat", request, _jsonOptions, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            yield return ChatStreamEvent.Error($"온라인 답변 서비스 오류: {(int)response.StatusCode} {response.ReasonPhrase} {error}");
            yield break;
        }

        var result = await response.Content.ReadFromJsonAsync<ChatApiResponse>(_jsonOptions, cancellationToken);
        if (result is null)
        {
            yield return ChatStreamEvent.Error("온라인 답변 서비스 응답을 읽을 수 없습니다.");
            yield break;
        }

        foreach (var evt in result.Events)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return evt;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private sealed record ChatApiResponse(
        string Answer,
        string Details,
        IReadOnlyList<ChatStreamEvent> Events);
}
