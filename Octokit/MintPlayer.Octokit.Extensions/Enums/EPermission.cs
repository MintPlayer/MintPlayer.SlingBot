using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Enums;

public enum EPermission
{
    [Parameter(Value = null)]
    None,

    [Parameter(Value = "read")]
    Read,

    [Parameter(Value = "write")]
    Write,
}
