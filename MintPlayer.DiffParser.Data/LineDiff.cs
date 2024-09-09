using MintPlayer.DiffParser.Data.Enums;

namespace MintPlayer.DiffParser.Data;

public class LineDiff
{
    public ELineDiffStatus Status { get; internal set; }
    public string? Line { get; internal set; }
    public int? LeftIndex { get; internal set; }
    public int? RightIndex { get; internal set; }

    public bool CanCommentLeft => Status.OneOf([ELineDiffStatus.Unchanged, ELineDiffStatus.Removed]);
    public bool CanCommentRight => Status.OneOf([ELineDiffStatus.Added]);

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