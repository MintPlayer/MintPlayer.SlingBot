using Microsoft.Extensions.Options;
using MintPlayer.Octokit.Extensions.Requests;
using MintPlayer.SlingBot.Options;
using Newtonsoft.Json;
using Octokit;
using System.Security.Cryptography;
using System.Text;

namespace MintPlayer.SlingBot.Services;

internal class AuthenticatedGithubService : Abstractions.IAuthenticatedGithubService
{
    #region Constructor
    private readonly IOptions<BotOptions> botOptions;
    public AuthenticatedGithubService(IOptions<BotOptions> botOptions)
    {
        this.botOptions = botOptions;
    }
    #endregion

    private readonly ProductHeaderValue header = new("Test", "0.0.1");
    
    public Task<IGitHubClient> GetAppClient()
    {
        var privateKey = botOptions.Value.PrivateKey;
        if (string.IsNullOrEmpty(privateKey))
        {
            privateKey = ReadPrivateKey(botOptions.Value.PrivateKeyPath!);
        }

        var jwt = GetJwt(botOptions.Value.ClientId!, privateKey);
        var ghclient = new GitHubClient(header)
        {
            Credentials = new Credentials(jwt, AuthenticationType.Bearer)
        };
        return Task.FromResult<IGitHubClient>(ghclient);
    }

    public async Task<IGitHubClient> GetInstallationClient(long installationId)
    {
        var appClient = await GetAppClient();

        var response = await appClient.GitHubApps.CreateInstallationToken(installationId);
        var repoClient = new GitHubClient(header)
        {
            Credentials = new Credentials(response.Token)
        };
        return repoClient;
    }

    public async Task<IGitHubClient> GetInstallationClient(long installationId, CreateInstallationTokenPayload reposAndPermissions)
    {
        var appClient = await GetAppClient();

        var response = await appClient.GitHubApps.CreateInstallationToken(installationId, reposAndPermissions);
        var repoClient = new GitHubClient(header)
        {
            Credentials = new Credentials(response.Token)
        };
        return repoClient;
    }

    private string ReadPrivateKey(string privateKeyPath)
    {
        var absolutePemPath = Path.Combine(Directory.GetCurrentDirectory(), privateKeyPath);
        var privateKey = File.ReadAllText(absolutePemPath);
        return privateKey;
    }

    private string GetJwt(string clientId, string privateKey)
    {
        var header = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
        {
            alg = "RS256",
            typ = "JWT"
        }))).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
        {
            iat = DateTimeOffset.UtcNow.AddSeconds(-1).ToUnixTimeSeconds(),
            exp = DateTimeOffset.UtcNow.AddMinutes(9).ToUnixTimeSeconds(),
            //iss = appId,
            iss = clientId
        })));

        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);

        var signature = Convert.ToBase64String(rsa.SignData(Encoding.UTF8.GetBytes($"{header}.{payload}"), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var jwt = $"{header}.{payload}.{signature}";
        return jwt;
    }
}
