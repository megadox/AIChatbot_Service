using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;
using Microsoft.Data.Sqlite;

namespace BAStudio.Chatbot.Infra.VectorStore;

public sealed class SqliteVectorStore : IVectorStore
{
    private readonly string _dbPath;

    public SqliteVectorStore(string dbPath)
    {
        _dbPath = dbPath;
    }

    public async Task<IReadOnlyList<RetrievedChunk>> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        if (!File.Exists(_dbPath))
        {
            return Array.Empty<RetrievedChunk>();
        }

        var tokens = Tokenize(request.Query)
            .SelectMany(ExpandSearchToken)
            .Where(t => !IsSearchStopToken(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var candidates = new List<Candidate>();

        await using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadOnly");
        await connection.OpenAsync(cancellationToken);
        var aliasMatches = await FindAliasMatchesAsync(connection, request.Query, cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT c.id, c.source, c.group_name, c.activity_name, c.title, c.section_path, c.chunk_type,
                   c.content, e.vector_blob
            FROM kb_chunks c
            JOIN kb_embeddings e ON e.chunk_id = c.id
            """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var content = reader.GetString(7);
            var source = reader.GetString(1);
            var group = reader.GetString(2);
            var activity = reader.IsDBNull(3) ? null : reader.GetString(3);
            var title = reader.GetString(4);
            var section = reader.GetString(5);
            var chunkType = reader.GetString(6);

            var keyword = ScoreKeyword(tokens, source, group, activity, title, section, content);
            var vector = ReadVector((byte[])reader["vector_blob"]);
            var vectorScore = Dot(request.QueryEmbedding, vector);
            var domainBoost = request.PreferredGroup is not null && string.Equals(group, request.PreferredGroup, StringComparison.OrdinalIgnoreCase) ? 0.35 : 0;
            var activityBoost = request.ActivityNameHint is not null && string.Equals(activity, request.ActivityNameHint, StringComparison.OrdinalIgnoreCase) ? 0.08 : 0;
            var sourceBoost = request.PreferredSource is not null && string.Equals(source, request.PreferredSource, StringComparison.OrdinalIgnoreCase) ? 0.55 : 0;
            var exactSummaryBoost = IsExactSummaryMatch(request.Query, chunkType, content) ? 0.45 : 0;
            var activityNameBoost = !string.IsNullOrWhiteSpace(activity) && ContainsActivityName(request.Query, activity) ? 0.18 : 0;
            var aliasBoost = aliasMatches.TryGetValue(source, out var aliasScore)
                ? 0.45 + aliasScore * 1.1
                : 0;
            var summaryBoost = string.Equals(chunkType, "summary", StringComparison.OrdinalIgnoreCase) ? 0.12 : 0;
            var actionBoost = ScoreActionConcept(request.Query, source, activity, title, content);
            var final = vectorScore * 0.55 + keyword * 0.25 + domainBoost + activityBoost + sourceBoost + exactSummaryBoost + activityNameBoost + aliasBoost + summaryBoost + actionBoost;

            candidates.Add(new Candidate(
                new RetrievedChunk(
                    reader.GetString(0),
                    source,
                    group,
                    activity,
                    title,
                    section,
                    chunkType,
                    content,
                    vectorScore,
                    keyword,
                    final),
                vector));
        }

        return SelectMmr(candidates.Where(c => c.Chunk.FinalScore >= request.MinScore), request.TopK)
            .Select(c => c.Chunk)
            .ToArray();
    }

    private static IEnumerable<Candidate> SelectMmr(IEnumerable<Candidate> candidates, int topK)
    {
        var pool = candidates
            .OrderByDescending(c => c.Chunk.FinalScore)
            .ThenByDescending(c => c.Chunk.ChunkType == "summary" ? 1 : 0)
            .ThenBy(c => c.Chunk.Source, StringComparer.OrdinalIgnoreCase)
            .Take(80)
            .ToList();
        var selected = new List<Candidate>();

        while (selected.Count < topK && pool.Count > 0)
        {
            Candidate best;
            if (selected.Count == 0)
            {
                best = pool[0];
            }
            else
            {
                best = pool
                    .OrderByDescending(c =>
                    {
                        var duplicatePenalty = selected.Max(s => Dot(c.Vector, s.Vector));
                        return 0.7 * c.Chunk.FinalScore - 0.3 * duplicatePenalty;
                    })
                    .ThenByDescending(c => c.Chunk.ChunkType == "summary" ? 1 : 0)
                    .ThenBy(c => c.Chunk.Source, StringComparer.OrdinalIgnoreCase)
                    .First();
            }

            selected.Add(best);
            pool.Remove(best);
        }

        return selected;
    }

    private static async Task<Dictionary<string, double>> FindAliasMatchesAsync(SqliteConnection connection, string query, CancellationToken cancellationToken)
    {
        var sources = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        if (!await TableExistsAsync(connection, "activity_aliases", cancellationToken))
        {
            return sources;
        }

        var normalizedQuery = NormalizeForMatching(query);
        var compactQuery = NormalizeForCompactMatch(query);
        var queryTokens = TokenizeForMatching(query).SelectMany(ExpandToken).ToHashSet(StringComparer.OrdinalIgnoreCase);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT source, alias, normalized_alias
            FROM activity_aliases
            """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var source = reader.GetString(0);
            var alias = reader.GetString(1);
            var normalizedAlias = NormalizeForMatching(reader.GetString(2));
            var compactAlias = NormalizeForCompactMatch(alias);
            var score = ScoreAliasMatch(normalizedQuery, compactQuery, queryTokens, normalizedAlias, compactAlias, alias);
            if (score > 0)
            {
                sources[source] = Math.Max(sources.GetValueOrDefault(source), score);
            }
        }

        return sources;
    }

    private static double ScoreAliasMatch(
        string normalizedQuery,
        string compactQuery,
        HashSet<string> queryTokens,
        string normalizedAlias,
        string compactAlias,
        string rawAlias)
    {
        if (string.IsNullOrWhiteSpace(normalizedAlias) || string.IsNullOrWhiteSpace(compactAlias))
        {
            return 0;
        }

        if (string.Equals(normalizedQuery, normalizedAlias, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(compactQuery, compactAlias, StringComparison.OrdinalIgnoreCase))
        {
            return 1.0;
        }

        if (ContainsPhrase(normalizedQuery, normalizedAlias))
        {
            var specificity = Math.Min(1.0, (double)normalizedAlias.Length / Math.Max(normalizedQuery.Length, 1));
            return 0.65 + specificity * 0.35;
        }

        if (compactQuery.Contains(compactAlias, StringComparison.OrdinalIgnoreCase))
        {
            var specificity = Math.Min(1.0, (double)compactAlias.Length / Math.Max(compactQuery.Length, 1));
            return 0.7 + specificity * 0.18;
        }

        var aliasTokens = TokenizeForMatching(rawAlias)
            .Select(NormalizeTokenForMatching)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (aliasTokens.Length == 0 || queryTokens.Count == 0)
        {
            return 0;
        }

        var hits = aliasTokens.Count(queryTokens.Contains);
        var coverage = (double)hits / aliasTokens.Length;
        if (aliasTokens.Length < 3 || coverage < 0.75)
        {
            return 0;
        }

        var tokenSpecificity = Math.Min(0.2, aliasTokens.Length * 0.03);
        return coverage * (0.55 + tokenSpecificity);
    }

    private static bool ContainsPhrase(string haystack, string needle)
    {
        return Regex.IsMatch(
            haystack,
            $@"(^|\s){Regex.Escape(needle)}($|\s)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private static async Task<bool> TableExistsAsync(SqliteConnection connection, string tableName, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT 1
            FROM sqlite_master
            WHERE type = 'table' AND name = $name
            LIMIT 1
            """;
        command.Parameters.AddWithValue("$name", tableName);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is not null;
    }

    private static double ScoreKeyword(string[] tokens, params string?[] fields)
    {
        if (tokens.Length == 0)
        {
            return 0;
        }

        var fieldTokens = fields
            .Where(f => !string.IsNullOrWhiteSpace(f))
            .SelectMany(f => Tokenize(f!))
            .SelectMany(ExpandSearchToken)
            .Where(t => !IsSearchStopToken(t))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var hits = tokens.Count(fieldTokens.Contains);
        return Math.Min(1.0, (double)hits / tokens.Length);
    }

    private static IEnumerable<string> Tokenize(string text)
    {
        foreach (Match match in Regex.Matches(text.ToLowerInvariant(), @"[A-Za-z0-9_]+|[가-힣]+"))
        {
            yield return match.Value;
        }
    }

    private static IEnumerable<string> ExpandSearchToken(string token)
    {
        yield return token;

        var normalized = NormalizeTokenForMatching(token);
        if (!string.Equals(token, normalized, StringComparison.OrdinalIgnoreCase))
        {
            yield return normalized;
        }

        foreach (var synonym in GetSearchSynonyms(normalized))
        {
            yield return synonym;
        }
    }

    private static IEnumerable<string> GetSearchSynonyms(string token)
    {
        return token switch
        {
            "자바스크립트" or "js" or "javascript" => ["javascript", "script", "자바스크립트"],
            "스크립트" or "script" => ["script", "javascript", "스크립트"],
            "어플리케이션" or "애플리케이션" or "프로그램" or "application" or "app" => ["애플리케이션", "어플리케이션", "프로그램", "application", "app"],
            "최소화" or "minimize" => ["최소화", "minimize"],
            "최대화" or "maximize" => ["최대화", "maximize"],
            "삭제" or "delete" or "remove" or "clear" => ["삭제", "delete", "remove", "clear"],
            "동시" or "병렬" or "스레드" or "멀티스레드" or "thread" or "multithread" or "parallel" => ["동시", "병렬", "스레드", "멀티스레드", "thread", "multithread", "parallel"],
            _ => []
        };
    }

    private static bool IsSearchStopToken(string token)
    {
        return token is "하는" or "한다" or "할" or "하기" or "액티비티" or "사용" or "방법";
    }

    private static bool IsExactSummaryMatch(string query, string chunkType, string content)
    {
        return string.Equals(chunkType, "summary", StringComparison.OrdinalIgnoreCase) &&
               string.Equals(NormalizeForExactMatch(query), NormalizeForExactMatch(content), StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsActivityName(string query, string activity)
    {
        var normalizedQuery = NormalizeForExactMatch(query);
        var normalizedActivity = NormalizeForExactMatch(activity);
        return normalizedActivity.Length > 0 && normalizedQuery.Contains(normalizedActivity, StringComparison.OrdinalIgnoreCase);
    }

    private static double ScoreActionConcept(string query, params string?[] fields)
    {
        var queryTokens = Tokenize(query)
            .SelectMany(ExpandSearchToken)
            .Where(t => !IsSearchStopToken(t))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (queryTokens.Count == 0)
        {
            return 0;
        }

        var fieldTokens = fields
            .Where(f => !string.IsNullOrWhiteSpace(f))
            .SelectMany(f => Tokenize(f!))
            .SelectMany(ExpandSearchToken)
            .Where(t => !IsSearchStopToken(t))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var concept in ActionConcepts)
        {
            if (concept.Terms.Any(queryTokens.Contains) && concept.Terms.Any(fieldTokens.Contains))
            {
                return 0.42;
            }
        }

        return 0;
    }

    private static readonly ActionConcept[] ActionConcepts =
    [
        new(["최소화", "minimize"]),
        new(["최대화", "maximize"]),
        new(["표시", "show"]),
        new(["숨김", "숨기", "hide"]),
        new(["실행", "execute", "run"]),
        new(["삭제", "delete", "remove", "clear"]),
        new(["동시", "병렬", "스레드", "멀티스레드", "thread", "multithread", "parallel"]),
        new(["클릭", "click"]),
        new(["더블클릭", "doubleclick"])
    ];

    private static string NormalizeForExactMatch(string value)
    {
        return Regex.Replace(value.Trim().ToLowerInvariant(), @"\s+", " ");
    }

    private static string NormalizeForMatching(string value)
    {
        var normalized = Regex.Replace(value.Trim().ToLowerInvariant(), @"[^\p{L}\p{N}_]+", " ");
        return Regex.Replace(normalized, @"\s+", " ").Trim();
    }

    private static string NormalizeForCompactMatch(string value)
    {
        return Regex.Replace(value.Trim().ToLowerInvariant(), @"[^\p{L}\p{N}_]+", "");
    }

    private static IEnumerable<string> TokenizeForMatching(string text)
    {
        foreach (Match match in Regex.Matches(text.ToLowerInvariant(), @"[A-Za-z0-9_]+|[가-힣]+"))
        {
            yield return match.Value;
        }
    }

    private static IEnumerable<string> ExpandToken(string token)
    {
        yield return token;

        var normalized = NormalizeTokenForMatching(token);
        if (!string.Equals(token, normalized, StringComparison.OrdinalIgnoreCase))
        {
            yield return normalized;
        }
    }

    private static string NormalizeTokenForMatching(string token)
    {
        if (!Regex.IsMatch(token, @"^[가-힣]+$") || token.Length <= 2)
        {
            return token;
        }

        var endings = new[] { "시켜주는", "시키는", "하려고", "되는", "하는", "한다", "하기", "하려", "된", "할" };
        foreach (var ending in endings)
        {
            if (token.EndsWith(ending, StringComparison.Ordinal) && token.Length > ending.Length + 1)
            {
                return token[..^ending.Length];
            }
        }

        var particles = new[] { "으로부터", "로부터", "에서는", "에서", "에게", "으로", "로", "의", "을", "를", "은", "는", "이", "가", "에" };
        foreach (var particle in particles)
        {
            if (token.EndsWith(particle, StringComparison.Ordinal) && token.Length > particle.Length + 1)
            {
                return token[..^particle.Length];
            }
        }

        return token;
    }

    private static double Dot(IReadOnlyList<float> a, IReadOnlyList<float> b)
    {
        var len = Math.Min(a.Count, b.Count);
        double sum = 0;
        for (var i = 0; i < len; i++)
        {
            sum += a[i] * b[i];
        }

        return sum;
    }

    public static byte[] ToBlob(float[] vector)
    {
        var bytes = new byte[vector.Length * sizeof(float)];
        Buffer.BlockCopy(vector, 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static float[] ReadVector(byte[] blob)
    {
        var vector = new float[blob.Length / sizeof(float)];
        Buffer.BlockCopy(blob, 0, vector, 0, blob.Length);
        return vector;
    }

    private sealed record Candidate(RetrievedChunk Chunk, float[] Vector);

    private sealed record ActionConcept(string[] Terms);
}
