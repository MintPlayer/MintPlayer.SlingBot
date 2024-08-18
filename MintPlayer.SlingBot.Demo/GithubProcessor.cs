using Microsoft.Extensions.Primitives;
using Octokit.Webhooks;

namespace MintPlayer.SlingBot.Demo;

public class GithubProcessor : SlingBotWebhookEventProcessor
{
    public GithubProcessor(IHostEnvironment environment, IServiceProvider serviceProvider) : base(environment, serviceProvider) { }
}
