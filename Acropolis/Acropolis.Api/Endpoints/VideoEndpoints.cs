using Acropolis.Api.Extensions;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Domain.DownloadedVideos;
using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.EfCore.Extensions;
using Acropolis.Infrastructure.Extensions;
using Acropolis.Shared.Models;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Acropolis.Api.Endpoints;

public static class VideoEndpoints
{
    public static IEndpointRouteBuilder MapVideoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("videos").WithTags("Videos");

        group.MapGet("", GetVideos)
            .Produces<PagedResult<DownloadedVideo>>()
            .ProducesCommonResponses()
            .WithName(nameof(GetVideos));

        group.MapGet("{id:guid}", GetVideo)
            .Produces<DownloadedVideo>()
            .ProducesCommonResponses()
            .WithName(nameof(GetVideo));

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

    private static async Task<IResult> GetVideos(
        [AsParameters] Shared.Models.Sieve sieve,
        [FromServices] AppDbContext dbContext,
        [FromServices] ISieveProcessor sieveProcessor,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.DownloadedVideos.AsPagedResult(
            sieveProcessor,
            sieve,
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetVideo(
        Guid id,
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.DownloadedVideos.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return result is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(result);
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