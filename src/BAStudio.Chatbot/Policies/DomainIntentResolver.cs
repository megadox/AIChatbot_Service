using System.Text.Json;
using System.Text.RegularExpressions;
using BAStudio.Chatbot.Contracts;

namespace BAStudio.Chatbot.Policies;

/// <summary>
/// Resolves rule-based domain, activity, and intent hints from a user question.
/// </summary>
public sealed class DomainIntentResolver
{
    private static readonly Regex UrlOrDomainRegex = new(
        @"(?:https?://|www\.)[^\s]+|(?<![A-Za-z0-9-])(?:[a-z0-9-]+\.)+[a-z]{2,}(?=$|[^A-Za-z0-9-])",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex ExcelRangeRegex = new(
        @"\b[A-Z]{1,3}\d+\s*:\s*[A-Z]{1,3}\d+\b|\b[A-Z]{1,3}\d+\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex WindowsPathRegex = new(
        @"\b[A-Z]:\\[^\s]+|\\\\[^\s\\]+\\[^\s]+",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
        ["BuiltIn"] = ["프로세스", "반복", "조건", "예외", "병렬", "동시", "스레드", "멀티", "멀티스레드", "멀티 실행", "thread", "parallel", "process", "while", "foreach"]
    };

    private static readonly HashSet<string> KnownGroups = new(GroupSignals.Keys, StringComparer.OrdinalIgnoreCase);

    private static readonly Lazy<IReadOnlyList<ScenarioActionConcept>> ScenarioActionConcepts = new(LoadScenarioActionConcepts);

    /// <summary>
    /// Analyzes a question and returns retrieval preferences for the orchestrator.
    /// </summary>
    public DomainIntent Resolve(string question)
    {
        var normalized = question.Trim();
        var signals = new List<string>();
        string? preferredGroup = null;
        string? requiredGroup = null;

        foreach (var (group, words) in GroupSignals)
        {
            if (words.Any(w => MatchesSignal(normalized, w)))
            {
                preferredGroup ??= group;
                signals.Add(group);
            }
        }

        ApplyEntitySignals(normalized, signals, ref preferredGroup, ref requiredGroup);

        var explicitGroup = KnownGroups.FirstOrDefault(g => Regex.IsMatch(normalized, $@"\b{Regex.Escape(g)}\b", RegexOptions.IgnoreCase));
        if (!string.IsNullOrWhiteSpace(explicitGroup))
        {
            preferredGroup = explicitGroup;
            signals.Add(explicitGroup);
        }

        var intent = ResolveIntent(normalized);
        if (intent == UserIntent.ProductGuide)
        {
            signals.Add("ProductManual");
            var productSourceHint = ResolveProductGuideSourceHint(normalized);
            return new DomainIntent(
                null,
                null,
                intent,
                signals.Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
                BuildProductGuideQuery(normalized),
                null,
                productSourceHint,
                []);
        }

        var actionConcept = ResolveScenarioActionConcept(normalized, preferredGroup, requiredGroup);
        if (actionConcept is not null)
        {
            preferredGroup = actionConcept.Group;
            requiredGroup ??= actionConcept.Group;
            signals.Add(actionConcept.Group);
            intent = UserIntent.ActivityRecommendation;
        }

        var activityHint = actionConcept?.Activity ?? (requiredGroup == "WEB" && UrlOrDomainRegex.IsMatch(normalized) ? null : ExtractActivityHint(normalized, preferredGroup));
        var rewrittenQuery = BuildRewrittenQuery(normalized, preferredGroup, actionConcept);
        return new DomainIntent(
            preferredGroup,
            activityHint,
            intent,
            signals.Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
            rewrittenQuery,
            requiredGroup,
            actionConcept?.Source,
            actionConcept?.RewriteTerms ?? []);
    }

    private static void ApplyEntitySignals(string question, List<string> signals, ref string? preferredGroup, ref string? requiredGroup)
    {
        if (MatchesAny(question, ["로그", "로그 메시지", "log"]))
        {
            preferredGroup = "COMMON";
            requiredGroup ??= "COMMON";
            signals.Add("COMMON");
        }

        if (UrlOrDomainRegex.IsMatch(question))
        {
            preferredGroup = "WEB";
            requiredGroup = "WEB";
            signals.Add("WEB");
        }

        if (ExcelRangeRegex.IsMatch(question) ||
            MatchesAny(question, ["셀", "시트", "범위"]))
        {
            preferredGroup ??= "EXCEL";
            requiredGroup ??= "EXCEL";
            signals.Add("EXCEL");
        }

        if (WindowsPathRegex.IsMatch(question))
        {
            preferredGroup ??= "FILE";
            requiredGroup = "FILE";
            signals.Add("FILE");
        }

        if (MatchesAny(question, ["파일", "폴더", "경로"]) &&
            !MatchesAny(question, ["로그", "로그 메시지"]))
        {
            preferredGroup ??= "FILE";
            signals.Add("FILE");
        }

        if (MatchesAny(question, ["마우스", "좌표", "컨트롤"]))
        {
            preferredGroup ??= "WIN32";
            requiredGroup ??= "WIN32";
            signals.Add("WIN32");
        }

        if (MatchesAny(question, ["창"]))
        {
            preferredGroup ??= "WIN32";
            signals.Add("WIN32");
        }
    }

    private static bool MatchesAny(string question, IReadOnlyList<string> terms)
    {
        return terms.Any(term => MatchesSignal(question, term));
    }

    private static ScenarioActionConcept? ResolveScenarioActionConcept(string question, string? preferredGroup, string? requiredGroup)
    {
        var group = requiredGroup ?? preferredGroup;
        var hasUrl = UrlOrDomainRegex.IsMatch(question);

        var best = ScenarioActionConcepts.Value
            .Select(concept => new
            {
                Concept = concept,
                Score = ScoreScenarioConcept(question, concept, group, hasUrl)
            })
            .Where(candidate => candidate.Score >= 0.7)
            .OrderByDescending(candidate => candidate.Score)
            .ThenByDescending(candidate => candidate.Concept.IntentTerms.Length)
            .FirstOrDefault();

        return best?.Concept;
    }

    private static double ScoreScenarioConcept(string question, ScenarioActionConcept concept, string? group, bool hasUrl)
    {
        if (group is not null && !string.Equals(group, concept.Group, StringComparison.OrdinalIgnoreCase))
        {
            return 0;
        }

        if (!EntityRequirementsMatch(question, concept, hasUrl))
        {
            return 0;
        }

        if (string.Equals(concept.Source, "WEB/Navigate.md", StringComparison.OrdinalIgnoreCase) &&
            !LooksLikeNavigateScenario(question, hasUrl))
        {
            return 0;
        }

        if (string.Equals(concept.Source, "WEB/Click.md", StringComparison.OrdinalIgnoreCase) &&
            !MatchesAny(question, ["웹", "브라우저", "엘리먼트", "요소", "selector", "셀렉터"]))
        {
            return 0;
        }

        var score = 0.0;
        foreach (var term in concept.IntentTerms)
        {
            if (MatchesSignal(question, term))
            {
                score += ScoreMatchedTerm(term);
            }
        }

        foreach (var term in concept.NegativeTerms)
        {
            if (MatchesSignal(question, term))
            {
                score -= ScoreMatchedTerm(term) + 0.35;
            }
        }

        score += ScoreEntityAffinity(question, concept, hasUrl);

        if (question.Contains(concept.Activity, StringComparison.OrdinalIgnoreCase))
        {
            score += 1.2;
        }

        if (group is not null && string.Equals(group, concept.Group, StringComparison.OrdinalIgnoreCase))
        {
            score += 0.25;
        }

        return Math.Max(0, score);
    }

    private static double ScoreEntityAffinity(string question, ScenarioActionConcept concept, bool hasUrl)
    {
        var score = 0.0;
        foreach (var entity in concept.RequiredEntities)
        {
            var name = entity.EndsWith("?", StringComparison.Ordinal) ? entity[..^1] : entity;
            var matched = name switch
            {
                "url" => hasUrl,
                "excel" => ExcelRangeRegex.IsMatch(question) || MatchesAny(question, ["엑셀", "셀", "시트", "범위"]),
                "cellAddress" => ExcelRangeRegex.IsMatch(question),
                "file" => WindowsPathRegex.IsMatch(question) || MatchesAny(question, ["파일", "폴더", "경로"]),
                _ => false
            };

            if (matched)
            {
                score += 0.75;
            }
        }

        if (ExcelRangeRegex.IsMatch(question) &&
            string.Equals(concept.Source, "EXCEL/GetActiveCell.md", StringComparison.OrdinalIgnoreCase))
        {
            score -= 1.2;
        }

        return score;
    }

    private static bool EntityRequirementsMatch(string question, ScenarioActionConcept concept, bool hasUrl)
    {
        foreach (var entity in concept.RequiredEntities)
        {
            var optional = entity.EndsWith("?", StringComparison.Ordinal);
            var name = optional ? entity[..^1] : entity;
            var matched = name switch
            {
                "url" => hasUrl,
                "excel" => ExcelRangeRegex.IsMatch(question) || MatchesAny(question, ["엑셀", "셀", "시트", "범위"]),
                "cellAddress" => ExcelRangeRegex.IsMatch(question),
                "file" => WindowsPathRegex.IsMatch(question) || MatchesAny(question, ["파일", "폴더", "경로"]),
                _ => true
            };

            if (!optional && !matched)
            {
                return false;
            }
        }

        return true;
    }

    private static double ScoreMatchedTerm(string term)
    {
        var tokenCount = Tokenize(term).Count();
        if (term.Length >= 12 || tokenCount >= 3)
        {
            return 1.4;
        }

        if (term.Length >= 5 || tokenCount >= 2)
        {
            return 1.0;
        }

        return 0.45;
    }

    private static bool LooksLikeNavigateScenario(string question, bool hasUrl)
    {
        if (Regex.IsMatch(question, @"에서\s*.*(?:으로|로)\s*이동", RegexOptions.IgnoreCase))
        {
            return true;
        }

        if (hasUrl && MatchesAny(question, ["변경", "이동"]))
        {
            return true;
        }

        return MatchesAny(question, ["사이트 이동", "url 변경", "url 이동", "페이지 이동", "다른 사이트로 이동"]);
    }

    private static string BuildRewrittenQuery(string question, string? preferredGroup, ScenarioActionConcept? concept)
    {
        var parts = new List<string> { question };
        if (!string.IsNullOrWhiteSpace(preferredGroup))
        {
            parts.Add(preferredGroup);
        }

        if (UrlOrDomainRegex.IsMatch(question))
        {
            parts.AddRange(["WEB", "브라우저", "URL", "사이트"]);
        }

        if (ExcelRangeRegex.IsMatch(question))
        {
            parts.AddRange(["EXCEL", "셀", "범위"]);
        }

        if (WindowsPathRegex.IsMatch(question))
        {
            parts.AddRange(["FILE", "파일", "경로"]);
        }

        if (concept is not null)
        {
            parts.Add(concept.Activity);
            parts.Add(concept.Source);
            parts.AddRange(concept.RewriteTerms);
        }

        return string.Join(' ', parts.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct(StringComparer.OrdinalIgnoreCase));
    }

    private static bool MatchesSignal(string question, string signal)
    {
        if (string.IsNullOrWhiteSpace(signal))
        {
            return false;
        }

        if (signal.Contains(' ') && question.Contains(signal, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (signal.Contains(' '))
        {
            var questionTokens = Tokenize(question)
                .Select(NormalizeKoreanToken)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var signalTokens = Tokenize(signal)
                .Select(NormalizeKoreanToken)
                .Where(t => t.Length > 1)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            if (signalTokens.Length > 0 && signalTokens.All(questionTokens.Contains))
            {
                return true;
            }
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
        if (LooksLikeProductGuideQuestion(question))
        {
            return UserIntent.ProductGuide;
        }

        if (question.Contains("오류", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("에러", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("실패", StringComparison.OrdinalIgnoreCase))
        {
            return UserIntent.Troubleshooting;
        }

        if (question.Contains("어떤", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("추천", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("뭘 써", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("이때 사용하는", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("사용하는 것은", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("하고 싶", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("하려면", StringComparison.OrdinalIgnoreCase))
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

    private static bool LooksLikeProductGuideQuestion(string question)
    {
        var productTerms = new[]
        {
            "ba-studio", "ba studio", "ba스튜디오", "스튜디오", "ba-assist", "ba assist", "ba어시스트", "어시스트"
        };
        var guideTerms = new[]
        {
            "프로젝트", "태스크", "스케줄", "스케줄러", "메뉴", "툴바", "설치", "화면", "인터페이스",
            "실행", "등록", "생성", "설정", "로그", "패키지", "셀렉터 편집기", "디버깅", "라이브러리",
            "어떻게", "방법", "사용법", "차이"
        };

        if (productTerms.Any(term => question.Contains(term, StringComparison.OrdinalIgnoreCase)) &&
            guideTerms.Any(term => question.Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return MatchesAny(question, ["BA-Assist와 BA-Server", "BA Assist와 BA Server", "BA-Worker", "logs.db", "Queue", "Sequential"]);
    }

    private static string BuildProductGuideQuery(string question)
    {
        var parts = new List<string> { question, "제품 매뉴얼", "사용자 매뉴얼" };
        if (question.Contains("스튜디오", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("BA-Studio", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("BA Studio", StringComparison.OrdinalIgnoreCase))
        {
            parts.AddRange(["BA-Studio", "BA Studio"]);
        }

        if (question.Contains("어시스트", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("BA-Assist", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("BA Assist", StringComparison.OrdinalIgnoreCase))
        {
            parts.AddRange(["BA-Assist", "BA Assist"]);
        }

        return string.Join(' ', parts.Distinct(StringComparer.OrdinalIgnoreCase));
    }

    private static string? ResolveProductGuideSourceHint(string question)
    {
        if (MatchesAny(question, ["BA-Server", "BA Server", "BA-Worker", "BA Worker", "logs.db", "Queue", "Sequential"]))
        {
            return "docs/product-manuals/normalized/ba-assist/2.5.0-appendix.md";
        }

        if (question.Contains("BA-Assist", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("BA Assist", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("어시스트", StringComparison.OrdinalIgnoreCase))
        {
            return "docs/product-manuals/normalized/ba-assist/2.5.0.md";
        }

        if (question.Contains("BA-Studio", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("BA Studio", StringComparison.OrdinalIgnoreCase) ||
            question.Contains("스튜디오", StringComparison.OrdinalIgnoreCase))
        {
            return "docs/product-manuals/normalized/ba-studio/2.6.0.md";
        }

        return null;
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

    private static IReadOnlyList<ScenarioActionConcept> LoadScenarioActionConcepts()
    {
        var path = FindScenarioMapPath();
        if (path is not null)
        {
            try
            {
                using var document = JsonDocument.Parse(File.ReadAllText(path));
                return document.RootElement
                    .EnumerateArray()
                    .Select(ReadScenarioActionConcept)
                    .Where(r => !string.IsNullOrWhiteSpace(r.Source) &&
                                !string.IsNullOrWhiteSpace(r.Group) &&
                                !string.IsNullOrWhiteSpace(r.Activity))
                    .Select(r => new ScenarioActionConcept(
                        r.Source.Trim().Replace('\\', '/'),
                        r.Group.Trim(),
                        r.Activity.Trim(),
                        CleanTerms(r.IntentTerms),
                        CleanTerms(r.NegativeTerms),
                        CleanTerms(r.RewriteTerms),
                        CleanTerms(r.RequiredEntities)))
                    .ToArray();
            }
            catch (IOException)
            {
                return FallbackScenarioActionConcepts();
            }
            catch (JsonException)
            {
                return FallbackScenarioActionConcepts();
            }
        }

        return FallbackScenarioActionConcepts();
    }

    private static string? FindScenarioMapPath()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "qa", "activity_scenarios.json");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            if (File.Exists(Path.Combine(dir.FullName, "commands.json")))
            {
                break;
            }

            dir = dir.Parent;
        }

        var cwdCandidate = Path.Combine(Directory.GetCurrentDirectory(), "qa", "activity_scenarios.json");
        return File.Exists(cwdCandidate) ? cwdCandidate : null;
    }

    private static string[] CleanTerms(IReadOnlyList<string>? terms)
    {
        return terms is null
            ? []
            : terms
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
    }

    private static ScenarioActionConceptRecord ReadScenarioActionConcept(JsonElement element)
    {
        return new ScenarioActionConceptRecord(
            ReadString(element, "source"),
            ReadString(element, "group"),
            ReadString(element, "activity"),
            ReadStringArray(element, "intentTerms"),
            ReadStringArray(element, "negativeTerms"),
            ReadStringArray(element, "rewriteTerms"),
            ReadStringArray(element, "requiredEntities"));
    }

    private static string ReadString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString() ?? ""
            : "";
    }

    private static string[] ReadStringArray(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            return value.ValueKind == JsonValueKind.String ? [value.GetString() ?? ""] : [];
        }

        return value
            .EnumerateArray()
            .Where(item => item.ValueKind == JsonValueKind.String)
            .Select(item => item.GetString() ?? "")
            .ToArray();
    }

    private static IReadOnlyList<ScenarioActionConcept> FallbackScenarioActionConcepts()
    {
        return
        [
            new(
                Source: "WEB/OpenBrowser.md",
                Group: "WEB",
                Activity: "OpenBrowser",
                IntentTerms: ["브라우저 오픈", "브라우저 열", "웹 브라우저 열", "웹 브라우저 오픈", "사이트 오픈", "페이지 오픈", "url 오픈", "오픈"],
                NegativeTerms: ["닫기", "종료", "새로고침", "이동", "변경"],
                RewriteTerms: ["WEB", "브라우저", "오픈", "열기", "OpenBrowser", "웹 브라우저를 새로 열", "URL 페이지 오픈"],
                RequiredEntities: ["url?"]),
            new(
                Source: "WEB/Navigate.md",
                Group: "WEB",
                Activity: "Navigate",
                IntentTerms: ["사이트 이동", "url 변경", "url 이동", "페이지 이동", "다른 사이트로 이동", "이동"],
                NegativeTerms: ["오픈", "열기", "새로 열", "닫기", "종료", "새로고침"],
                RewriteTerms: ["WEB", "브라우저", "URL", "사이트 이동", "Navigate", "새로운 웹 사이트로 이동"],
                RequiredEntities: ["url"]),
            new(
                Source: "WEB/Refresh.md",
                Group: "WEB",
                Activity: "Refresh",
                IntentTerms: ["새로고침", "다시 불러오기", "다시 불러", "reload", "refresh"],
                NegativeTerms: ["오픈", "열기", "닫기", "종료", "이동"],
                RewriteTerms: ["WEB", "브라우저", "현재 페이지", "새로고침", "Refresh", "reload"],
                RequiredEntities: []),
            new(
                Source: "WEB/Click.md",
                Group: "WEB",
                Activity: "Click",
                IntentTerms: ["웹 요소 클릭", "웹 엘리먼트 클릭", "웹 클릭", "클릭"],
                NegativeTerms: [],
                RewriteTerms: ["WEB", "웹 요소", "엘리먼트", "클릭", "Click"],
                RequiredEntities: [])
        ];
    }

    private sealed record ScenarioActionConcept(
        string Source,
        string Group,
        string Activity,
        string[] IntentTerms,
        string[] NegativeTerms,
        string[] RewriteTerms,
        string[] RequiredEntities);

    private sealed record ScenarioActionConceptRecord(
        string Source,
        string Group,
        string Activity,
        string[] IntentTerms,
        string[] NegativeTerms,
        string[] RewriteTerms,
        string[] RequiredEntities);
}
