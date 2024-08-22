using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using MintPlayer.SlingBot.Abstractions;
using MintPlayer.SlingBot.Options;
using MintPlayer.SlingBot.Services;
using Octokit.Webhooks;

namespace MintPlayer.SlingBot;

public abstract class SlingBotWebhookEventProcessor : Octokit.Webhooks.WebhookEventProcessor
{
    private readonly IHostEnvironment environment;
    private readonly IServiceProvider serviceProvider;
    private readonly ISignatureService signatureService;
    private readonly IOptions<BotOptions> botOptions;
    public SlingBotWebhookEventProcessor(IHostEnvironment environment, IServiceProvider serviceProvider, ISignatureService signatureService, IOptions<BotOptions> botOptions)
    {
        this.environment = environment;
        this.serviceProvider = serviceProvider;
        this.signatureService = signatureService;
        this.botOptions = botOptions;
    }

    public abstract Task<IEnumerable<SocketClient>> GetDevSocketsForWebhook(WebhookEvent webhook, IEnumerable<SocketClient> connectedClients);

    // The processor should be registered as a scoped service
    // When the signature can be read from the headers,
    // We can store whether it's correct.
    private bool verifiedSignature = false;

    public override async Task ProcessWebhookAsync(IDictionary<string, StringValues> headers, string body)
    {
        // This base method is using a case-sensitive Dictionary.
        // This means that headers can't be found in most situations.
        // We override the method, and create a case-insensitive Dictionary instead.
        var caseInsensitiveHeaders = new Dictionary<string, StringValues>(headers, StringComparer.OrdinalIgnoreCase);

        if (!string.IsNullOrEmpty(botOptions.Value.WebhookSecret))
        {
            // This is the perfect place to verify the signature against the secret
            caseInsensitiveHeaders.TryGetValue("X-Hub-Signature-256", out var signatureSha256);
            if (!signatureService.VerifySignature(signatureSha256, botOptions.Value.WebhookSecret, body))
            {
                return;
            }
            else
            {
                verifiedSignature = true;
            }
        }

        if (environment.IsProduction())
        {
            using var scope = serviceProvider.CreateScope();
            var devSocketService = scope.ServiceProvider.GetRequiredService<IDevSocketService>();
            await devSocketService.SendToClients(caseInsensitiveHeaders, body);
        }
        await base.ProcessWebhookAsync(caseInsensitiveHeaders, body);
    }
}
