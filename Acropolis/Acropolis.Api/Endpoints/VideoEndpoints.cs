using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.Endpoints;

public static class VideoEndpoints
{
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
}