using MintPlayer.Octokit.Extensions.Requests;
using Octokit;

namespace MintPlayer.SlingBot.Abstractions;

public interface IAuthenticatedGithubService
{
    Task<IGitHubClient> GetAppClient();
    Task<IGitHubClient> GetInstallationClient(long installationId);
    Task<IGitHubClient> GetInstallationClient(long installationId, CreateInstallationTokenPayload reposAndPermissions);
}
