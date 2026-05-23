using System.IO;
using System.Text.Json;

namespace BAStudio.Wpf.Services;

/// <summary>
/// Appends answer correction records to the QA JSONL file.
/// </summary>
public sealed class AnswerCorrectionStore
{
    private readonly string _path;

    /// <summary>
    /// Creates a correction store rooted at the repository directory.
    /// </summary>
    public AnswerCorrectionStore(string repoRoot)
    {
        _path = Path.Combine(repoRoot, "qa", "answer_corrections.jsonl");
    }

    /// <summary>
    /// Appends a correction record as one JSON line.
    /// </summary>
    public async Task AppendAsync(AnswerCorrectionRecord record, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var json = JsonSerializer.Serialize(record, new JsonSerializerOptions { WriteIndented = false });
        await File.AppendAllTextAsync(_path, json + Environment.NewLine, cancellationToken);
    }
}
