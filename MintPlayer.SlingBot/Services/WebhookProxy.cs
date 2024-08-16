using Microsoft.Extensions.Hosting;
using Octokit.Webhooks.Models;
using Octokit.Webhooks;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.SlingBot.Extensions;
using Smee.IO.Client;

namespace MintPlayer.SlingBot.Services;

internal class WebhookProxy : IHostedService
{
    private readonly IConfiguration configuration;
    private readonly IServiceProvider serviceProvider;
    private readonly IServiceProvider services;
    private ISmeeClient? smeeClient;
    public WebhookProxy(IConfiguration configuration, IServiceProvider serviceProvider, IServiceProvider services)
    {
        this.configuration = configuration;
        this.serviceProvider = serviceProvider;
        this.services = services;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var username = configuration["WebhookProxy:Username"];
        var password = configuration["WebhookProxy:Password"];
        var webhookProxyUrl = configuration["WebhookProxy:ProductionWebsocketUrl"];

        if (!string.IsNullOrEmpty(webhookProxyUrl))
        {
            var ws = new ClientWebSocket();
            ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(1);
            ws.Options.AddSubProtocol("ws");
            ws.Options.AddSubProtocol("wss");
            await ws.ConnectAsync(new Uri(webhookProxyUrl), cancellationToken);

            await Task.Run(async () =>
            {
                var handshake = new Handshake
                {
                    Username = username,
                    Password = password,
                };
                await ws.WriteObject(handshake);

                var buffer = new byte[512];
                while (true)
                {
                    var message = await ws.ReadMessage();

                    var split = message.Split("\r\n\r\n");
                    var headers = split[0].Split("\r\n")
                        .Select(h => h.Split(':'))
                        .ToDictionary(h => h[0].Trim(), h => new Microsoft.Extensions.Primitives.StringValues(h[1].Trim()));
                    var body = split[1];


                    using var scope = services.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<WebhookEventProcessor>();

                    await processor.ProcessWebhookAsync(
                        headers,
                        body
                    );

                    if (cancellationToken.IsCancellationRequested) break;
                }
            });
        }

        var smeeChannelUrl = configuration["SmeeChannel:Url"];
        if (!string.IsNullOrEmpty(smeeChannelUrl))
        {
            smeeClient = new SmeeClient(new Uri(smeeChannelUrl));
            smeeClient.OnMessage += SmeeClient_OnMessage;

            // StartAsync is actually a blocking call
            smeeClient.StartAsync(cancellationToken).ContinueWith(delegate { }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    private async void SmeeClient_OnMessage(object? sender, Smee.IO.Client.Dto.SmeeEvent e)
    {
        if (e.Event == SmeeEventType.Message)
        {
            var jsonFormatted = e.Data.GetFormattedJson();
            using (var scope = serviceProvider.CreateScope())
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

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (smeeClient != null)
        {
            smeeClient.Stop();
            smeeClient.OnMessage -= SmeeClient_OnMessage;
        }

        return Task.CompletedTask;
    }
}
