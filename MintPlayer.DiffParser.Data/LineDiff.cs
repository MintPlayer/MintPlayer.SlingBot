using MintPlayer.DiffParser.Data.Enums;

namespace MintPlayer.DiffParser.Data;

public class LineDiff
{
    public ELineDiffStatus Status { get; set; }
    public string? Line { get; set; }
    public int? LeftIndex { get; set; }
    public int? RightIndex { get; set; }

    private string Num(int? val) => val == null ? string.Empty : val.ToString()!;

    public override string ToString()
    {
        switch (Status)
        {
            case ELineDiffStatus.Unchanged: return $" {Line} ({Num(LeftIndex)},{Num(RightIndex)})";
            case ELineDiffStatus.Added: return $"+{Line} ({Num(LeftIndex)},{Num(RightIndex)})";
            case ELineDiffStatus.Removed: return $"-{Line} ({Num(LeftIndex)},{Num(RightIndex)})";
            default: throw new InvalidOperationException();
        }
    }
}