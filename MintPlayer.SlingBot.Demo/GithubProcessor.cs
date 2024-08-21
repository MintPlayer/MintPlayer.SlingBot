using Microsoft.Extensions.Primitives;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.Issues;

namespace MintPlayer.SlingBot.Demo;

public class GithubProcessor : SlingBotWebhookEventProcessor
{
    public GithubProcessor(IHostEnvironment environment, IServiceProvider serviceProvider) : base(environment, serviceProvider) { }

    protected override async Task ProcessIssuesWebhookAsync(WebhookHeaders headers, IssuesEvent issuesEvent, IssuesAction action)
    {
        await base.ProcessIssuesWebhookAsync(headers, issuesEvent, action);
    }
}
