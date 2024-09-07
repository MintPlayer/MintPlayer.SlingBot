using MintPlayer.DiffParser.Abstractions;
using MintPlayer.DiffParser.Data.Enums;
using MintPlayer.DiffParser.Data;
using MintPlayer.EnumerableExtensions;

namespace MintPlayer.DiffParser;

internal class DiffParser : IDiffParser
{
    public Diff Parse(string diff)
    {
        var result = Regexes.HeadRegex().Split(diff)
            .Where(c => !string.IsNullOrEmpty(c))
            .Pairwise()
            .Select((item, index) => new Chunk
            {
                Header = item.Item1,
                Lines = item.Item2?.Trim('\r', '\n').Split([Environment.NewLine], StringSplitOptions.None)
                    .Select(l => new LineDiff
                    {
                        Line = l.Length > 0 ? l.Substring(1) : string.Empty,
                        Status = (l.Length > 0 ? l[0] : ' ') switch
                        {
                            '+' => ELineDiffStatus.Added,
                            '-' => ELineDiffStatus.Removed,
                            _ => ELineDiffStatus.Unchanged,
                        }
                    })
                    .ToArray()
            })
            .ToArray();

        return new Diff
        {
            Chunks = result,
        };
    }
}
