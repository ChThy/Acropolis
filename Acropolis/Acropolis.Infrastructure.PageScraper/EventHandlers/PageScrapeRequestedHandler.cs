using Acropolis.Application.Events.PageScraper;
using Acropolis.Infrastructure.FileStorages;
using Acropolis.Infrastructure.PageScraper.Options;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acropolis.Infrastructure.PageScraper.EventHandlers;

public class PageScrapeRequestedHandler(
    Browser browser,
    IOptionsMonitor<ScrapeOptions> optionsMonitor,
    IFileStorage fileStorage,
    TimeProvider timeProvider,
    ILogger<PageScrapeRequestedHandler> logger) : IConsumer<PageScrapeRequested>
{
    private readonly Browser browser = browser;
    private readonly IFileStorage fileStorage = fileStorage;
    private readonly ILogger<PageScrapeRequestedHandler> logger = logger;
    private readonly ScrapeOptions options = optionsMonitor.CurrentValue;
    private readonly TimeProvider timeProvider = timeProvider;

    public async Task Consume(ConsumeContext<PageScrapeRequested> context)
    {
        if (!Uri.TryCreate(context.Message.Url, UriKind.RelativeOrAbsolute, out var uri))
        {
            logger.LogError("Invalid url: {url}", context.Message.Url);
            await context.Publish(new PageScrapeFailed(context.Message.Url, timeProvider.GetUtcNow(), $"Invalid url: {context.Message.Url}"));
            return;
        }

        if (options.IgnoredDomains.Contains(uri.Host))
        {
            logger.LogWarning("Ignoring domain {domain}", uri.Host);
            await context.Publish(new PageScrapeSkipped(context.Message.Url, $"Invalid url: {context.Message.Url}"));
            return;
        }

        try
        {
            var page = await browser.GetImage(uri.ToString());
            var location = await fileStorage.StoreFile(ConstructFileName(page.Domain, timeProvider.GetUtcNow(), page.DocumentName), page.Document);

            await context.Publish(new PageScraped(uri.ToString(), timeProvider.GetUtcNow(), page.PageTitle, page.Domain, location));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to scrape page: {url}", uri.ToString());
            await context.Publish(new PageScrapeFailed(uri.ToString(), timeProvider.GetUtcNow(), ex.Message));
        }
    }

    private static string ConstructFileName(string domain, DateTimeOffset uploaded, string filename)
    {
        var timestamp = uploaded.ToString("yyyyMMdd");
        return Path.Join(domain, $"{timestamp}_{filename}");
    }
}