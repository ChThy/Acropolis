using Acropolis.Api.Extensions;
using Acropolis.Api.Models;
using Acropolis.Application.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Acropolis.Api.Endpoints;

public static class DownloadEndpoints
{
    public static IEndpointRouteBuilder MapDownloadEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("download").WithTags("Download");

        group.MapPost("", Download)
            .Produces(StatusCodes.Status202Accepted)
            .ProducesCommonResponses()
            .WithName(nameof(Download));

        return endpoints;
    }

    private static async Task<IResult> Download(
        [FromServices] IBus bus,
        [FromBody] DownloadVideoRequest request,
        CancellationToken cancellationToken)
    {
        await bus.Publish(new UrlRequestReceived(Guid.NewGuid(), request.Url, DateTimeOffset.UtcNow),
            ctx => ctx.CorrelationId = NewId.NextGuid(), cancellationToken);

        return Results.Accepted();
    }
}