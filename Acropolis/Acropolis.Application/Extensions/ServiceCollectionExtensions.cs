using Acropolis.Application.PageScraper;
using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ScrapeSettings>(configuration, ScrapeSettings.Name);
        
        return services;
    }
}