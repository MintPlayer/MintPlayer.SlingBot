using MintPlayer.Octokit.Extensions.Requests;
using System.Net;
using System.Xml.Linq;

namespace Octokit;

public static class CreateInstallationTokenExtension
{
    public static async Task<AccessToken> CreateInstallationToken(this IGitHubAppsClient client, long installationId, CreateInstallationTokenPayload reposAndPermissions)
    {
        var endpoint = ApiUrls.AccessTokens(installationId);
        var connection = typeof(ApiClient).GetProperty("ApiConnection", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(client) as ApiConnection;
        if (connection == null) throw new InvalidOperationException();

        var response = await connection.Connection.Post<AccessToken>(endpoint, reposAndPermissions, null, "application/json").ConfigureAwait(false);
        if (response.HttpResponse.StatusCode != HttpStatusCode.Created)
        {
            throw new ApiException("Invalid Status Code returned. Expected a 201", response.HttpResponse.StatusCode);
        }

        return response.Body;
    }
}
