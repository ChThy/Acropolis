using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace Acropolis.Infrastructure.PageScraper.BackgroundWorkers;

public class BrowserDownloader(InstalledBrowsers installedBrowsers, ILogger<BrowserDownloader> logger) : BackgroundService
{
    private readonly InstalledBrowsers installedBrowsers = installedBrowsers;
    private readonly ILogger<BrowserDownloader> logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        // {
        //     logger.LogInformation("Running on Linux and assuming stable Chrome is already installed.");
        //     return;
        // }
        
        var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
        {
            Path = "browser",
            Platform = GetPlatform()
        });

        await browserFetcher.DownloadAsync();
        
        logger.LogInformation("Browser downloaded");
    }

    private Platform? GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Platform.Win64;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Platform.Linux;
        }

        logger.LogWarning("Could not resolve platform! Defaulting to unknown platform.");
        return Platform.Unknown;
    }
}