using System.IO;
using System.Text.Json;

namespace BAStudio.Wpf.Services;

public sealed class AnswerCorrectionStore
{
    private readonly string _path;

    public AnswerCorrectionStore(string repoRoot)
    {
        _path = Path.Combine(repoRoot, "qa", "answer_corrections.jsonl");
    }

    public async Task AppendAsync(AnswerCorrectionRecord record, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var json = JsonSerializer.Serialize(record, new JsonSerializerOptions { WriteIndented = false });
        await File.AppendAllTextAsync(_path, json + Environment.NewLine, cancellationToken);
    }
}
