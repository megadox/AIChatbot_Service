using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

var root = FindRepoRoot();
var outDir = GetArg(args, "--out") ?? Path.Combine(root, "docs", "product-manuals");
var skipDownload = HasFlag(args, "--skip-download");
var sources = LoadSources(args, root);

Directory.CreateDirectory(outDir);
Directory.CreateDirectory(Path.Combine(outDir, "raw"));
Directory.CreateDirectory(Path.Combine(outDir, "assets"));
Directory.CreateDirectory(Path.Combine(outDir, "normalized"));
Directory.CreateDirectory(Path.Combine(outDir, "reports"));

using var http = new HttpClient();
http.DefaultRequestHeaders.UserAgent.ParseAdd("BAStudioChatbotManualCollector/1.0");

var manifests = new List<ProductManualManifest>();
var audits = new List<ProductManualAudit>();

foreach (var source in sources)
{
    Console.WriteLine($"Collecting {source.Product} {source.Version}: {source.Url}");

    var rawPath = GetRawHtmlPath(outDir, source);
    string html;
    if (skipDownload && File.Exists(rawPath))
    {
        html = await File.ReadAllTextAsync(rawPath, Encoding.UTF8);
    }
    else if (skipDownload)
    {
        Console.WriteLine($"Skipping {source.Product} {source.Version}; raw HTML does not exist: {rawPath}");
        continue;
    }
    else
    {
        html = await http.GetStringAsync(source.Url);
        Directory.CreateDirectory(Path.GetDirectoryName(rawPath)!);
        await File.WriteAllTextAsync(rawPath, html, Encoding.UTF8);
    }

    var normalized = await NormalizeManualAsync(http, outDir, source, html, skipDownload);
    normalized = AppendSupplement(outDir, source, normalized);
    var normalizedPath = GetNormalizedPath(outDir, source);
    Directory.CreateDirectory(Path.GetDirectoryName(normalizedPath)!);
    await File.WriteAllTextAsync(normalizedPath, normalized.Markdown, Encoding.UTF8);

    manifests.Add(new ProductManualManifest(
        source.Product,
        source.Version,
        source.Url.ToString(),
        ToRepoRelative(root, rawPath),
        ToRepoRelative(root, normalizedPath),
        normalized.Images.Select(i => i with { LocalPath = ToRepoRelative(root, i.LocalPath) }).ToArray(),
        normalized.Headings,
        DateTimeOffset.UtcNow));

    audits.Add(new ProductManualAudit(
        source.Product,
        source.Version,
        source.Url.ToString(),
        normalized.Headings.Length,
        normalized.Images.Length,
        normalized.Images.Count(i => !i.Downloaded),
        normalized.EmptyHeadingPaths,
        normalized.Warnings));
}

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

var manifestPath = Path.Combine(outDir, "manual_manifest.json");
var auditPath = Path.Combine(outDir, "reports", "manual_audit.json");
await File.WriteAllTextAsync(manifestPath, JsonSerializer.Serialize(manifests, jsonOptions), Encoding.UTF8);
await File.WriteAllTextAsync(auditPath, JsonSerializer.Serialize(audits, jsonOptions), Encoding.UTF8);

Console.WriteLine($"Manual collection completed: {outDir}");
Console.WriteLine($"Sources: {sources.Length}");
Console.WriteLine($"Manifest: {manifestPath}");
Console.WriteLine($"Audit: {auditPath}");

static ProductManualSource[] LoadSources(string[] args, string root)
{
    var sourceArgs = GetArgs(args, "--source").ToArray();
    if (sourceArgs.Length > 0)
    {
        return sourceArgs.Select(ParseSourceArg).ToArray();
    }

    var sourceFile = GetArg(args, "--sources-file") ?? Path.Combine(root, "qa", "product_manual_sources.json");
    if (File.Exists(sourceFile))
    {
        var records = JsonSerializer.Deserialize<ProductManualSourceRecord[]>(File.ReadAllText(sourceFile, Encoding.UTF8), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        return records
            .Where(record => !string.IsNullOrWhiteSpace(record.Product)
                && !string.IsNullOrWhiteSpace(record.Version)
                && Uri.TryCreate(record.Url, UriKind.Absolute, out _))
            .Select(record => new ProductManualSource(record.Product.Trim(), record.Version.Trim(), new Uri(record.Url.Trim())))
            .ToArray();
    }

    return
    [
        new("BA-Studio", "2.6.0", new Uri("https://docs.batem.com/b_manual/b_studio_2.6.0.html")),
        new("BA-Assist", "2.5.0", new Uri("https://docs.batem.com/b_manual/b_assist_2.5.0.html")),
        new("BA-Assist", "2.5.0-appendix", new Uri("https://docs.batem.com/b_manual/b_assist_appendix_2.5.0.html"))
    ];
}

static ProductManualSource ParseSourceArg(string value)
{
    var parts = value.Split('|', 3, StringSplitOptions.TrimEntries);
    if (parts.Length != 3 || !Uri.TryCreate(parts[2], UriKind.Absolute, out var url))
    {
        throw new ArgumentException("--source must use format Product|Version|https://manual-url");
    }

    return new ProductManualSource(parts[0], parts[1], url);
}

static async Task<NormalizedManual> NormalizeManualAsync(
    HttpClient http,
    string outDir,
    ProductManualSource source,
    string html,
    bool skipDownload)
{
    var title = ExtractTitle(html);
    var body = SanitizeHtml(ExtractBody(html));
    var tokens = ExtractContentTokens(body).ToArray();
    var markdown = new StringBuilder();
    var images = new List<ProductManualImage>();
    var headings = new List<string>();
    var warnings = new List<string>();
    var emptyHeadingPaths = new List<string>();
    var headingStack = new List<string>();
    var contentSinceHeading = 0;

    markdown.AppendLine($"# {NormalizeWhitespace(title.Length == 0 ? $"{source.Product} {source.Version}" : title)}");
    markdown.AppendLine();
    markdown.AppendLine($"- Product: {source.Product}");
    markdown.AppendLine($"- Version: {source.Version}");
    markdown.AppendLine($"- Source URL: {source.Url}");
    markdown.AppendLine();

    foreach (var token in tokens)
    {
        switch (token.Name)
        {
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
                var level = int.Parse(token.Name[1..]);
                var heading = NormalizeWhitespace(HtmlToText(token.InnerHtml));
                if (heading.Length == 0)
                {
                    continue;
                }

                RecordEmptyHeading();
                while (headingStack.Count >= level)
                {
                    headingStack.RemoveAt(headingStack.Count - 1);
                }

                headingStack.Add(heading);
                headings.Add(string.Join(" > ", headingStack));
                markdown.AppendLine($"{new string('#', Math.Min(level + 1, 6))} {heading}");
                markdown.AppendLine();
                contentSinceHeading = 0;
                break;

            case "p":
                var paragraph = NormalizeWhitespace(HtmlToText(token.InnerHtml));
                if (paragraph.Length > 0)
                {
                    markdown.AppendLine(paragraph);
                    markdown.AppendLine();
                    contentSinceHeading++;
                }

                foreach (var image in await ExtractAndDownloadImagesAsync(http, outDir, source, token.FullHtml, skipDownload))
                {
                    images.Add(image);
                    markdown.AppendLine($"![{EscapeMarkdown(image.AltText)}]({ToMarkdownPath(image.LocalPath, outDir)})");
                    markdown.AppendLine();
                    contentSinceHeading++;
                }
                break;

            case "li":
            case "dd":
                var item = NormalizeWhitespace(HtmlToText(token.InnerHtml));
                if (item.Length > 0)
                {
                    markdown.AppendLine(token.Name == "li" ? $"- {item}" : item);
                    if (token.Name == "dd")
                    {
                        markdown.AppendLine();
                    }

                    contentSinceHeading++;
                }
                break;

            case "dt":
                var definitionTerm = NormalizeWhitespace(HtmlToText(token.InnerHtml));
                if (definitionTerm.Length > 0)
                {
                    if (Regex.IsMatch(definitionTerm, @"^Q\d+\.", RegexOptions.IgnoreCase))
                    {
                        markdown.AppendLine($"#### {definitionTerm}");
                    }
                    else
                    {
                        markdown.AppendLine($"- {definitionTerm}");
                    }

                    markdown.AppendLine();
                    contentSinceHeading++;
                }
                break;

            case "table":
                var table = ConvertTableToMarkdown(token.InnerHtml);
                if (table.Length > 0)
                {
                    markdown.AppendLine(table);
                    markdown.AppendLine();
                    contentSinceHeading++;
                }
                break;

            case "img":
                foreach (var image in await ExtractAndDownloadImagesAsync(http, outDir, source, token.FullHtml, skipDownload))
                {
                    images.Add(image);
                    markdown.AppendLine($"![{EscapeMarkdown(image.AltText)}]({ToMarkdownPath(image.LocalPath, outDir)})");
                    markdown.AppendLine();
                    contentSinceHeading++;
                }
                break;
        }
    }

    RecordEmptyHeading();

    if (headings.Count == 0)
    {
        warnings.Add("No headings were extracted from the manual.");
    }

    if (images.Count == 0)
    {
        warnings.Add("No images were found in the manual.");
    }

    return new NormalizedManual(markdown.ToString().Trim() + Environment.NewLine, images.ToArray(), headings.ToArray(), emptyHeadingPaths.ToArray(), warnings.ToArray());

    void RecordEmptyHeading()
    {
        if (headingStack.Count > 0 && contentSinceHeading == 0)
        {
            var path = string.Join(" > ", headingStack);
            if (!emptyHeadingPaths.Contains(path, StringComparer.Ordinal))
            {
                emptyHeadingPaths.Add(path);
            }
        }
    }
}

static NormalizedManual AppendSupplement(string outDir, ProductManualSource source, NormalizedManual normalized)
{
    var supplementPath = Path.Combine(outDir, "supplements", Slug(source.Product), $"{Slug(source.Version)}.md");
    if (!File.Exists(supplementPath))
    {
        return normalized;
    }

    var supplement = File.ReadAllText(supplementPath, Encoding.UTF8).Trim();
    if (string.IsNullOrWhiteSpace(supplement))
    {
        return normalized;
    }

    var markdown = normalized.Markdown.TrimEnd() + Environment.NewLine + Environment.NewLine + supplement + Environment.NewLine;
    var supplementHeadings = Regex.Matches(supplement, @"^(?<marks>#{2,6})\s+(?<title>.+?)\s*$", RegexOptions.Multiline)
        .Select(match => $"Source Supplement > {match.Groups["title"].Value.Trim()}")
        .ToArray();

    return normalized with
    {
        Markdown = markdown,
        Headings = normalized.Headings.Concat(supplementHeadings).ToArray()
    };
}

static async Task<IEnumerable<ProductManualImage>> ExtractAndDownloadImagesAsync(
    HttpClient http,
    string outDir,
    ProductManualSource source,
    string html,
    bool skipDownload)
{
    var images = new List<ProductManualImage>();
    foreach (Match match in Regex.Matches(html, @"<img\b(?<attrs>[^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
    {
        var attrs = match.Groups["attrs"].Value;
        var src = GetAttribute(attrs, "src");
        if (string.IsNullOrWhiteSpace(src))
        {
            continue;
        }

        var absolute = new Uri(source.Url, WebUtility.HtmlDecode(src));
        var alt = NormalizeWhitespace(GetAttribute(attrs, "alt") ?? string.Empty);
        if (alt.Length == 0)
        {
            alt = Path.GetFileNameWithoutExtension(absolute.LocalPath);
        }

        var localPath = GetImagePath(outDir, source, absolute);
        Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);
        var downloaded = File.Exists(localPath);

        if (!skipDownload && !downloaded)
        {
            try
            {
                var bytes = await http.GetByteArrayAsync(absolute);
                await File.WriteAllBytesAsync(localPath, bytes);
                downloaded = true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException)
            {
                downloaded = false;
            }
        }

        images.Add(new ProductManualImage(absolute.ToString(), localPath, alt, downloaded));
    }

    return images;
}

static IEnumerable<HtmlToken> ExtractContentTokens(string html)
{
    var pattern = @"<(?<name>h[1-6]|p|li|dt|dd|table|img)\b(?<attrs>[^>]*)>(?:(?<inner>.*?)(?:</\k<name>>))?";
    foreach (Match match in Regex.Matches(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
    {
        var name = match.Groups["name"].Value.ToLowerInvariant();
        yield return new HtmlToken(name, match.Value, match.Groups["inner"].Value);
    }
}

static string ConvertTableToMarkdown(string html)
{
    var rows = Regex.Matches(html, @"<tr\b[^>]*>(?<row>.*?)</tr>", RegexOptions.IgnoreCase | RegexOptions.Singleline)
        .Select(row => Regex.Matches(row.Groups["row"].Value, @"<t[dh]\b[^>]*>(?<cell>.*?)</t[dh]>", RegexOptions.IgnoreCase | RegexOptions.Singleline)
            .Select(cell => NormalizeWhitespace(HtmlToText(cell.Groups["cell"].Value)).Replace("|", "\\|", StringComparison.Ordinal))
            .Where(cell => cell.Length > 0)
            .ToArray())
        .Where(cells => cells.Length > 0)
        .ToArray();

    if (rows.Length == 0)
    {
        return string.Empty;
    }

    var builder = new StringBuilder();
    builder.AppendLine("| " + string.Join(" | ", rows[0]) + " |");
    builder.AppendLine("| " + string.Join(" | ", rows[0].Select(_ => "---")) + " |");
    foreach (var row in rows.Skip(1))
    {
        builder.AppendLine("| " + string.Join(" | ", row) + " |");
    }

    return builder.ToString().TrimEnd();
}

static string HtmlToText(string html)
{
    var text = Regex.Replace(html, @"<\s*br\s*/?>", "\n", RegexOptions.IgnoreCase);
    text = Regex.Replace(text, @"</\s*(p|div|li|tr|h[1-6])\s*>", "\n", RegexOptions.IgnoreCase);
    text = Regex.Replace(text, @"<[^>]+>", " ");
    return WebUtility.HtmlDecode(text);
}

static string ExtractTitle(string html)
{
    var title = Regex.Match(html, @"<title\b[^>]*>(?<title>.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    if (title.Success)
    {
        return HtmlToText(title.Groups["title"].Value);
    }

    var h1 = Regex.Match(html, @"<h1\b[^>]*>(?<title>.*?)</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    return h1.Success ? HtmlToText(h1.Groups["title"].Value) : string.Empty;
}

static string ExtractBody(string html)
{
    var body = Regex.Match(html, @"<body\b[^>]*>(?<body>.*?)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    return body.Success ? body.Groups["body"].Value : html;
}

static string SanitizeHtml(string html)
{
    var sanitized = Regex.Replace(html, @"<!--.*?-->", string.Empty, RegexOptions.Singleline);
    sanitized = Regex.Replace(sanitized, @"<script\b[^>]*>.*?</script>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    sanitized = Regex.Replace(sanitized, @"<style\b[^>]*>.*?</style>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    sanitized = Regex.Replace(sanitized, @"<noscript\b[^>]*>.*?</noscript>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    return sanitized;
}

static string? GetAttribute(string attrs, string name)
{
    var match = Regex.Match(attrs, $@"\b{Regex.Escape(name)}\s*=\s*(?:""(?<value>[^""]*)""|'(?<value>[^']*)'|(?<value>[^\s>]+))", RegexOptions.IgnoreCase);
    return match.Success ? WebUtility.HtmlDecode(match.Groups["value"].Value) : null;
}

static string NormalizeWhitespace(string value)
{
    return Regex.Replace(value.Replace('\u00a0', ' '), @"\s+", " ").Trim();
}

static string EscapeMarkdown(string value)
{
    return value.Replace("[", "\\[", StringComparison.Ordinal).Replace("]", "\\]", StringComparison.Ordinal);
}

static string ToMarkdownPath(string path, string outDir)
{
    return Path.GetRelativePath(Path.Combine(outDir, "normalized"), path).Replace('\\', '/');
}

static string GetRawHtmlPath(string outDir, ProductManualSource source)
{
    return Path.Combine(outDir, "raw", Slug(source.Product), $"{Slug(source.Version)}.html");
}

static string GetNormalizedPath(string outDir, ProductManualSource source)
{
    return Path.Combine(outDir, "normalized", Slug(source.Product), $"{Slug(source.Version)}.md");
}

static string GetImagePath(string outDir, ProductManualSource source, Uri imageUrl)
{
    var extension = Path.GetExtension(imageUrl.LocalPath);
    if (string.IsNullOrWhiteSpace(extension) || extension.Length > 8)
    {
        extension = ".bin";
    }

    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(imageUrl.ToString()))).ToLowerInvariant()[..16];
    var fileName = $"{Path.GetFileNameWithoutExtension(imageUrl.LocalPath)}-{hash}{extension}";
    return Path.Combine(outDir, "assets", Slug(source.Product), Slug(source.Version), fileName);
}

static string Slug(string value)
{
    var cleaned = Regex.Replace(value.ToLowerInvariant(), @"[^a-z0-9가-힣._-]+", "-");
    return cleaned.Trim('-');
}

static string ToRepoRelative(string root, string path)
{
    return Path.GetRelativePath(root, path).Replace('\\', '/');
}

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

static IEnumerable<string> GetArgs(string[] args, string name)
{
    for (var i = 0; i < args.Length - 1; i++)
    {
        if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
        {
            yield return args[i + 1];
        }
    }
}

static bool HasFlag(string[] args, string name)
{
    return args.Any(arg => string.Equals(arg, name, StringComparison.OrdinalIgnoreCase));
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

internal sealed record ProductManualSource(string Product, string Version, Uri Url);

internal sealed record ProductManualSourceRecord(string Product, string Version, string Url);

internal sealed record HtmlToken(string Name, string FullHtml, string InnerHtml);

internal sealed record NormalizedManual(
    string Markdown,
    ProductManualImage[] Images,
    string[] Headings,
    string[] EmptyHeadingPaths,
    string[] Warnings);

internal sealed record ProductManualImage(string Url, string LocalPath, string AltText, bool Downloaded);

internal sealed record ProductManualManifest(
    string Product,
    string Version,
    string SourceUrl,
    string RawHtmlPath,
    string NormalizedMarkdownPath,
    ProductManualImage[] Images,
    string[] SectionPaths,
    DateTimeOffset CollectedAtUtc);

internal sealed record ProductManualAudit(
    string Product,
    string Version,
    string SourceUrl,
    int HeadingCount,
    int ImageCount,
    int MissingImageCount,
    string[] EmptySectionPaths,
    string[] Warnings);
