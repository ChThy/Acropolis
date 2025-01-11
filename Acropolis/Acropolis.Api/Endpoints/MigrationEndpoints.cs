using Acropolis.Application.Events;
using Acropolis.Application.Events.PageScraper;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Models;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.Endpoints;

public static class MigrationEndpoints
{
    public static IEndpointRouteBuilder MapMigrationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("migrate").WithTags("Download");

        group.MapPost("pages", async (
            [FromServices] IBus bus,
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var pages = await dbContext.Set<ScrapePageState>()
                .Where(e => e.CurrentState == nameof(ScrapePageSaga.Scraped))
                .ToListAsync(cancellationToken);

            var messages = pages.Select(page => new PageScraped(
                page.Url,
                page.ScrapedTimestamp.Value, 
                page.Title,
                page.Domain,
                page.StorageLocation));

            await bus.PublishBatch(messages, cancellationToken);
            return Results.Accepted();
        });
        
        group.MapPost("videos", async (
            [FromServices] IBus bus,
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var videos = await dbContext.Set<DownloadVideoState>()
                .Where(e => e.CurrentState == nameof(DownloadVideoSaga.Downloaded))
                .ToListAsync(cancellationToken);

            var messages = videos.Select(video => new VideoDownloaded(
                video.Url,
                video.DownloadedTimestamp.Value, 
                video.VideoMetaData));

            await bus.PublishBatch(messages, cancellationToken);
            return Results.Accepted();
        });

        return endpoints;
    }
}