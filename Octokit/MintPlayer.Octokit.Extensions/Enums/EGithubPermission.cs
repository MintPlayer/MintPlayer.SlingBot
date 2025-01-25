using ParameterAttribute = Octokit.Internal.ParameterAttribute;

namespace MintPlayer.Octokit.Extensions.Enums;

/// <summary>
/// Github permissions
/// </summary>
public enum EGithubPermission
{
    /// <summary>
    /// The level of permission to grant the access token for GitHub Actions workflows, workflow runs, and artifacts.
    /// </summary>
	[Parameter(Key = "actions")]
    Actions,

    /// <summary>
    /// The level of permission to grant the access token for repository creation, deletion, settings, teams, and collaborators creation.
    /// </summary>
    [Parameter(Key = "administration")]
    Administration,

    /// <summary>
    /// The level of permission to grant the access token for checks on code.
    /// </summary>
    [Parameter(Key = "checks")]
    Checks,

    /// <summary>
    /// The level of permission to grant the access token to create, edit, delete, and list Codespaces.
    /// </summary>
    [Parameter(Key = "codespaces")]
    Codespaces,

    /// <summary>
    /// The level of permission to grant the access token for repository contents, commits, branches, downloads, releases, and merges.
    /// </summary>
    [Parameter(Key = "contents")]
    Contents,

    /// <summary>
    /// The leve of permission to grant the access token to manage Dependabot secrets.
    /// </summary>
    [Parameter(Key = "dependabot_secrets")]
    DependabotSecrets,

    /// <summary>
    /// The level of permission to grant the access token for deployments and deployment statuses.
    /// </summary>
    [Parameter(Key = "deployments")]
    Deployments,

    /// <summary>
    /// The level of permission to grant the access token for managing repository environments.
    /// </summary>
    [Parameter(Key = "environments")]
    Environments,

    /// <summary>
    /// The level of permission to grant the access token for issues and related comments, assignees, labels, and milestones.
    /// </summary>
    [Parameter(Key = "issues")]
    Issues,

    /// <summary>
    /// The level of permission to grant the access token to search repositories, list collaborators, and access repository metadata.
    /// </summary>
    [Parameter(Key = "metadata")]
    Metadata,

    /// <summary>
    /// The level of permission to grant the access token for packages published to GitHub Packages.
    /// </summary>
    [Parameter(Key = "packages")]
    Packages,

    /// <summary>
    /// The level of permission to grant the access token to retrieve Pages statuses, configuration, and builds, as well as create new builds.
    /// </summary>
    [Parameter(Key = "pages")]
    Pages,

    /// <summary>
    /// The level of permission to grant the access token for pull requests and related comments, assignees, labels, milestones, and merges.
    /// </summary>
    [Parameter(Key = "pull_requests")]
    PullRequests,

    /// <summary>
    /// The level of permission to grant the access token to view and edit custom properties for a repository, when allowed by the property.
    /// </summary>
    [Parameter(Key = "repository_custom_properties")]
    RepositoryCustomProperties,

    /// <summary>
    /// The level of permission to grant the access token to manage the post-receive hooks for a repository.
    /// </summary>
    [Parameter(Key = "repository_hooks")]
    RepositoryHooks,

    /// <summary>
    /// The level of permission to grant the access token to manage repository projects, columns, and cards.
    /// </summary>
    [Parameter(Key = "repository_projects")]
    RepositoryProjects,

    /// <summary>
    /// The level of permission to grant the access token to view and manage secret scanning alerts.
    /// </summary>
    [Parameter(Key = "secret_scanning_alerts")]
    SecretScanningAlerts,

    /// <summary>
    /// The level of permission to grant the access token to manage repository secrets.
    /// </summary>
    [Parameter(Key = "secrets")]
    Secrets,

    /// <summary>
    /// The level of permission to grant the access token to view and manage security events like code scanning alerts.
    /// </summary>
    [Parameter(Key = "security_events")]
    SecurityEvents,

    /// <summary>
    /// The level of permission to grant the access token to manage just a single file.
    /// </summary>
    [Parameter(Key = "single_file")]
    SingleFile,

    /// <summary>
    /// The level of permission to grant the access token for commit statuses.
    /// </summary>
    [Parameter(Key = "statuses")]
    Statuses,

    /// <summary>
    /// The level of permission to grant the access token to manage Dependabot alerts.
    /// </summary>
    [Parameter(Key = "vulnerability_alerts")]
    VulnerabilityAlerts,

    /// <summary>
    /// The level of permission to grant the access token to update GitHub Actions workflow files.
    /// </summary>
    [Parameter(Key = "workflows")]
    Workflows,

    /// <summary>
    /// The level of permission to grant the access token for organization teams and members.
    /// </summary>
    [Parameter(Key = "members")]
    Members,

    /// <summary>
    /// The level of permission to grant the access token to manage access to an organization.
    /// </summary>
    [Parameter(Key = "organization_administration")]
    OrganizationAdministration,

    /// <summary>
    /// The level of permission to grant the access token for custom repository roles management.
    /// </summary>
    [Parameter(Key = "organization_custom_roles")]
    OrganizationCustomRoles,

    /// <summary>
    /// The level of permission to grant the access token for custom organization roles management.
    /// </summary>
    [Parameter(Key = "organization_custom_org_roles")]
    OrganizationCustomOrgRoles,

    /// <summary>
    /// The level of permission to grant the access token for custom property management.
    /// </summary>
    [Parameter(Key = "organization_custom_properties")]
    OrganizationCustomProperties,

    /// <summary>
    /// The level of permission to grant the access token for managing access to GitHub Copilot for members of an organization with a Copilot Business subscription. This property is in public preview and is subject to change.
    /// </summary>
    [Parameter(Key = "organization_copilot_seat_management")]
    OrganizationCopilotSeatManagement,

    /// <summary>
    /// The level of permission to grant the access token to view and manage announcement banners for an organization.
    /// </summary>
    [Parameter(Key = "organization_announcement_banners")]
    OrganizationAnnouncementBanners,

    /// <summary>
    /// The level of permission to grant the access token to view events triggered by an activity in an organization.
    /// </summary>
    [Parameter(Key = "organization_events")]
    OrganizationEvents,

    /// <summary>
    /// The level of permission to grant the access token to manage the post-receive hooks for an organization.
    /// </summary>
    [Parameter(Key = "organization_hooks")]
    OrganizationHooks,

    /// <summary>
    /// The level of permission to grant the access token for viewing and managing fine-grained personal access token requests to an organization.
    /// </summary>
    [Parameter(Key = "organization_personal_access_tokens")]
    OrganizationPersonalAccessTokens,

    /// <summary>
    /// The level of permission to grant the access token for viewing and managing fine-grained personal access tokens that have been approved by an organization.
    /// </summary>
    [Parameter(Key = "organization_personal_access_token_requests")]
    OrganizationPersonalAccessTokenRequests,

    /// <summary>
    /// The level of permission to grant the access token for viewing an organization's plan.
    /// </summary>
    [Parameter(Key = "organization_plan")]
    OrganizationPlan,

    /// <summary>
    /// The level of permission to grant the access token to manage organization projects and projects public preview (where available).
    /// </summary>
    [Parameter(Key = "organization_projects")]
    OrganizationProjects,

    /// <summary>
    /// The level of permission to grant the access token for organization packages published to GitHub Packages.
    /// </summary>
    [Parameter(Key = "organization_packages")]
    OrganizationPackages,

    /// <summary>
    /// The level of permission to grant the access token to manage organization secrets.
    /// </summary>
    [Parameter(Key = "organization_secrets")]
    OrganizationSecrets,

    /// <summary>
    /// The level of permission to grant the access token to view and manage GitHub Actions self-hosted runners available to an organization.
    /// </summary>
    [Parameter(Key = "organization_self_hosted_runners")]
    OrganizationSelfHostedRunners,

    /// <summary>
    /// The level of permission to grant the access token to view and manage users blocked by the organization.
    /// </summary>
    [Parameter(Key = "organization_user_blocking")]
    OrganizationUserBlocking,

    /// <summary>
    /// The level of permission to grant the access token to manage team discussions and related comments.
    /// </summary>
    [Parameter(Key = "team_discussions")]
    TeamDiscussions,

    /// <summary>
    /// The level of permission to grant the access token to manage the email addresses belonging to a user.
    /// </summary>
    [Parameter(Key = "email_addresses")]
    EmailAddresses,

    /// <summary>
    /// The level of permission to grant the access token to manage the followers belonging to a user.
    /// </summary>
    [Parameter(Key = "followers")]
    Followers,

    /// <summary>
    /// The level of permission to grant the access token to manage git SSH keys.
    /// </summary>
    [Parameter(Key = "git_ssh_keys")]
    GitSshKeys,

    /// <summary>
    /// The level of permission to grant the access token to view and manage GPG keys belonging to a user.
    /// </summary>
    [Parameter(Key = "gpg_keys")]
    GpgKeys,

    /// <summary>
    /// The level of permission to grant the access token to view and manage interaction limits on a repository.
    /// </summary>
    [Parameter(Key = "interaction_limits")]
    InteractionLimits,

    /// <summary>
    /// The level of permission to grant the access token to manage the profile settings belonging to a user.
    /// </summary>
    [Parameter(Key = "profile")]
    Profile,

    /// <summary>
    /// The level of permission to grant the access token to list and manage repositories a user is starring.
    /// </summary>
    [Parameter(Key = "starring")]
    Starring,
}
