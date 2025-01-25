using MintPlayer.Octokit.Extensions.Enums;
using Octokit;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Requests;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class CreateInstallationTokenPayload : RequestParameters
{
    public CreateInstallationTokenPayload(string[] repositories, Dictionary<EGithubPermission, EPermission> permissions)
    {
        Repositories = repositories;
        Permissions = permissions.AsReadOnly();
    }
    public CreateInstallationTokenPayload(long[] repositoryIds, Dictionary<EGithubPermission, EPermission> permissions)
    {
        this.RepositoryIds = repositoryIds;
        Permissions = permissions.AsReadOnly();
    }

    [Parameter(Key = "repositories")]
    public string[]? Repositories { get; private set; }

    [Parameter(Key = "repository_ids")]
    public long[]? RepositoryIds { get; private set; }

    [Parameter(Key = "permissions")]
    public ReadOnlyDictionary<EGithubPermission, EPermission> Permissions { get; private set; }
}
