using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Enums;

public enum EPullRequestReviewCommentSide
{
    [Parameter(Value = "LEFT")]
    Left,

    [Parameter(Value = "RIGHT")]
    Right,
}