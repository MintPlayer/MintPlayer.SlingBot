using MintPlayer.Octokit.Extensions.Enums;
using Octokit;
using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Requests;

public sealed class PullRequestReviewCommentOnFileCreate : RequestParameters
{
    /// <summary>Creates a comment on the entire file</summary>
    /// <param name="body">The text of the comment</param>
    /// <param name="commitId">The SHA of the commit to comment on</param>
    /// <param name="path">The relative path of the file to comment on</param>
    public PullRequestReviewCommentOnFileCreate(string body, string commitId, string path)
    {
        Body = body;
        CommitId = commitId;
        Path = path;
    }

    /// <summary>
    /// The text of the comment.
    /// </summary>
    [Parameter(Key = "body")]
    public string Body { get; private set; }

    /// <summary>
    /// The SHA of the commit to comment on.
    /// </summary>
    [Parameter(Key = "commit_id")]
    public string CommitId { get; private set; }

    /// <summary>
    /// The relative path of the file to comment on.
    /// </summary>
    [Parameter(Key = "path")]
    public string Path { get; private set; }

    [Parameter(Key = "subject_type")]
    public ESubjectType SubjectType => ESubjectType.File;
}