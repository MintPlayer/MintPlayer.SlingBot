using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Enums;

public enum ESubjectType
{
    [Parameter(Value = null)]
    Line,

    [Parameter(Value = "file")]
    File,
}