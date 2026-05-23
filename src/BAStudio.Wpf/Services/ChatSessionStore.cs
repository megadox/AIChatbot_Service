using System.IO;
using System.Text.Json;

namespace BAStudio.Wpf.Services;

/// <summary>
/// Loads, saves, and deletes chat session files under the repository conversation folder.
/// </summary>
public sealed class ChatSessionStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _directory;
    private readonly string _indexPath;

    /// <summary>
    /// Creates a session store rooted at the repository directory.
    /// </summary>
    public ChatSessionStore(string repoRoot)
    {
        _directory = Path.Combine(repoRoot, "ChatBot", "conversations");
        _indexPath = Path.Combine(_directory, "sessions.json");
    }

    /// <summary>
    /// Loads all sessions asynchronously.
    /// </summary>
    public async Task<IReadOnlyList<ChatSessionRecord>> LoadAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.Run(LoadAll, cancellationToken);
    }

    /// <summary>
    /// Loads all sessions from disk and skips corrupt session files.
    /// </summary>
    public IReadOnlyList<ChatSessionRecord> LoadAll()
    {
        Directory.CreateDirectory(_directory);
        if (!File.Exists(_indexPath))
        {
            return [];
        }

        var indexJson = File.ReadAllText(_indexPath);
        var index = JsonSerializer.Deserialize<ChatSessionIndexRecord[]>(indexJson, JsonOptions) ?? [];
        var sessions = new List<ChatSessionRecord>();

        foreach (var item in index.OrderByDescending(i => i.UpdatedAt))
        {
            var path = GetSessionPath(item.Id);
            if (!File.Exists(path))
            {
                continue;
            }

            try
            {
                var json = File.ReadAllText(path);
                var session = JsonSerializer.Deserialize<ChatSessionRecord>(json, JsonOptions);
                if (session is not null)
                {
                    sessions.Add(session);
                }
            }
            catch (JsonException)
            {
                // Skip corrupt session files and keep loading the rest.
            }
        }

        return sessions;
    }

    /// <summary>
    /// Saves one session file and updates the session index.
    /// </summary>
    public async Task SaveAsync(ChatSessionRecord session, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_directory);
        await File.WriteAllTextAsync(
            GetSessionPath(session.Id),
            JsonSerializer.Serialize(session, JsonOptions),
            cancellationToken);

        var index = await LoadIndexAsync(cancellationToken);
        var updated = index
            .Where(i => !string.Equals(i.Id, session.Id, StringComparison.OrdinalIgnoreCase))
            .Append(new ChatSessionIndexRecord(session.Id, session.Title, session.CreatedAt, session.UpdatedAt))
            .OrderByDescending(i => i.UpdatedAt)
            .ToArray();

        await SaveIndexAsync(updated, cancellationToken);
    }

    /// <summary>
    /// Deletes a session file and removes it from the session index.
    /// </summary>
    public async Task DeleteAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_directory);
        var path = GetSessionPath(sessionId);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var index = await LoadIndexAsync(cancellationToken);
        await SaveIndexAsync(
            index.Where(i => !string.Equals(i.Id, sessionId, StringComparison.OrdinalIgnoreCase)).ToArray(),
            cancellationToken);
    }

    private async Task<ChatSessionIndexRecord[]> LoadIndexAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_indexPath))
        {
            return [];
        }

        try
        {
            var json = await File.ReadAllTextAsync(_indexPath, cancellationToken);
            return JsonSerializer.Deserialize<ChatSessionIndexRecord[]>(json, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private Task SaveIndexAsync(ChatSessionIndexRecord[] index, CancellationToken cancellationToken)
    {
        return File.WriteAllTextAsync(
            _indexPath,
            JsonSerializer.Serialize(index.OrderByDescending(i => i.UpdatedAt).ToArray(), JsonOptions),
            cancellationToken);
    }

    private string GetSessionPath(string sessionId) => Path.Combine(_directory, $"{sessionId}.json");
}
