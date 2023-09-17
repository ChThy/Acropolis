using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Infrastructure.Dapr.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<DaprSettings>(configuration, DaprSettings.Name);
        services.AddDaprClient();
        return services;
    }
}
