using System.Text;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Prompting;

/// <summary>
/// Builds the instruction prompt used for grounded BA-Studio manual answers.
/// </summary>
public sealed class PromptBuilder : IPromptBuilder
{
    /// <summary>
    /// Combines the user question and retrieved chunks into a single chat prompt.
    /// </summary>
    public string Build(string question, IReadOnlyList<RetrievedChunk> chunks)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<|system|>");
        sb.AppendLine("너는 BA-Studio 내장 도움말 챗봇이다.");
        sb.AppendLine("반드시 [Manual]에 있는 내용만 근거로 답변한다.");
        sb.AppendLine("[Manual]에 없는 내용은 추측하지 않는다.");
        sb.AppendLine("속성명, 기본값, 옵션, 액티비티명은 원문 표기를 유지한다.");
        sb.AppendLine("답변 마지막에는 항상 [근거] 섹션을 만들고 사용한 source를 bullet로 나열한다.");
        sb.AppendLine("근거가 부족하면 \"문서에 근거가 부족합니다\"라고 말하고 필요한 추가 정보를 질문한다.");
        sb.AppendLine("<|end|>");
        sb.AppendLine();
        sb.AppendLine("<|user|>");
        sb.AppendLine("[Manual]");

        for (var i = 0; i < chunks.Count; i++)
        {
            var c = chunks[i];
            sb.AppendLine($"--- chunk {i + 1}");
            sb.AppendLine($"source: {c.Source}");
            sb.AppendLine($"group: {c.GroupName}");
            sb.AppendLine($"activity: {c.ActivityName ?? "-"}");
            sb.AppendLine($"section: {c.SectionPath}");
            sb.AppendLine($"type: {c.ChunkType}");
            sb.AppendLine("content:");
            sb.AppendLine(c.Content);
            sb.AppendLine();
        }

        sb.AppendLine("[Question]");
        sb.AppendLine(question);
        sb.AppendLine("<|end|>");
        sb.AppendLine();
        sb.AppendLine("<|assistant|>");
        return sb.ToString();
    }
}
