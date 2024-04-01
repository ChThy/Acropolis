using Acropolis.Application.PageScraper;
using Acropolis.Application.YoutubeDownloader;
using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddMediatR(config =>
        //{
        //    config.RegisterServicesFromAssemblies(typeof(DownloadYoutubeVideoHandler).Assembly);
        //});



        services.RegisterOptions<YoutubeSettings>(configuration, YoutubeSettings.Name);
        services.AddHttpClient<IYoutubeService, YoutubeService>((sp, client) =>
        {
            var options = sp.GetOptions<YoutubeSettings>();
            client.BaseAddress = new Uri(options.YoutubeDownloaderEndpoint);
        });

        services.RegisterOptions<ScrapeSettings>(configuration, ScrapeSettings.Name);
        services.AddHttpClient<IScrapeService, ScrapeService>((sp, client) =>
        {
            var options = sp.GetOptions<ScrapeSettings>();
            client.BaseAddress = new Uri(options.ScraperEndpoint);
        });

        return services;
    }
}
