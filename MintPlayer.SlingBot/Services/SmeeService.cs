using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MintPlayer.SlingBot.Extensions;
using MintPlayer.SlingBot.Options;
using Octokit.Webhooks;
using Smee.IO.Client;
using System.Threading;

namespace MintPlayer.SlingBot.Services;

internal class SmeeService : BackgroundService
{
    private readonly IOptions<BotOptions> botOptions;
    private readonly IServiceProvider services;
    private ISmeeClient? smeeClient;
    public SmeeService(IOptions<BotOptions> botOptions, IServiceProvider services)
    {
        this.botOptions = botOptions;
        this.services = services;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrEmpty(botOptions.Value.DevSmeeChannelUrl)) return;

        smeeClient = new SmeeClient(new Uri(botOptions.Value.DevSmeeChannelUrl));
        smeeClient.OnMessage += SmeeClient_OnMessage;

        // StartAsync is actually a blocking call
        await smeeClient.StartAsync(stoppingToken);

        smeeClient.Stop();
        smeeClient.OnMessage -= SmeeClient_OnMessage;
    }

    private async void SmeeClient_OnMessage(object? sender, Smee.IO.Client.Dto.SmeeEvent e)
    {
        if (e.Event == SmeeEventType.Message)
        {
            var jsonFormatted = e.Data.GetFormattedJson();
            using var scope = services.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<WebhookEventProcessor>();
            await processor.ProcessWebhookAsync(
                e.Data.Headers.ToDictionary(h => h.Key, h => new Microsoft.Extensions.Primitives.StringValues(h.Value)),
                jsonFormatted
            );
        }
    }
}
