using System.IO;
using System.Text.Json;

namespace BAStudio.Wpf.Services;

/// <summary>
/// Persists chatbot UI settings under the local ChatBot folder.
/// </summary>
public sealed class ChatbotSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _path;

    public ChatbotSettingsStore(string repoRoot)
    {
        _path = Path.Combine(repoRoot, "ChatBot", "settings.json");
    }

    public ChatbotUiSettings Load()
    {
        if (!File.Exists(_path))
        {
            return new ChatbotUiSettings();
        }

        try
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<ChatbotUiSettings>(json, JsonOptions) ?? new ChatbotUiSettings();
        }
        catch (JsonException)
        {
            return new ChatbotUiSettings();
        }
    }

    public async Task SaveAsync(ChatbotUiSettings settings, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        await File.WriteAllTextAsync(
            _path,
            JsonSerializer.Serialize(settings, JsonOptions),
            cancellationToken);
    }
}
