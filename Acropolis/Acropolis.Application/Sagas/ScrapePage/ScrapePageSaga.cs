using Acropolis.Application.Events;
using Acropolis.Application.Events.PageScraper;
using Acropolis.Application.Extensions.MassTransitExtensions;
using MassTransit;

namespace Acropolis.Application.Sagas.ScrapePage;

public class ScrapePageSaga : MassTransitStateMachine<ScrapePageState>
{
    public ScrapePageSaga()
    {
        InstanceState(e => e.CurrentState);

        Initially(
            When(WhenUrlRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();

                    saga.RequestedTimestamp = message.Timestamp;
                    saga.Url = message.Url;

                    await ctx.Publish(new PageScrapeRequested(saga.Url));
                })
                .TransitionTo(ScrapeRequested)
        );
        
        During(ScrapeRequested,
            Ignore(WhenUrlRequestReceived),
            When(WhenPageScraped)
                .Then(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.ScrapedTimestamp = message.Timestamp;
                    saga.Title = message.PageTitle;
                    saga.Domain = message.Domain;
                    saga.StorageLocation = message.StorageLocation;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new UrlRequestReplyRequested(
                        saga.CorrelationId,
                        saga.Url,
                        $"Scraped {saga.Title}. Location: {saga.StorageLocation}");
                })
                .TransitionTo(Scraped),
            When(WhenPageScapeSkipped)
                // .Publish(ctx =>
                // {
                //     var (saga, message) = ctx.Deconstruct();
                //     return new UrlRequestReplyRequested(saga.CorrelationId, saga.Url, $"Scrape skipped: {message.Reason}");
                // })
                .TransitionTo(ScrapeSkipped),
            When(WhenPageScrapeFailed)
                .Then(x =>
                {
                    var (saga, message) = x.Deconstruct();
                    saga.ErrorMessage = message.ErrorMessage;
                    saga.ErrorTimestamp = message.Timestamp;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new UrlRequestReplyRequested(
                        saga.CorrelationId,
                        saga.Url,
                        $"Scrape failed: {message.ErrorMessage}");
                })
                .TransitionTo(ScrapeFailed)
        );
        
        During(Scraped,
            When(WhenUrlRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    await ctx.Publish(new UrlRequestReplyRequested(saga.CorrelationId, saga.Url, $"Already scraped. Location: {saga.StorageLocation}"));
                }),
            When(WhenPageScraped)
                .Then(x =>
                {
                    var (saga, message) = x.Deconstruct();
                    saga.ScrapedTimestamp = message.Timestamp;
                    saga.Title = message.PageTitle;
                    saga.Domain = message.Domain;
                    saga.StorageLocation = message.StorageLocation;
                }),
            Ignore(WhenPageScapeSkipped),
            Ignore(WhenPageScrapeFailed),
            Ignore(WhenRetryFailedPageScrapeRequested)
        );
        
        During(ScrapeSkipped,
            Ignore(WhenUrlRequestReceived),
            Ignore(WhenPageScraped),
            Ignore(WhenPageScapeSkipped),
            Ignore(WhenPageScrapeFailed),
            Ignore(WhenRetryFailedPageScrapeRequested)
        );
        
        During(ScrapeFailed,
            When(WhenUrlRequestReceived)
                .Then(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.ErrorTimestamp = null;
                    saga.ErrorMessage = null;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new PageScrapeRequested(saga.Url);
                })
                .TransitionTo(ScrapeRequested),
            When(WhenRetryFailedPageScrapeRequested)
                .Then(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.ErrorTimestamp = null;
                    saga.ErrorMessage = null;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new PageScrapeRequested(saga.Url);
                })
                .TransitionTo(ScrapeRequested),
            Ignore(WhenPageScraped),
            Ignore(WhenPageScapeSkipped),
            Ignore(WhenPageScrapeFailed)
        );
        
        Event(() => WhenUrlRequestReceived,
            e => e.CorrelateById(ctx => ctx.Message.RequestId).CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenPageScraped,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenPageScrapeFailed,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenPageScapeSkipped,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenRetryFailedPageScrapeRequested,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
    }

    public Event<UrlRequestReceived> WhenUrlRequestReceived { get; private set; } = null!;
    public Event<PageScraped> WhenPageScraped { get; private set; } = null!;
    public Event<PageScrapeSkipped> WhenPageScapeSkipped { get; private set; } = null!;
    public Event<PageScrapeFailed> WhenPageScrapeFailed { get; private set; } = null!;
    public Event<RetryFailedPageScrapeRequested> WhenRetryFailedPageScrapeRequested { get; private set; } = null!;

    public State ScrapeRequested { get; private set; } = null!;
    public State Scraped { get; private set; } = null!;
    public State ScrapeFailed { get; private set; } = null!;
    public State ScrapeSkipped { get; private set; } = null!;
}