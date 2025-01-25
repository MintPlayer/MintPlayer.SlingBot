using MintPlayer.Octokit.Extensions.Requests;
using System.Net;

namespace Octokit;

public static class PullRequestReviewCommentWithSideExtension
{
    public static async Task<PullRequestReviewCommentWithSide> Create(this IPullRequestReviewCommentsClient client, string owner, string repository, int pullRequestNumber, PullRequestReviewCommentWithSideCreate comment)
    {
        var endpoint = ApiUrls.PullRequestReviewComments(owner, repository, pullRequestNumber);
        var connection = typeof(ApiClient).GetProperty("ApiConnection", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(client) as ApiConnection;
        if (connection == null) throw new InvalidOperationException();

        var response = await connection.Connection.Post<PullRequestReviewCommentWithSide>(endpoint, comment, null, "application/json").ConfigureAwait(false);
        if (response.HttpResponse.StatusCode != HttpStatusCode.Created)
        {
            throw new ApiException("Invalid Status Code returned. Expected a 201", response.HttpResponse.StatusCode);
        }

        return response.Body;
    }
}
