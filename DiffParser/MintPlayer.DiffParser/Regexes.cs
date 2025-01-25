using System.Text.RegularExpressions;

namespace MintPlayer.DiffParser;

internal partial class Regexes
{
    [GeneratedRegex(@"^\@\@(?<header>[\s\-\+\,0-9]+)\@\@", RegexOptions.Multiline)]
    public static partial Regex HeadRegex();

    [GeneratedRegex(@"^\s\-(?<leftstart>[0-9]+)\,(?<leftheight>[0-9]+)\s\+(?<rightstart>[0-9]+)\,(?<rightheight>[0-9]+)\s$", RegexOptions.ExplicitCapture)]
    public static partial Regex HeadNumbersRegex();
}
