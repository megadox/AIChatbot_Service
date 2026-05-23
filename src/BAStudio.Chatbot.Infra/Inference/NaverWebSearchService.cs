using BAStudio.Chatbot.Contracts;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace BAStudio.Chatbot.Infra.Inference;

public sealed class NaverWebSearchService : IWebSearchService
{
    private const string ProxyEndpoint = "http://api.batem.com/naverSearch";

    // 배포 전 회사 서버 프록시 사용을 권장한다. 직접 호출 fallback이 필요하면 아래 값을 설정한다.
    private const string HardcodedClientId = "S0lPWmFTY0ZMZkp5OUw2UzVLRlg=";
    private const string HardcodedClientSecret = "UU1oUldzRk03eA==";

    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(8)
    };

    public async Task<WebSearchResult> SearchAsync(string query, CancellationToken cancellationToken)
    {
        var proxyResult = await TrySearchProxyAsync(query, cancellationToken);
        if (proxyResult is not null)
        {
            return proxyResult;
        }

        return await SearchNaverOpenApiAsync(query, cancellationToken);
    }

    private static async Task<WebSearchResult?> TrySearchProxyAsync(string query, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"{ProxyEndpoint}?query={Uri.EscapeDataString(query)}&display=5";
            var response = await HttpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var searchResponse = await response.Content.ReadFromJsonAsync<NaverSearchResponse>(cancellationToken);
            return searchResponse is null ? null : ToResult(query, searchResponse);
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return null;
        }
    }

    private static async Task<WebSearchResult> SearchNaverOpenApiAsync(string query, CancellationToken cancellationToken)
    {
        var clientId = FirstNonEmpty(dconvert(HardcodedClientId), Environment.GetEnvironmentVariable("NAVER_CLIENT_ID"));
        var clientSecret = FirstNonEmpty(dconvert(HardcodedClientSecret), Environment.GetEnvironmentVariable("NAVER_CLIENT_SECRET"));
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            return new WebSearchResult(
                query,
                "",
                [],
                IsConnected: false,
                "웹 검색 서버에 연결할 수 없고, 네이버 검색 API 키도 설정되어 있지 않습니다.");
        }

        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://openapi.naver.com/v1/search/webkr.json?query={Uri.EscapeDataString(query)}&display=5&start=1&sort=sim");
            request.Headers.Add("X-Naver-Client-Id", clientId);
            request.Headers.Add("X-Naver-Client-Secret", clientSecret);

            var response = await HttpClient.SendAsync(request, cancellationToken);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                return new WebSearchResult(query, "", [], IsConnected: false, "네이버 검색 API 인증에 실패했습니다. Client ID/Secret을 확인해주세요.");
            }

            response.EnsureSuccessStatusCode();
            var searchResponse = await response.Content.ReadFromJsonAsync<NaverSearchResponse>(cancellationToken);
            return searchResponse is null
                ? new WebSearchResult(query, "", [], IsConnected: true, "네이버 검색 결과를 읽을 수 없습니다.")
                : ToResult(query, searchResponse);
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

    private static WebSearchResult ToResult(string query, NaverSearchResponse response)
    {
        var items = (response.Items ?? [])
            .Where(i => !string.IsNullOrWhiteSpace(i.Title) || !string.IsNullOrWhiteSpace(i.Description))
            .Select(i => new WebSearchItem(
                CleanText(i.Title),
                CleanText(i.Description),
                i.Link ?? ""))
            .ToArray();
        var summary = items.FirstOrDefault()?.Snippet ?? "";
        return new WebSearchResult(query, summary, items, IsConnected: true);
    }

    private static string CleanText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        var withoutTags = Regex.Replace(value, "<.*?>", "");
        return WebUtility.HtmlDecode(withoutTags).Trim();
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        return values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
    }

    private static string dconvert(string value)
    {
        byte[] dbt = Convert.FromBase64String(value);
        string d = Encoding.UTF8.GetString(dbt);
        return d;
    }

    private sealed record NaverSearchResponse(
        [property: JsonPropertyName("items")] NaverSearchItem[]? Items);

    private sealed record NaverSearchItem(
        [property: JsonPropertyName("title")] string? Title,
        [property: JsonPropertyName("link")] string? Link,
        [property: JsonPropertyName("description")] string? Description);
}
