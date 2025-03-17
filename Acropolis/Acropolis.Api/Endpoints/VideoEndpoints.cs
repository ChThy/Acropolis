using Acropolis.Api.Extensions;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Domain.DownloadedVideos;
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

        group.MapGet("", Videos)
            .Produces<DownloadedVideo[]>()
            .ProducesCommonResponses()
            .WithName(nameof(Videos));

        group.MapGet("requested", RequestedVideos)
            .Produces<DownloadVideoState[]>()
            .ProducesCommonResponses()
            .WithName(nameof(RequestedVideos));

        group.MapPost("failed/retry", RetryAllFailedVideos)
            .Produces(StatusCodes.Status202Accepted)
            .ProducesCommonResponses()
            .WithName(nameof(RetryAllFailedVideos));

        group.MapPost("failed/{id:guid}/retry", RetryFailedVideo)
            .Produces(StatusCodes.Status202Accepted)
            .ProducesCommonResponses()
            .WithName(nameof(RetryFailedVideo));

        return endpoints;
    }

    private static async Task<IResult> Videos(
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.DownloadedVideos
            .Include(e => e.Resources)
            .ToListAsync(cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> RequestedVideos(
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.Set<DownloadVideoState>()
            .ToListAsync(cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> RetryAllFailedVideos(
        [FromServices] IBus bus,
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var failedVideos = await dbContext.Set<DownloadVideoState>()
            .Where(e => e.CurrentState == "DownloadFailed")
            .ToListAsync(cancellationToken);

        await bus.PublishBatch(failedVideos.Select(e => new RetryFailedVideoDownloadRequested(e.Url, DateTimeOffset.UtcNow)),
            cancellationToken);

        return Results.Accepted();
    }

    private static async Task<IResult> RetryFailedVideo(
        [FromServices] IBus bus,
        [FromServices] AppDbContext dbContext,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var failedVideo = await dbContext.Set<DownloadVideoState>()
            .FirstOrDefaultAsync(e => e.CorrelationId == id && e.CurrentState == "DownloadFailed", cancellationToken);

        if (failedVideo is null)
        {
            return Results.NotFound();
        }

        await bus.Publish(new RetryFailedVideoDownloadRequested(failedVideo.Url, DateTimeOffset.UtcNow), cancellationToken);

        return Results.Accepted();
    }
}