using Acropolis.Application.Mediator;
using Acropolis.Application.PageScraper.ApplicationEvents;
using Acropolis.Application.YoutubeDownloader.ApplicationEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Acropolis.Infrastructure.Dapr;
public static class SubcriptionEndpoints
{
    public static WebApplication UseEventSupscriptionEndpoints(this WebApplication app)
    {
        var daprSettings = app.Services.GetRequiredService<IOptions<DaprSettings>>().Value;
        app.UseCloudEvents();
        app.MapEventSubcriptionEndpoints(daprSettings);

        return app;
    }

    public static IEndpointRouteBuilder MapEventSubcriptionEndpoints(this IEndpointRouteBuilder endpoints, DaprSettings daprSettings)
    {
        endpoints.MapSubscribeHandler();
        endpoints.MapPost("PageScraped", async (
            [FromServices] IMediator mediator,
            PageScraped @event) =>
        {
            await mediator.Send(@event);
        }).WithTopic(daprSettings.PubSubName, daprSettings.ScraperTopicName);

        endpoints.MapPost("YoutubeVideoDownloaded", async (
            [FromServices] IMediator mediator,
            YoutubeVideoDownloaded @event) =>
        {
            await mediator.Send(@event);
        }).WithTopic(daprSettings.PubSubName, daprSettings.YoutubeDownloaderTopicName);

        return endpoints;
    }
}
