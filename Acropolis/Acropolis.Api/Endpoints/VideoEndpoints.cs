using System.Net.Mime;
using Acropolis.Api.Extensions;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Domain.DownloadedVideos;
using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.EfCore.Extensions;
using Acropolis.Infrastructure.Extensions;
using Acropolis.Infrastructure.Options;
using Acropolis.Shared.Models;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        group.MapGet("{videoId:guid}/{resourceId:guid}", DownloadVideo)
            .Produces<PhysicalFileResult>(200, "video/mp4")
            .ProducesCommonResponses()
            .WithName(nameof(DownloadVideo));

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

        group.MapDelete("{id:guid}", DeleteVideo)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesCommonResponses()
            .WithName(nameof(DeleteVideo));

        return endpoints;
    }

    private static async Task<IResult> GetVideos(
        [AsParameters] Shared.Models.Sieve sieve,
        [FromServices] AppDbContext dbContext,
        [FromServices] ISieveProcessor sieveProcessor,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.DownloadedVideos
            .Include(e => e.Resources)
            .AsPagedResult(
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
        var result = await dbContext.DownloadedVideos
            .Include(e => e.Resources)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return result is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(result);
    }

    private static async Task<IResult> DownloadVideo(
        Guid videoId,
        Guid resourceId, 
        HttpContext httpContext,
        [FromServices] AppDbContext dbContext, 
        [FromServices] IOptionsMonitor<FileStorageOptions> optionsMonitor,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.DownloadedVideos
            .Include(e => e.Resources)
            .FirstOrDefaultAsync(e => e.Id == videoId, cancellationToken);
        
        var resource = result?.Resources.FirstOrDefault(e => e.Id == resourceId);

        if (resource is null)
        {
            return Results.NotFound();
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), optionsMonitor.CurrentValue.BaseDirectory, resource.StorageLocation).Replace("?", "");

        if (!File.Exists(filePath))
        {
            return Results.NotFound();
        }
        
        var fileStream = File.OpenRead(filePath);
        return Results.File(fileStream, contentType: "video/mp4", "filename.mp4", enableRangeProcessing: true);
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
    
    private static async Task<IResult> DeleteVideo(
        Guid id,
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.DownloadedVideos.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (result is null)
        {
            return Results.NotFound();
        }

        dbContext.DownloadedVideos.Remove(result);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
}