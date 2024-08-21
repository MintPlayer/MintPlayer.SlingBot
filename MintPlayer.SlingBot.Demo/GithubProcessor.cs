using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.Issues;

namespace MintPlayer.SlingBot.Demo;

public class GithubProcessor : SlingBotWebhookEventProcessor
{
    public GithubProcessor(IHostEnvironment environment, IServiceProvider serviceProvider) : base(environment, serviceProvider) { }

    public override async Task<IEnumerable<SocketClient>> GetDevSocketsForWebhook(WebhookEvent webhook, IEnumerable<SocketClient> connectedClients)
    {
        await Task.Delay(1);
        return connectedClients.Where(c => c.Email == webhook.Sender?.Email);
    }

    protected override async Task ProcessIssuesWebhookAsync(WebhookHeaders headers, IssuesEvent issuesEvent, IssuesAction action)
    {
        await base.ProcessIssuesWebhookAsync(headers, issuesEvent, action);
    }
}
