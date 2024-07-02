using Acropolis.Api.Extensions;
using Acropolis.Api.HostedServices;
using Acropolis.Api.MigrateOldData;
using Acropolis.Api.Models;
using Acropolis.Application.Events;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using MassTransit.Internals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YoutubeExplode;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddServices(builder.Configuration);

        builder.Services.AddYoutubeDownloaderDataMigration(builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPost("download", async (
            [FromServices] IBus bus,
            [FromBody] DownloadVideoRequest request,
            CancellationToken cancellationToken) =>
        {
            await bus.Publish(new UrlRequestReceived(Guid.NewGuid(), request.Url, DateTimeOffset.UtcNow),
                ctx => ctx.CorrelationId = NewId.NextGuid(), cancellationToken);
            return Results.Accepted();
        });

        app.MapPost("migrate", async (
            int take,
            int skip,
            ScraperDbContext scraperDbContext,
            AppDbContext dbContext,
            IBus bus,
            
            ILogger<ScrapeRequest> logger) =>
        {
            List<string> notFound = [];

            var scrapedPages = await scraperDbContext
                .Set<ScrapedPage>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            foreach (var page in scrapedPages.Where(e => e.Content.Count > 1))
            {
                logger.LogWarning("More than 1 content for: {url}", page.Request.Url);
            }
            
            foreach (var page in scrapedPages.Where(e => e.PageData is not null && e.Content.Count == 1))
            {
                var content = page.Content.Single();
                if (!File.Exists(Path.Combine("data", "downloads", "scraper", content.FileName)))
                {
                    // Download file
                    
                }
                else
                {
                    var pageState = new ScrapePageState()
                    {
                        CorrelationId = page.Id,
                        CurrentState = "Downloaded",
                        RequestedTimestamp =  page.Request!.Timestamp,
                        Url = page.Request.Url,
                        Domain = page.PageData!.Domain,
                        Title = page.PageData.Title,
                        ScrapedTimestamp = content.ScrapedOn,
                        StorageLocation = Path.Combine("scraper", content.FileName)
                    };
                    if (!(await dbContext.Set<ScrapePageState>().AnyAsync(e => e.Url == pageState.Url)))
                    {
                        dbContext.Add(pageState);
                    }
                    else
                    {
                        logger.LogInformation("Duplicate found: {duplicate}", pageState.Url);
                    }
                }
            }

            await dbContext.SaveChangesAsync();

            foreach (var page in scrapedPages.Where(e => e.PageData is null))
            {
                await bus.Publish(new UrlRequestReceived(page.Id, page.Request.Url, DateTimeOffset.UtcNow));
            }

            return Results.Ok(notFound);
        });

        app.Run();
    }
}