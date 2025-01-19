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
    public static IEndpointRouteBuilder MapScrapedPagesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("pages").WithTags("Pages");
        
        group.MapGet("", async (
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var result = await dbContext.ScrapedPages
                .Include(e => e.Resources)
                .ToListAsync(cancellationToken);
            
            return Results.Ok(result);
        }).Produces<ScrapedPage[]>();
        
        group.MapGet("requested", async (
            [FromServices] AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var result = await dbContext.Set<ScrapePageState>().ToListAsync(cancellationToken);
            
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