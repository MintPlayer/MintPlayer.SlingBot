using MintPlayer.DiffParser.Data.Enums;

namespace MintPlayer.DiffParser.Data;

public class LineDiff
{
    public ELineDiffStatus Status { get; set; }
    public string? Line { get; set; }
}