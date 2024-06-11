using Acropolis.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace Acropolis.Infrastructure.PageScraper.Extensions;

public class Browser(
    IOptions<LaunchOptions> launchOptions,
    IOptions<PdfOptions> pdfOptions,
    IOptions<ScreenshotOptions> screenshotOptions,
    ILogger<Browser> logger)
{
    private readonly LaunchOptions launchOptions = launchOptions.Value;
    private readonly ILogger<Browser> logger = logger;
    private readonly PdfOptions pdfOptions = pdfOptions.Value;
    private readonly ScreenshotOptions screenshotOptions = screenshotOptions.Value;

    private Task<IBrowser> GetBrowser()
    {
        return Puppeteer.LaunchAsync(launchOptions);
    }

    public async Task<(string, Stream)> GetPdf(string url)
    {
        await using var browser = await GetBrowser();
        logger.LogDebug("Browser started");

        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        logger.LogDebug("Navigated to {url}", url);

        var pageTitle = await page.GetTitleAsync();

        var result = await page.PdfStreamAsync(pdfOptions);
        logger.LogDebug("PDF stream taken");
        return (pageTitle.RemoveInvalidFileNameChars(), result);
    }

    public async Task<(string, Stream)> GetImage(string url)
    {
        await using var browser = await GetBrowser();
        logger.LogDebug("Browser started");

        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        logger.LogDebug("Navigated to {url}", url);

        var pageTitle = await page.GetTitleAsync();

        var result = await page.ScreenshotStreamAsync(screenshotOptions);
        logger.LogDebug("Screenshot stream taken");
        return (pageTitle.RemoveInvalidFileNameChars(), result);
    }
}