using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration, string name)
        where TOptions : class
    {
        services.Configure<TOptions>(configuration.GetSection(name));
        return services;
    }
}
