using MintPlayer.DiffParser.Data.Enums;

namespace MintPlayer.DiffParser.Data;

public class LineDiff
{
    public ELineDiffStatus Status { get; set; }
    public string? Line { get; set; }
    public int? LeftIndex { get; set; }
    public int? RightIndex { get; set; }

    public override string ToString()
    {
        switch (Status)
        {
            case ELineDiffStatus.Unchanged: return $" {Line}";
            case ELineDiffStatus.Added: return $"+{Line}";
            case ELineDiffStatus.Removed: return $"-{Line}";
            default: throw new InvalidOperationException();
        }
    }
}