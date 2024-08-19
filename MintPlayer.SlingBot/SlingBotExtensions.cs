using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MintPlayer.SlingBot.Abstractions;
using MintPlayer.SlingBot.Options;
using MintPlayer.SlingBot.Services;
using MintPlayer.SlingBot.Extensions;
using Octokit.Webhooks;
using Octokit.Webhooks.AspNetCore;
using System.Net.WebSockets;

namespace MintPlayer.SlingBot;

public static class SlingBotExtensions
{
    public static IServiceCollection AddSlingBot<TWebhookEventProcessor>(this IServiceCollection services, IWebHostEnvironment environment)
        where TWebhookEventProcessor : SlingBotWebhookEventProcessor
    {
        services.AddScoped<WebhookEventProcessor, TWebhookEventProcessor>();

        if (environment.IsProduction())
            services.AddSingleton<IDevSocketService, DevSocketService>();

        if (environment.IsDevelopment())
            services.AddHostedService<WebhookProxy>();

        return services;
    }

    public static IEndpointRouteBuilder MapSlingBot(this WebApplication app, string path = "/api/github/webhooks")
    {
        var options = app.Services.GetRequiredService<IOptions<BotOptions>>();

        if (app.Environment.IsProduction())
        {
            app.UseWebSockets();
        }

        app.MapGitHubWebhooks(secret: options.Value.WebhookSecret ?? string.Empty);

        if (app.Environment.IsProduction())
        {
            app.MapGet("Blah", () =>
            {
                var proxyUser = app.Configuration["WebhookProxy:Username"];
                var proxyPassword = app.Configuration["WebhookProxy:Password"];

                return $"{proxyUser} {proxyPassword}";
            });

            app.Map("/ws", async (context) =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }

                var proxyUser = app.Configuration["WebhookProxy:Username"];
                var proxyPassword = app.Configuration["WebhookProxy:Password"];

                var ws = await context.WebSockets.AcceptWebSocketAsync(new WebSocketAcceptContext
                {
                    SubProtocol = "wss",
                    KeepAliveInterval = TimeSpan.FromSeconds(10)
                });

                //Receive handshake
                var handshake = await ws.ReadObject<Handshake>();
                if (handshake == null || handshake.Username != proxyUser || handshake.Password != proxyPassword)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Wrong credentials", CancellationToken.None);
                    return;
                }

                while (true)
                {
                    var svc = app.Services.GetRequiredService<IDevSocketService>();
                    var message = await svc.GetMessage();
                    await ws.WriteObject(message);
                    await Task.Delay(5000);
                }

                //var socketService = app.Services.GetRequiredService<IDevSocketService>();
                //await socketService.NewSocketClient(new SocketClient(ws));
            });
        }

        return app;
    }
}
