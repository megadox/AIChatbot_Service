using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Infra.Inference;

/// <summary>
/// Searches the DuckDuckGo instant answer API and normalizes results for the chatbot.
/// </summary>
public sealed class DuckDuckGoWebSearchService : IWebSearchService
{
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(8)
    };

    /// <summary>
    /// Executes a DuckDuckGo search for the supplied query.
    /// </summary>
    public async Task<WebSearchResult> SearchAsync(string query, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"https://api.duckduckgo.com/?q={Uri.EscapeDataString(query)}&format=json&no_html=1&skip_disambig=1";
            var response = await HttpClient.GetFromJsonAsync<DuckDuckGoResponse>(url, cancellationToken);
            if (response is null)
            {
                return new WebSearchResult(query, "", [], IsConnected: true, "웹 검색 결과를 읽을 수 없습니다.");
            }

            var items = (response.RelatedTopics ?? [])
                .SelectMany(FlattenTopics)
                .Where(i => !string.IsNullOrWhiteSpace(i.Text))
                .Take(5)
                .Select(i =>
                {
                    var text = i.Text ?? "";
                    var title = string.IsNullOrWhiteSpace(i.FirstUrl)
                        ? text
                        : text.Split(" - ").FirstOrDefault() ?? text;
                    return new WebSearchItem(title, text, i.FirstUrl ?? "");
                })
                .ToArray();

            var summary = !string.IsNullOrWhiteSpace(response.AbstractText)
                ? response.AbstractText
                : items.FirstOrDefault()?.Snippet ?? "";

            return new WebSearchResult(query, summary, items, IsConnected: true);
        }
        catch (HttpRequestException ex)
        {
            return new WebSearchResult(query, "", [], IsConnected: false, $"웹에 연결할 수 없습니다. {ex.Message}");
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            return new WebSearchResult(query, "", [], IsConnected: false, $"웹 검색 시간이 초과되었습니다. {ex.Message}");
        }
    }

    private static IEnumerable<DuckDuckGoTopic> FlattenTopics(DuckDuckGoTopic topic)
    {
        if (!string.IsNullOrWhiteSpace(topic.Text))
        {
            yield return topic;
        }

        foreach (var child in topic.Topics ?? [])
        {
            foreach (var item in FlattenTopics(child))
            {
                yield return item;
            }
        }
    }

    private sealed record DuckDuckGoResponse(
        [property: JsonPropertyName("AbstractText")] string? AbstractText,
        [property: JsonPropertyName("RelatedTopics")] DuckDuckGoTopic[]? RelatedTopics);

    private sealed record DuckDuckGoTopic(
        [property: JsonPropertyName("Text")] string? Text,
        [property: JsonPropertyName("FirstURL")] string? FirstUrl,
        [property: JsonPropertyName("Topics")] DuckDuckGoTopic[]? Topics);
}
