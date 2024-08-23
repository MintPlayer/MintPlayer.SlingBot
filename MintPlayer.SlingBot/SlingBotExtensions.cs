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
using Microsoft.Extensions.Configuration;

namespace MintPlayer.SlingBot;

public static class SlingBotExtensions
{
    public static IServiceCollection AddSlingBot<TWebhookEventProcessor>(this IServiceCollection services, IWebHostEnvironment environment)
        where TWebhookEventProcessor : SlingBotWebhookEventProcessor
    {
        services.AddScoped<WebhookEventProcessor, TWebhookEventProcessor>();

        if (environment.IsProduction())
        {
            services.AddSingleton<IDevSocketService, DevSocketService>();
            services.AddScoped<SlingBotWebhookEventProcessor, TWebhookEventProcessor>();
        }

        if (environment.IsDevelopment())
            services.AddHostedService<WebhookProxy>();

        return services
            .AddScoped<Abstractions.IAuthenticatedGithubService, AuthenticatedGithubService>()
            .AddScoped<Abstractions.ISignatureService, SignatureService>();
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
            app.Map("/ws", async (context) =>
            {
                try
                {
                    if (!context.WebSockets.IsWebSocketRequest)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }

                    var proxyAllowedUsers = app.Configuration.GetValue<string[]>("WebhookProxy:AllowedUsers") ?? [];

                    var ws = await context.WebSockets.AcceptWebSocketAsync(new WebSocketAcceptContext
                    {
                        SubProtocol = "wss",
                        KeepAliveInterval = TimeSpan.FromMinutes(5)
                    });

                    //Receive handshake
                    var handshake = await ws.ReadObject<Handshake>();
                    if (handshake == null)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Wrong credentials", CancellationToken.None);
                        return;
                    }

                    var githubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("SlingBot"));
                    githubClient.Credentials = new Octokit.Credentials(handshake.GithubToken);
                    var githubUser = await githubClient.User.Current();

                    if (!proxyAllowedUsers.Contains(githubUser.Login))
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Unauthorized", CancellationToken.None);
                        return;
                    }

                    var socketService = app.Services.GetRequiredService<IDevSocketService>();
                    await socketService.NewSocketClient(new SocketClient(ws, githubUser.Login));
                }
                catch (Octokit.AuthorizationException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            });
        }

        return app;
    }
}
