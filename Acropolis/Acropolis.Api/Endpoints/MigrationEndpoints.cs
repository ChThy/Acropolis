using Acropolis.Application.Events.PageScraper;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.Endpoints;

public static class MigrationEndpoints
{
    public static IEndpointRouteBuilder MapMigrationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("migrate").WithTags("Migrate");

        group.MapPost("movedb", async (
            [FromServices] AppDbContext dbContext,
            [FromServices] IConfiguration configuration) =>
        {
            await using var sqliteDbContext = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>().UseSqlite(configuration.GetConnectionString("SqliteDatabase")).Options);

            await dbContext.DownloadedVideos.AddRangeAsync(await sqliteDbContext.DownloadedVideos
                .Include(e => e.MetaData)
                .Include(e => e.Resources)
                .ToListAsync()
            );

            await dbContext.Set<DownloadVideoState>().AddRangeAsync(await sqliteDbContext.Set<DownloadVideoState>()
                .Include(e => e.StoredVideo)
                .ThenInclude(e => e.MetaData)
                .ToListAsync()
            );

            await dbContext.ScrapedPages.AddRangeAsync(await sqliteDbContext.ScrapedPages
                .Include(e => e.MetaData)
                .Include(e => e.Resources)
                .ToListAsync()
            );
            
            await dbContext.Set<ScrapePageState>().AddRangeAsync(await sqliteDbContext.Set<ScrapePageState>()
                .ToListAsync()
            );

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

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

        group.MapDelete("skippedpages", async (
            [FromServices] AppDbContext dbContext) =>
        {
            var pages = await dbContext.Set<ScrapePageState>()
                .Where(e => e.CurrentState == nameof(ScrapePageSaga.ScrapeSkipped))
                .ToListAsync();

            dbContext.RemoveRange(pages);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("scrapedpages", async (
            [FromServices] AppDbContext dbContext) =>
        {
            var pages = await dbContext.Set<ScrapePageState>()
                .Where(e => e.CurrentState == nameof(ScrapePageSaga.Scraped))
                .ToListAsync();

            dbContext.RemoveRange(pages);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
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
                video.DownloadedTimestamp!.Value,
                video.StoredVideo!.MetaData,
                video.StoredVideo.StorageLocation));

            await bus.PublishBatch(messages, cancellationToken);
            return Results.Accepted();
        });

        group.MapDelete("skippedvideos", async (
            [FromServices] AppDbContext dbContext) =>
        {
            var pages = await dbContext.Set<DownloadVideoState>()
                .Where(e => e.CurrentState == nameof(DownloadVideoSaga.DownloadSkipped))
                .ToListAsync();

            dbContext.RemoveRange(pages);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("downloadedvideos", async (
            [FromServices] AppDbContext dbContext) =>
        {
            var pages = await dbContext.Set<DownloadVideoState>()
                .Where(e => e.CurrentState == nameof(DownloadVideoSaga.Downloaded))
                .ToListAsync();

            dbContext.RemoveRange(pages);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapGet("invalid-characters", () =>
        {
            return Results.Ok(new
            {
                PathHelpers.InvalidPathCharacters,
                PathHelpers.InvalidFileNameCharacters
            });
        });

        return endpoints;
    }
}