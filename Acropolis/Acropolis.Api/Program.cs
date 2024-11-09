using Acropolis.Api.Extensions;
using Acropolis.Api.HostedServices;
using Acropolis.Api.Models;
using Acropolis.Application.Events;
using Acropolis.Application.Events.PageScraper;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        app.MapPost("retry-failed-videos", async (
            [FromServices] IBus bus,
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var failedVideos = await dbContext.Set<DownloadVideoState>()
                .Where(e => e.CurrentState == "DownloadFailed")
                .ToListAsync(cancellationToken);
            
            await bus.PublishBatch(failedVideos.Select(e => new RetryFailedVideoDownloadRequested(e.Url, DateTimeOffset.UtcNow)), 
                cancellationToken);

            return Results.Accepted();
        });
        
        app.MapPost("retry-failed-videos/{id:guid}", async (
            [FromServices] IBus bus,
            [FromServices] AppDbContext dbContext,
            [FromRoute] Guid id,
            CancellationToken cancellationToken) =>
        {
            var failedVideo = await dbContext.Set<DownloadVideoState>()
                .FirstOrDefaultAsync(e => e.CorrelationId == id && e.CurrentState == "DownloadFailed", cancellationToken);

            if (failedVideo is null)
            {
                return Results.NotFound();
            }
            
            await bus.Publish(new RetryFailedVideoDownloadRequested(failedVideo.Url, DateTimeOffset.UtcNow), cancellationToken);

            return Results.Accepted();
        });
        
        app.MapPost("retry-failed-pagescrapes", async (
            [FromServices] IBus bus,
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var failedVideos = await dbContext.Set<ScrapePageState>()
                .Where(e => e.CurrentState == "ScrapeFailed")
                .ToListAsync(cancellationToken);
            
            await bus.PublishBatch(failedVideos.Select(e => new RetryFailedPageScrapeRequested(e.Url, DateTimeOffset.UtcNow)), 
                cancellationToken);

            return Results.Accepted();
        });

        app.Run();
    }
}