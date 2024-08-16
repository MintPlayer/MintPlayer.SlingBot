using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.SlingBot.Extensions;
using Octokit.Webhooks;
using Smee.IO.Client;

namespace MintPlayer.SlingBot.Services;

internal class SmeeService : IHostedService
{
    private readonly IConfiguration configuration;
    private readonly IServiceProvider services;
    private ISmeeClient? smeeClient;
    public SmeeService(IConfiguration configuration, IServiceProvider services)
    {
        this.configuration = configuration;
        this.services = services;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var smeeChannelUrl = configuration["SmeeChannel:Url"];
        if (!string.IsNullOrEmpty(smeeChannelUrl))
        {
            smeeClient = new SmeeClient(new Uri(smeeChannelUrl));
            smeeClient.OnMessage += SmeeClient_OnMessage;

            // StartAsync is actually a blocking call
            await smeeClient.StartAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        var smeeChannelUrl = configuration["SmeeChannel:Url"];
        if (!string.IsNullOrEmpty(smeeChannelUrl) && smeeClient is not null)
        {
            smeeClient.Stop();
            smeeClient.OnMessage -= SmeeClient_OnMessage;
        }
        return Task.CompletedTask;
    }

    private async void SmeeClient_OnMessage(object? sender, Smee.IO.Client.Dto.SmeeEvent e)
    {
        if (e.Event == SmeeEventType.Message)
        {
            var jsonFormatted = e.Data.GetFormattedJson();
            using (var scope = services.CreateScope())
            {
                var processor = scope.ServiceProvider.GetRequiredService<WebhookEventProcessor>();
                //var json = System.Text.Json.JsonSerializer.Deserialize<IssuesEvent>(jsonFormatted);
                await processor.ProcessWebhookAsync(
                    e.Data.Headers.ToDictionary(h => h.Key, h => new Microsoft.Extensions.Primitives.StringValues(h.Value)),
                    jsonFormatted
                );
            }
        }
    }
}
