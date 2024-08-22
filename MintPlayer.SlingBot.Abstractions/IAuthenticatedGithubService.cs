using Octokit;

namespace MintPlayer.SlingBot.Abstractions;

public interface IAuthenticatedGithubService
{
    Task<IGitHubClient> GetAuthenticatedGithubClient(long installationId);
}
