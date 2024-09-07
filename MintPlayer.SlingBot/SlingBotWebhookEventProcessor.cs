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

    /// <summary>Determines whether a webhook should be downstreamed to a development machine.s</summary>
    public virtual Task<bool> ShouldDownstreamWebhookAsync(WebhookEvent webhook) => Task.FromResult(true);

    public override async Task ProcessWebhookAsync(IDictionary<string, StringValues> headers, string body)
    {
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(body);

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

        // Used the contents of base.ProcessWebhookAsync(IDictionary<string, StringValues>, string) here
        var webhookHeaders = WebhookHeaders.Parse(caseInsensitiveHeaders); // Let's not use the default headers dictionary here
        var webhookEvent = this.DeserializeWebhookEvent(webhookHeaders, body);


        var shouldSendToClients = await ShouldDownstreamWebhookAsync(webhookEvent);
        if (environment.IsProduction() && shouldSendToClients)
        {
            using var scope = serviceProvider.CreateScope();
            var devSocketService = scope.ServiceProvider.GetRequiredService<IDevSocketService>();
            await devSocketService.SendToClients(caseInsensitiveHeaders, body);
        }

        await this.ProcessWebhookAsync(webhookHeaders, webhookEvent);
    }
}
