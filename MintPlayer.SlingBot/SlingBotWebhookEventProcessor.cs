using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using Octokit.Webhooks;

namespace MintPlayer.SlingBot;

public class SlingBotWebhookEventProcessor : Octokit.Webhooks.WebhookEventProcessor
{
    private readonly IHostEnvironment environment;
    private readonly IServiceProvider serviceProvider;
    public SlingBotWebhookEventProcessor(IHostEnvironment environment, IServiceProvider serviceProvider)
    {
        this.environment = environment;
        this.serviceProvider = serviceProvider;
    }

    public override async Task ProcessWebhookAsync(IDictionary<string, StringValues> headers, string body)
    {
        if (environment.IsProduction())
        {
            using var scope = serviceProvider.CreateScope();
            var devSocketService = scope.ServiceProvider.GetRequiredService<IDevSocketService>();
            await devSocketService.SendToClients(headers, body);
        }
        await base.ProcessWebhookAsync(headers, body);
    }
}
