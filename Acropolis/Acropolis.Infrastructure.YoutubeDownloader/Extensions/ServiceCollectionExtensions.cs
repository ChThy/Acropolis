using Acropolis.Infrastructure.YoutubeDownloader.Options;
using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode;

namespace Acropolis.Infrastructure.YoutubeDownloader.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYoutubeDownloaderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<YoutubeOptions>(configuration, YoutubeOptions.Name);

        services.AddScoped(sp => new YoutubeClient(sp.GetRequiredService<HttpClient>()));

        return services;
    }
}
