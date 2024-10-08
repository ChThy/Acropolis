using System.Runtime.InteropServices;
using Acropolis.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace Acropolis.Infrastructure.PageScraper;

public class Browser(
    InstalledBrowsers installedBrowsers,
    IOptions<LaunchOptions> launchOptions,
    IOptions<PdfOptions> pdfOptions,
    IOptions<ScreenshotOptions> screenshotOptions,
    ILogger<Browser> logger)
{
    private readonly InstalledBrowsers installedBrowsers = installedBrowsers;
    private readonly LaunchOptions launchOptions = launchOptions.Value;
    private readonly ILogger<Browser> logger = logger;
    private readonly PdfOptions pdfOptions = pdfOptions.Value;
    private readonly ScreenshotOptions screenshotOptions = screenshotOptions.Value;

    private Task<IBrowser> GetBrowser()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            launchOptions.ExecutablePath = "google-chrome-stable";
            logger.LogInformation("Using google-chrome-stable browser at");
        }
        else
        {
            var executablePath = installedBrowsers.GetExecutablePaths().FirstOrDefault();
            if (executablePath is null)
            {
                throw new InvalidOperationException("No browser installed");
            }

            logger.LogInformation("Using browser at: {path}", executablePath);
            launchOptions.ExecutablePath = executablePath;
        }

        return Puppeteer.LaunchAsync(launchOptions);
    }

    public async Task<ScrapeResponse> GetPdf(string url)
    {
        await using var browser = await GetBrowser();
        logger.LogDebug("Browser started");

        await using var page = await browser.NewPageAsync();
        await SetPageOptions(page);
        await page.GoToAsync(url);
        logger.LogDebug("Navigated to {url}", url);

        var pageTitle = await page.GetTitleAsync();

        var result = await page.PdfStreamAsync(pdfOptions);
        logger.LogDebug("PDF stream taken");
        return new ScrapeResponse(pageTitle, new Uri(url).Host, $"{pageTitle.RemoveInvalidFileNameChars()}.pdf", result);
    }

    public async Task<ScrapeResponse> GetImage(string url)
    {
        await using var browser = await GetBrowser();
        logger.LogDebug("Browser started");

        await using var page = await browser.NewPageAsync();
        await SetPageOptions(page);
        await page.GoToAsync(url);
        logger.LogDebug("Navigated to {url}", url);

        var pageTitle = await page.GetTitleAsync();

        var result = await page.ScreenshotStreamAsync(screenshotOptions);
        logger.LogDebug("Screenshot stream taken");
        return new ScrapeResponse(pageTitle, new Uri(url).Host, $"{pageTitle.RemoveInvalidFileNameChars()}.png", result);
    }

    private static Task SetPageOptions(IPage page)
    {
        return page.SetViewportAsync(new ViewPortOptions
        {
            Width = 1920,
            Height = 1080,
            IsLandscape = true
        });
    }
}