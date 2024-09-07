using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using MintPlayer.SlingBot.Options;
using Octokit;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.Issues;
using Octokit.Webhooks.Events.PullRequest;
using Octokit.Webhooks.Events.PullRequestReviewComment;

namespace MintPlayer.SlingBot.Demo;

public class GithubProcessor : SlingBotWebhookEventProcessor
{
    private readonly IAuthenticatedGithubService authenticatedGithubService;
    public GithubProcessor(IHostEnvironment environment, IServiceProvider serviceProvider, ISignatureService signatureService, IOptions<BotOptions> botOptions, IAuthenticatedGithubService authenticatedGithubService) : base(environment, serviceProvider, signatureService, botOptions)
    {
        this.authenticatedGithubService = authenticatedGithubService;
    }

    public override async Task<IEnumerable<SocketClient>> GetDevSocketsForWebhook(WebhookEvent webhook, IEnumerable<SocketClient> connectedClients)
    {
        await Task.Delay(1);
        return connectedClients.Where(c => c.GithubUsername == webhook.Sender?.Login);
    }

    protected override async Task ProcessIssuesWebhookAsync(WebhookHeaders headers, IssuesEvent issuesEvent, IssuesAction action)
    {
        var githubClient = await authenticatedGithubService.GetAuthenticatedGithubClient(issuesEvent.Installation!.Id);
        await githubClient.Issue.Comment.Create(issuesEvent.Repository!.Id, (int)issuesEvent.Issue.Number, "Thanks for creating an issue");
    }

    protected override async Task ProcessPullRequestWebhookAsync(WebhookHeaders headers, PullRequestEvent pullRequestEvent, PullRequestAction action)
    {
        await base.ProcessPullRequestWebhookAsync(headers, pullRequestEvent, action);
        var githubClient = await authenticatedGithubService.GetAuthenticatedGithubClient(pullRequestEvent.Installation!.Id);
        await githubClient.PullRequest.ReviewComment.Create(pullRequestEvent.Repository.Id, (int)pullRequestEvent.PullRequest.Number, new PullRequestReviewCommentCreate("Test", pullRequestEvent.PullRequest.Head.Sha, "Test.cs", 5));
    }

    public override async Task ProcessWebhookAsync(IDictionary<string, StringValues> headers, string body)
    {
        await base.ProcessWebhookAsync(headers, body);
    }
}
