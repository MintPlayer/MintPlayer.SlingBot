using Microsoft.Extensions.DependencyInjection;
using MintPlayer.DiffParser.Abstractions;

namespace MintPlayer.DiffParser.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiffParser(this IServiceCollection services)
    {
        return services.AddTransient<IDiffParser, DiffParser>();
    }
}
