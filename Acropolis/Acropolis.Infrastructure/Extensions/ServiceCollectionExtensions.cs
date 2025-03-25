using Acropolis.Infrastructure.FileStorages;
using Acropolis.Infrastructure.Options;
using Acropolis.Infrastructure.Sieve;
using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

namespace Acropolis.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<FileStorageOptions>(configuration, FileStorageOptions.Name);
        services.AddScoped<IFileStorage, FileStorage>();

        services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
        services.AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>();
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
        
        return services;
    }
}