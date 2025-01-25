namespace MintPlayer.DiffParser.Data;

public class Chunk
{
    public string? Header { get; set; }
    public HeaderInfo? HeaderInfo { get; set; }
    public LineDiff[]? Lines { get; set; }
}
