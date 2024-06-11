using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Acropolis.Infrastructure.PageScraper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPageScraper(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LaunchOptions>(o =>
        {
            o.Headless = true;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) o.ExecutablePath = "google-chrome-stable";
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
        

        return services;
    }
}