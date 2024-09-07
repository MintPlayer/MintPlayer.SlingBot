using MintPlayer.DiffParser.Data;

namespace MintPlayer.DiffParser.Abstractions;

public interface IDiffParser
{
    Diff Parse(string diff);
}