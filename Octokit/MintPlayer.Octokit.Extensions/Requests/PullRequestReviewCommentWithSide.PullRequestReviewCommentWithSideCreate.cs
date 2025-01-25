using MintPlayer.Octokit.Extensions.Enums;
using Octokit;
using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Requests;

public class PullRequestReviewCommentWithSideCreate : RequestParameters
{
	/// <summary>Creates a comment on the specified side of the diff</summary>
	/// <param name="body">The text of the comment</param>
	/// <param name="commitId">The SHA of the commit to comment on</param>
	/// <param name="path">The relative path of the file to comment on</param>
	/// <param name="line">The line index in the diff to comment on</param>
	/// <param name="side">On which side does the comment go</param>
	public PullRequestReviewCommentWithSideCreate(string body, string commitId, string path, int line, EPullRequestReviewCommentSide side)
	{
		Body = body;
		CommitId = commitId;
		Path = path;
		Line = line;
		Side = side;
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

	[Parameter(Key = "line")]
	public int? Line { get; private set; }

	/// <summary>
	/// The side of the comment.
	/// </summary>
	[Parameter(Key = "side")]
	public EPullRequestReviewCommentSide Side { get; private set; }

    //[Parameter(Key = "subject_type")]
    //public ESubjectType SubjectType => ESubjectType.Line;
}