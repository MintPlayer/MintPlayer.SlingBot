namespace MintPlayer.DiffParser.Data;

public class HeaderInfo
{
    public HeaderInfoSide? Left { get; init; }
    public HeaderInfoSide? Right { get; init; }

    public override string ToString() => $"@@ -{Left} +{Right} @@";
}

public class HeaderInfoSide
{
    public int Start { get; init; }
    public int Height { get; init; }

    public override string ToString() => $"{Start},{Height}";
}