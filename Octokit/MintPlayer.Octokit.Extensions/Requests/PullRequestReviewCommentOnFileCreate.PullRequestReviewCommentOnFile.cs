using Octokit;

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
    [global::Octokit.Internal.Parameter(Key = "body")]
    public string Body { get; private set; }

    /// <summary>
    /// The SHA of the commit to comment on.
    /// </summary>
    [global::Octokit.Internal.Parameter(Key = "commit_id")]
    public string CommitId { get; private set; }

    /// <summary>
    /// The relative path of the file to comment on.
    /// </summary>
    [global::Octokit.Internal.Parameter(Key = "path")]
    public string Path { get; private set; }

    [global::Octokit.Internal.Parameter(Key = "subject_type")]
    public ESubjectType SubjectType => ESubjectType.File;
}