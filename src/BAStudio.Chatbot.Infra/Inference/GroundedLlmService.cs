using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Infra.Inference;

public sealed class GroundedLlmService : ILlmService
{
    public async IAsyncEnumerable<string> StreamAsync(
        string prompt,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var answer = BuildFallbackAnswer(prompt);
        foreach (var part in SplitForStreaming(answer))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(8, cancellationToken);
            yield return part;
        }
    }

    private static string BuildFallbackAnswer(string prompt)
    {
        if (!prompt.Contains("[Manual]", StringComparison.OrdinalIgnoreCase) &&
            prompt.Contains("<|user|>", StringComparison.OrdinalIgnoreCase))
        {
            return "일반 질문 모드는 켜져 있지만, 현재 로컬 일반 답변 모델을 사용할 수 없습니다. 모델 파일을 설정하면 일반 지식 질문에도 답변할 수 있습니다.";
        }

        var sources = Regex.Matches(prompt, @"source:\s*(.+)")
            .Select(m => m.Groups[1].Value.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct()
            .Take(5)
            .ToArray();

        var contents = Regex.Matches(prompt, @"content:\s*\r?\n(?<content>.*?)(?:\r?\n\r?\n--- chunk|\r?\n\[Question\])", RegexOptions.Singleline)
            .Select(m => m.Groups["content"].Value.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Take(3)
            .ToArray();

        if (contents.Length == 0)
        {
            return "문서에 근거가 부족합니다. 질문과 관련된 BA-Studio 도메인이나 액티비티명을 알려주세요.";
        }

        return string.Join(Environment.NewLine + Environment.NewLine, contents) +
               Environment.NewLine + Environment.NewLine +
               "[근거]" + Environment.NewLine +
               string.Join(Environment.NewLine, sources.Select(s => $"- {s}"));
    }

    private static IEnumerable<string> SplitForStreaming(string text)
    {
        foreach (var line in text.Split('\n'))
        {
            yield return line + "\n";
        }
    }
}
