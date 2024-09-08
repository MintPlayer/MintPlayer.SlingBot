namespace MintPlayer.DiffParser.Data;

public class HeaderInfo
{
    public HeaderInfoSide? Left { get; init; }
    public HeaderInfoSide? Right { get; init; }
}

public class HeaderInfoSide
{
    public int Start { get; set; }
    public int Height { get; set; }
}