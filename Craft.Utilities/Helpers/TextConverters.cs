using System.Net.Http;
using System.Text;
using Markdig;

namespace Craft.Utilities.Helpers;

public static class TextConverters
{
    public static string? ConvertMarkdownToRtf(string? markdown)
    {
        ArgumentException.ThrowIfNullOrEmpty(markdown, nameof(markdown));

        string html = Markdown.ToHtml(markdown);

        return ConvertHtmlToRtf(html);
    }

    private static string ConvertHtmlToRtf(string html)
    {
        // Simple HTML-to-RTF conversion (basic)
        string rtf = html
            .Replace("<b>", @"\b ")  // Bold start
            .Replace("</b>", @"\b0 ")  // Bold end
            .Replace("<strong>", @"\b ")  // Bold start
            .Replace("</strong>", @"\b0 ")  // Bold end
            .Replace("<i>", @"\i ")  // Italics start
            .Replace("</i>", @"\i0 ")  // Italics end
            .Replace("<br>", @"\line ")  // Line break
            .Replace("<br />", @"\line ")  // Line break
            .Replace("<p>", @"\par ")  // Paragraph start
            .Replace("</p>", @"\par ")  // Paragraph end
            .Replace("<h1>", @"\b\fs28 ")  // Header 1 start
            .Replace("</h1>", @"\b0\fs20\par ")  // Header 1 end
            .Replace("&nbsp;", " ");  // Non-breaking space

        // Wrap in RTF structure
        return $@"{{\rtf1\ansi\deff0 {rtf}}}";
    }
}
