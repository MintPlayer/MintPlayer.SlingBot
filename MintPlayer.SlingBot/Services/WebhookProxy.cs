using Microsoft.Extensions.Hosting;
using Octokit.Webhooks.Models;
using Octokit.Webhooks;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.SlingBot.Extensions;

namespace MintPlayer.SlingBot.Services;

internal class WebhookProxy : IHostedService
{
    private readonly IConfiguration configuration;
    private readonly IServiceProvider services;
    public WebhookProxy(IConfiguration configuration, IServiceProvider services)
    {
        this.configuration = configuration;
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
            ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(900);
            ws.Options.AddSubProtocol("ws");
            ws.Options.AddSubProtocol("wss");
            var baseUri = new Uri(webhookProxyUrl);
            await ws.ConnectAsync(new Uri(baseUri, "ws"), cancellationToken);

            await Task.Run(async () =>
            {
                var handshake = new Handshake
                {
                    Username = username,
                    Password = password,
                };
                await ws.WriteObject(handshake);

                while (true)
                {
                    var data = await ws.ReadObject<Handshake>();

                    //var message = await ws.ReadMessage();

                    //var split = message.Split("\r\n\r\n");
                    //var headers = split[0].Split("\r\n")
                    //    .Select(h => h.Split(':'))
                    //    .ToDictionary(h => h[0].Trim(), h => new Microsoft.Extensions.Primitives.StringValues(h[1].Trim()));
                    //var body = split[1];


                    //using var scope = services.CreateScope();
                    //var processor = scope.ServiceProvider.GetRequiredService<WebhookEventProcessor>();

                    //await processor.ProcessWebhookAsync(
                    //    headers,
                    //    body
                    //);

                    if (cancellationToken.IsCancellationRequested) break;
                }
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
