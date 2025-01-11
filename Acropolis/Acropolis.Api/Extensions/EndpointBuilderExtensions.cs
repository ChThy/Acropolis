using Acropolis.Api.Models;
using Acropolis.Application.DownloadedVideos;
using Acropolis.Application.DownloadedVideos.CreateDownloadedVideo;
using Acropolis.Application.Events;
using Acropolis.Application.Events.PageScraper;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.Extensions;

public static class EndpointBuilderExtensions
{
    public static IEndpointRouteBuilder MapDownloadEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("download").WithTags("Download");
        
        group.MapPost("download", async (
            [FromServices] IBus bus,
            [FromBody] DownloadVideoRequest request,
            CancellationToken cancellationToken) =>
        {
            await bus.Publish(new UrlRequestReceived(Guid.NewGuid(), request.Url, DateTimeOffset.UtcNow),
                ctx => ctx.CorrelationId = NewId.NextGuid(), cancellationToken);
            
            return Results.Accepted();
        });

        var debug = endpoints.MapGroup("debug").WithTags("Debug");

        debug.MapGet("", async (IMediator mediator) =>
        {
            await mediator.Send(new CreateDownloadedVideoRequest(new("", DateTimeOffset.Now, null!)));
            return Results.Ok();
        });
        
        return endpoints;
    }
    
    public static IEndpointRouteBuilder MapVideoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("videos").WithTags("Videos");

        group.MapGet("", async (
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var result = await dbContext.Set<DownloadVideoState>()
                .ToListAsync(cancellationToken);
            
            return Results.Ok(result);
        }).Produces<DownloadVideoState[]>();
        
        group.MapPost("failed/retry", async (
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
        
        group.MapPost("failed/{id:guid}/retry", async (
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
        
        return endpoints;
    }

    public static IEndpointRouteBuilder MapScrapedPagesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("pages").WithTags("Pages");
        
        group.MapGet("", async (
            [FromServices] AppDbContext dbContext,
            [FromQuery] bool includeSkipped,
            CancellationToken cancellationToken) =>
        {
            var query = dbContext.Set<ScrapePageState>().AsQueryable();

            if (!includeSkipped)
            {
                query = query.Where(e => e.CurrentState != nameof(ScrapePageSaga.ScrapeSkipped));
            }
            var result = await query.ToListAsync(cancellationToken);
            
            return Results.Ok(result);
        }).Produces<ScrapePageState[]>();

        group.MapPost("failed/retry", async (
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
        
        return endpoints;
    }
}