using Acropolis.Api.Extensions;
using Acropolis.Application.Events.PageScraper;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Domain.ScrapedPages;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.Endpoints;

public static class PageEndpoints
{
    public static IEndpointRouteBuilder MapPagesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("pages").WithTags("Pages");

        group.MapGet("", Pages)
            .Produces<ScrapedPage[]>()
            .ProducesCommonResponses()
            .WithName(nameof(Pages));

        group.MapGet("requested", RequestedPages)
            .Produces<ScrapePageState[]>()
            .ProducesCommonResponses()
            .WithName(nameof(RequestedPages));

        group.MapPost("failed/retry", RetryFailedScrapes)
            .Produces(StatusCodes.Status202Accepted)
            .ProducesCommonResponses()
            .WithName(nameof(RetryFailedScrapes));

        return endpoints;
    }

    private static async Task<IResult> Pages(
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.ScrapedPages
            .Include(e => e.Resources)
            .ToListAsync(cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> RequestedPages(
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var result = await dbContext.Set<ScrapePageState>().ToListAsync(cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> RetryFailedScrapes(
        [FromServices] IBus bus,
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var failedVideos = await dbContext.Set<ScrapePageState>()
            .Where(e => e.CurrentState == "ScrapeFailed")
            .ToListAsync(cancellationToken);

        await bus.PublishBatch(failedVideos.Select(e => new RetryFailedPageScrapeRequested(e.Url, DateTimeOffset.UtcNow)),
            cancellationToken);

        return Results.Accepted();
    }
}