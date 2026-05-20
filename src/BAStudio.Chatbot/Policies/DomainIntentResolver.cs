using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Policies;

public sealed class DomainIntentResolver
{
    private static readonly Dictionary<string, string[]> GroupSignals = new(StringComparer.OrdinalIgnoreCase)
    {
        ["WEB"] = ["웹", "브라우저", "browser", "selector", "html", "url", "alert", "엘리먼트", "javascript", "java script", "자바스크립트", "스크립트"],
        ["EXCEL"] = ["엑셀", "excel", "workbook", "sheet", "cell", "range", "셀", "시트"],
        ["WIN32"] = ["윈도우", "window", "exe", "프로그램", "창", "automation", "마우스", "키보드", "애플리케이션", "오브젝트", "컨트롤", "클릭", "더블클릭"],
        ["SAP"] = ["sap", "세션", "gui"],
        ["ANDROID"] = ["android", "안드로이드", "adb", "apk", "모바일", "앱"],
        ["FILE"] = ["파일", "폴더", "경로", "file", "folder", "path"],
        ["PDF"] = ["pdf"],
        ["WORD"] = ["word", "워드"],
        ["EMAIL"] = ["메일", "email", "smtp", "outlook"],
        ["COMMON"] = ["로그", "대기", "클립보드", "공통"],
        ["BuiltIn"] = ["프로세스", "반복", "조건", "예외", "process", "while", "foreach"]
    };

    private static readonly HashSet<string> KnownGroups = new(GroupSignals.Keys, StringComparer.OrdinalIgnoreCase);

    public DomainIntent Resolve(string question)
    {
        var normalized = question.Trim();
        var signals = new List<string>();
        string? preferredGroup = null;

        foreach (var (group, words) in GroupSignals)
        {
            if (words.Any(w => normalized.Contains(w, StringComparison.OrdinalIgnoreCase)))
            {
                preferredGroup ??= group;
                signals.Add(group);
            }
        }

        var explicitGroup = KnownGroups.FirstOrDefault(g => Regex.IsMatch(normalized, $@"\b{Regex.Escape(g)}\b", RegexOptions.IgnoreCase));
        if (!string.IsNullOrWhiteSpace(explicitGroup))
        {
            preferredGroup = explicitGroup;
            signals.Add(explicitGroup);
        }

        var activityHint = ExtractActivityHint(normalized, preferredGroup);
        var intent = ResolveIntent(normalized);
        return new DomainIntent(preferredGroup, activityHint, intent, signals.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
    }

    private static UserIntent ResolveIntent(string question)
    {
        if (question.Contains("오류", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("에러", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("실패", StringComparison.OrdinalIgnoreCase))
        {
            return UserIntent.Troubleshooting;
        }

        if (question.Contains("어떤", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("추천", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("뭘 써", StringComparison.OrdinalIgnoreCase))
        {
            return UserIntent.ActivityRecommendation;
        }

        if (question.Contains("방법", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("사용법", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("어떻게", StringComparison.OrdinalIgnoreCase))
        {
            return UserIntent.HowTo;
        }

        return UserIntent.ActivityLookup;
    }

    private static string? ExtractActivityHint(string question, string? preferredGroup)
    {
        var withoutGroup = preferredGroup is null
            ? question
            : Regex.Replace(question, Regex.Escape(preferredGroup), " ", RegexOptions.IgnoreCase);

        var match = Regex.Match(withoutGroup, @"[A-Za-z][A-Za-z0-9_]*(?:\([^)]+\))?");
        if (match.Success)
        {
            return match.Value.Trim();
        }

        return null;
    }
}
