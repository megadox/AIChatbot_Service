using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Infra.Embedding;
using BAStudio.Chatbot.Infra.VectorStore;
using BAStudio.Chatbot.Policies;
using Microsoft.Data.Sqlite;

var root = FindRepoRoot();
var docsDir = GetArg(args, "--docs") ?? Path.Combine(root, "docs", "generated", "commands");
var outPath = GetArg(args, "--out") ?? Path.Combine(root, "ChatBot", "ba_manual_vector.db");
Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);

if (File.Exists(outPath))
{
    File.Delete(outPath);
}

var embedding = new HashEmbeddingService();
var chunks = Directory.EnumerateFiles(docsDir, "*.md", SearchOption.AllDirectories)
    .SelectMany(path => ParseMarkdown(path, docsDir))
    .Concat(ParseCorrections(Path.Combine(root, "qa", "answer_corrections.jsonl")))
    .ToArray();
var aliases = LoadAliases(Path.Combine(root, "qa", "activity_aliases.json")).ToArray();

await using var connection = new SqliteConnection($"Data Source={outPath}");
await connection.OpenAsync();
await CreateSchemaAsync(connection);

await using var transaction = await connection.BeginTransactionAsync();
foreach (var chunk in chunks)
{
    var vector = embedding.Embed($"{chunk.Title}\n{chunk.Source}\n{chunk.SectionPath}\n{chunk.Content}");
    await InsertChunkAsync(connection, chunk, SqliteVectorStore.ToBlob(vector));
}

foreach (var alias in aliases)
{
    await InsertAliasAsync(connection, alias);
}

await transaction.CommitAsync();
Console.WriteLine($"KB build completed: {outPath}");
Console.WriteLine($"Chunks: {chunks.Length}");
Console.WriteLine($"Aliases: {aliases.Length}");
Console.WriteLine($"Embedding dimension: {embedding.Dimension}");

static string? GetArg(string[] args, string name)
{
    for (var i = 0; i < args.Length - 1; i++)
    {
        if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
        {
            return args[i + 1];
        }
    }

    return null;
}

static string FindRepoRoot()
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

static IEnumerable<KbChunk> ParseMarkdown(string path, string docsRoot)
{
    var relative = Path.GetRelativePath(docsRoot, path).Replace('\\', '/');
    var group = relative.Split('/')[0];
    var activity = Path.GetFileNameWithoutExtension(path);
    var text = File.ReadAllText(path, Encoding.UTF8);
    var title = Regex.Match(text, @"^#\s+(.+)$", RegexOptions.Multiline).Groups[1].Value.Trim();
    if (string.IsNullOrWhiteSpace(title))
    {
        title = $"Activity: {activity}";
    }

    var summary = ExtractSection(text, "Summary");
    if (!string.IsNullOrWhiteSpace(summary))
    {
        yield return Create(relative, group, activity, title, "Summary", "summary", summary);
    }

    var metadata = ExtractSection(text, "Metadata");
    if (!string.IsNullOrWhiteSpace(metadata))
    {
        yield return Create(relative, group, activity, title, "Metadata", "metadata", metadata);
    }

    var properties = ExtractSection(text, "Properties");
    if (!string.IsNullOrWhiteSpace(properties))
    {
        yield return Create(relative, group, activity, title, "Properties", "properties", properties);
    }

    var notes = ExtractPropertyNotes(text);
    foreach (var (name, content) in notes)
    {
        yield return Create(relative, group, activity, title, $"Property Notes/{name}", "property_note", content);
    }
}

static IEnumerable<KbChunk> ParseCorrections(string path)
{
    if (!File.Exists(path))
    {
        yield break;
    }

    var index = 0;
    var intentResolver = new DomainIntentResolver();
    foreach (var line in File.ReadLines(path, Encoding.UTF8))
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            continue;
        }

        AnswerCorrectionRecord? correction;
        try
        {
            correction = JsonSerializer.Deserialize<AnswerCorrectionRecord>(line);
        }
        catch (JsonException)
        {
            continue;
        }

        if (correction is null || string.IsNullOrWhiteSpace(correction.CorrectedAnswer))
        {
            continue;
        }

        index++;
        var expectedSource = NormalizeHint(correction.ExpectedSource);
        var source = LooksLikeSourcePath(expectedSource)
            ? expectedSource
            : $"qa/answer_corrections/{correction.Id ?? index.ToString()}";
        var intent = intentResolver.Resolve($"{correction.Question} {expectedSource}");
        var group = source.Contains('/')
            ? source.Split('/')[0]
            : intent.PreferredGroup ?? "QA";
        if (string.Equals(group, "qa", StringComparison.OrdinalIgnoreCase))
        {
            group = intent.PreferredGroup ?? "QA";
        }

        var activity = LooksLikeSourcePath(source)
            ? Path.GetFileNameWithoutExtension(source)
            : intent.ActivityNameHint ?? "AnswerCorrection";
        var hasChangedAnswer = !string.Equals(correction.CorrectedAnswer.Trim(), correction.OriginalAnswer.Trim(), StringComparison.Ordinal);
        var content = $"""
        Question:
        {correction.Question}

        Corrected Answer:
        {correction.CorrectedAnswer}

        Expected Source or Search Hint:
        {expectedSource}

        Has Changed Answer:
        {hasChangedAnswer}

        Memo:
        {correction.Memo}
        """;

        yield return Create(
            source,
            group,
            string.IsNullOrWhiteSpace(activity) ? "AnswerCorrection" : activity,
            BuildCorrectionTitle(correction.Question, expectedSource),
            $"Answer Corrections/{correction.Id ?? index.ToString()}",
            "answer_correction",
            content);
    }
}

static IEnumerable<ActivityAlias> LoadAliases(string path)
{
    if (!File.Exists(path))
    {
        yield break;
    }

    ActivityAliasRecord[]? records;
    try
    {
        records = JsonSerializer.Deserialize<ActivityAliasRecord[]>(File.ReadAllText(path, Encoding.UTF8), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
    catch (JsonException)
    {
        yield break;
    }

    foreach (var record in records ?? [])
    {
        if (string.IsNullOrWhiteSpace(record.Source) || record.Aliases is null)
        {
            continue;
        }

        foreach (var alias in record.Aliases.Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            yield return new ActivityAlias(record.Source.Trim().Replace('\\', '/'), alias);
        }
    }
}


static string NormalizeHint(string? value)
{
    return string.IsNullOrWhiteSpace(value)
        ? "qa/answer_corrections.jsonl"
        : value.Trim().Replace('\\', '/');
}

static bool LooksLikeSourcePath(string value)
{
    return Regex.IsMatch(value, @"^[A-Za-z0-9_가-힣-]+/[A-Za-z0-9_가-힣./-]+\.md$", RegexOptions.IgnoreCase);
}

static string BuildCorrectionTitle(string question, string expectedSource)
{
    return string.IsNullOrWhiteSpace(expectedSource) || expectedSource == "qa/answer_corrections.jsonl"
        ? $"Answer Correction: {question}"
        : $"Answer Correction: {question} ({expectedSource})";
}

static KbChunk Create(string source, string group, string activity, string title, string section, string type, string content)
{
    var idSeed = $"{source}|{section}|{content}";
    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(idSeed))).ToLowerInvariant();
    return new KbChunk(
        hash[..32],
        source,
        group,
        activity,
        title,
        section,
        type,
        Normalize(content),
        hash);
}

static string ExtractSection(string text, string heading)
{
    var match = Regex.Match(
        text,
        $@"^##\s+{Regex.Escape(heading)}\s*$\r?\n(?<content>.*?)(?=^##\s+|\z)",
        RegexOptions.Multiline | RegexOptions.Singleline);
    return match.Success ? match.Groups["content"].Value.Trim() : string.Empty;
}

static IEnumerable<(string Name, string Content)> ExtractPropertyNotes(string text)
{
    var section = ExtractSection(text, "Property Notes");
    if (string.IsNullOrWhiteSpace(section))
    {
        yield break;
    }

    var matches = Regex.Matches(
        section,
        @"^###\s+`?(?<name>[^`\r\n]+)`?\s*$\r?\n(?<content>.*?)(?=^###\s+|\z)",
        RegexOptions.Multiline | RegexOptions.Singleline);

    foreach (Match match in matches)
    {
        var name = match.Groups["name"].Value.Trim();
        var content = match.Groups["content"].Value.Trim();
        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(content))
        {
            yield return (name, $"### {name}\n{content}");
        }
    }
}

static string Normalize(string content)
{
    return content.Replace("<br/>", "\n", StringComparison.OrdinalIgnoreCase).Trim();
}

static async Task CreateSchemaAsync(SqliteConnection connection)
{
    var command = connection.CreateCommand();
    command.CommandText = """
        CREATE TABLE kb_chunks (
          id TEXT PRIMARY KEY,
          source TEXT NOT NULL,
          group_name TEXT NOT NULL,
          activity_name TEXT NULL,
          title TEXT NOT NULL,
          section_path TEXT NOT NULL,
          chunk_type TEXT NOT NULL,
          content TEXT NOT NULL,
          content_hash TEXT NOT NULL,
          kb_version TEXT NOT NULL,
          embedding_model_id TEXT NOT NULL,
          updated_at_utc TEXT NOT NULL
        );

        CREATE TABLE kb_embeddings (
          chunk_id TEXT PRIMARY KEY,
          dim INTEGER NOT NULL,
          vector_blob BLOB NOT NULL,
          FOREIGN KEY(chunk_id) REFERENCES kb_chunks(id) ON DELETE CASCADE
        );

        CREATE TABLE kb_meta (
          key TEXT PRIMARY KEY,
          value TEXT NOT NULL
        );

        CREATE TABLE activity_aliases (
          source TEXT NOT NULL,
          alias TEXT NOT NULL,
          normalized_alias TEXT NOT NULL,
          PRIMARY KEY(source, normalized_alias)
        );

        CREATE INDEX idx_kb_chunks_source ON kb_chunks(source);
        CREATE INDEX idx_kb_chunks_group ON kb_chunks(group_name);
        CREATE INDEX idx_kb_chunks_activity ON kb_chunks(activity_name);
        CREATE INDEX idx_activity_aliases_normalized ON activity_aliases(normalized_alias);

        INSERT INTO kb_meta(key, value) VALUES
          ('schema_version', '1'),
          ('kb_version', strftime('%Y%m%d%H%M%S', 'now')),
          ('created_at_utc', datetime('now')),
          ('embedding_model_id', 'hash-embedding-v1'),
          ('embedding_dim', '384');
        """;
    await command.ExecuteNonQueryAsync();
}

static async Task InsertAliasAsync(SqliteConnection connection, ActivityAlias alias)
{
    var command = connection.CreateCommand();
    command.CommandText = """
        INSERT OR IGNORE INTO activity_aliases(source, alias, normalized_alias)
        VALUES ($source, $alias, $normalized);
        """;
    command.Parameters.AddWithValue("$source", alias.Source);
    command.Parameters.AddWithValue("$alias", alias.Alias);
    command.Parameters.AddWithValue("$normalized", NormalizeAlias(alias.Alias));
    await command.ExecuteNonQueryAsync();
}

static string NormalizeAlias(string value)
{
    return Regex.Replace(value.Trim().ToLowerInvariant(), @"\s+", " ");
}

static async Task InsertChunkAsync(SqliteConnection connection, KbChunk chunk, byte[] vectorBlob)
{
    var command = connection.CreateCommand();
    command.CommandText = """
        INSERT INTO kb_chunks(
          id, source, group_name, activity_name, title, section_path, chunk_type,
          content, content_hash, kb_version, embedding_model_id, updated_at_utc)
        VALUES (
          $id, $source, $group, $activity, $title, $section, $type,
          $content, $hash, 'dev', 'hash-embedding-v1', datetime('now'));

        INSERT INTO kb_embeddings(chunk_id, dim, vector_blob)
        VALUES ($id, 384, $vector);
        """;
    command.Parameters.AddWithValue("$id", chunk.Id);
    command.Parameters.AddWithValue("$source", chunk.Source);
    command.Parameters.AddWithValue("$group", chunk.GroupName);
    command.Parameters.AddWithValue("$activity", chunk.ActivityName);
    command.Parameters.AddWithValue("$title", chunk.Title);
    command.Parameters.AddWithValue("$section", chunk.SectionPath);
    command.Parameters.AddWithValue("$type", chunk.ChunkType);
    command.Parameters.AddWithValue("$content", chunk.Content);
    command.Parameters.AddWithValue("$hash", chunk.ContentHash);
    command.Parameters.AddWithValue("$vector", vectorBlob);
    await command.ExecuteNonQueryAsync();
}

internal sealed record KbChunk(
    string Id,
    string Source,
    string GroupName,
    string ActivityName,
    string Title,
    string SectionPath,
    string ChunkType,
    string Content,
    string ContentHash);

internal sealed record AnswerCorrectionRecord(
    string? Id,
    DateTimeOffset CreatedAt,
    string? ConversationId,
    string Question,
    string OriginalAnswer,
    string CorrectedAnswer,
    string? Rating,
    string? ExpectedSource,
    string? Memo);

internal sealed record ActivityAliasRecord(
    string Source,
    string[]? Aliases);

internal sealed record ActivityAlias(
    string Source,
    string Alias);
