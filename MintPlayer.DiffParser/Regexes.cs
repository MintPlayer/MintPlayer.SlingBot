using System.Text.RegularExpressions;

namespace MintPlayer.DiffParser;

internal partial class Regexes
{
    [GeneratedRegex(@"^\@\@(?<header>[\s\-\+\,0-9]+)\@\@", RegexOptions.Multiline)]
    public static partial Regex HeadRegex();
}
