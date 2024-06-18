using System.Runtime.InteropServices;
using Acropolis.Infrastructure.PageScraper.BackgroundWorkers;
using Acropolis.Infrastructure.PageScraper.Options;
using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Acropolis.Infrastructure.PageScraper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPageScraper(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ScrapeOptions>(configuration, ScrapeOptions.Name);
        services.AddSingleton<InstalledBrowsers>();
        services.AddHostedService<BrowserDownloader>();

        services.Configure<LaunchOptions>(o =>
        {
            o.Headless = true;
        });
        services.Configure<PdfOptions>(o =>
        {
            o.Format = PaperFormat.A4;
            o.MarginOptions = new MarginOptions { Top = "20px", Right = "20px", Bottom = "20px", Left = "20px" };
        });
        services.Configure<ScreenshotOptions>(o =>
        {
            o.FullPage = true;
        });

        services.AddScoped<Browser>();

        return services;
    }
}