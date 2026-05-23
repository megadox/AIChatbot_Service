using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Policies;

/// <summary>
/// Resolves rule-based domain, activity, and intent hints from a user question.
/// </summary>
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
        ["EMAIL"] = ["메일", "이메일", "email", "smtp", "outlook"],
        ["COMMON"] = ["로그", "대기", "클립보드", "공통"],
        ["BuiltIn"] = ["프로세스", "반복", "조건", "예외", "병렬", "동시", "스레드", "멀티스레드", "thread", "parallel", "process", "while", "foreach"]
    };

    private static readonly HashSet<string> KnownGroups = new(GroupSignals.Keys, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Analyzes a question and returns retrieval preferences for the orchestrator.
    /// </summary>
    public DomainIntent Resolve(string question)
    {
        var normalized = question.Trim();
        var signals = new List<string>();
        string? preferredGroup = null;

        foreach (var (group, words) in GroupSignals)
        {
            if (words.Any(w => MatchesSignal(normalized, w)))
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

    private static bool MatchesSignal(string question, string signal)
    {
        if (string.IsNullOrWhiteSpace(signal))
        {
            return false;
        }

        if (Regex.IsMatch(signal, @"^[A-Za-z0-9_ ]+$"))
        {
            return signal.Contains(' ')
                ? question.Contains(signal, StringComparison.OrdinalIgnoreCase)
                : Regex.IsMatch(question, $@"\b{Regex.Escape(signal)}\b", RegexOptions.IgnoreCase);
        }

        foreach (var token in Tokenize(question))
        {
            var normalized = NormalizeKoreanToken(token);
            if (string.Equals(token, signal, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalized, signal, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (signal.Length <= 2)
            {
                if (token.StartsWith(signal, StringComparison.OrdinalIgnoreCase) ||
                    normalized.StartsWith(signal, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            else if (token.Contains(signal, StringComparison.OrdinalIgnoreCase) ||
                     normalized.Contains(signal, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<string> Tokenize(string text)
    {
        foreach (Match match in Regex.Matches(text.ToLowerInvariant(), @"[A-Za-z0-9_]+|[가-힣]+"))
        {
            yield return match.Value;
        }
    }

    private static string NormalizeKoreanToken(string token)
    {
        if (!Regex.IsMatch(token, @"^[가-힣]+$") || token.Length <= 1)
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
