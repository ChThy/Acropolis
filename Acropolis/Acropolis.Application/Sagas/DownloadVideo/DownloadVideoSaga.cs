using Acropolis.Application.Events;
using Acropolis.Application.Extensions.MassTransitExtensions;
using MassTransit;

namespace Acropolis.Application.Sagas.DownloadVideo;
public class DownloadVideoSaga : MassTransitStateMachine<DownloadVideoState>
{
    public DownloadVideoSaga()
    {
        InstanceState(e => e.CurrentState);
        Initially(
            When(WhenUrlRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();

                    saga.RequestedTimestamp = message.Timestamp;
                    saga.Url = message.Url;

                    await ctx.Publish(new VideoDownloadRequested(saga.Url));
                })
                .TransitionTo(DownloadRequested));

        During(DownloadRequested,
            When(WhenUrlRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();

                    await ctx.Publish(new VideoDownloadRequested(saga.Url)); //TODO: Redownload?
                    await ctx.Publish(new VideoDownloadAlreadyRequested(saga.Url, saga.RequestedTimestamp));
                }),
            When(WhenVideoDownloaded)
                .Then(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.DownloadedTimestamp = message.Timestamp;
                    saga.VideoMetaData = message.VideoMetaData;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new UrlRequestReplyRequested(
                        saga.CorrelationId,
                        saga.Url,
                        "Done");
                })
                .TransitionTo(Downloaded),
            When(WhenVideoDownloadSkipped)
                .TransitionTo(DownloadSkipped),
            When(WhenVideoDownloadFailed)
                .Then(x =>
                {
                    var (saga, message) = x.Deconstruct();
                    saga.ErrorMessage = message.ErrorMessage;
                    saga.DownloadedTimestamp = message.Timestamp;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new UrlRequestReplyRequested(
                        saga.CorrelationId,
                        saga.Url,
                        "Download failed");
                })
                .TransitionTo(DownloadFailed)
        );

        During(Downloaded,
            When(WhenUrlRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    await ctx.Publish(new VideoAlreadyDownloaded(saga.Url, saga.DownloadedTimestamp!.Value, saga.VideoMetaData!));
                }),
            When(WhenVideoDownloaded)
                .Then(x =>
                {
                    var (saga, message) = x.Deconstruct();

                    saga.DownloadedTimestamp = message.Timestamp;
                    saga.VideoMetaData = message.VideoMetaData;
                })
        );
        
        During(DownloadSkipped,
            Ignore(WhenUrlRequestReceived));
        During(DownloadFailed,
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
                    return new VideoDownloadRequested(saga.Url);
                })
                .TransitionTo(DownloadRequested)
            );

        Event(() => WhenUrlRequestReceived,
            e => e.CorrelateById(ctx => ctx.Message.RequestId).CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenVideoDownloaded,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenVideoDownloadSkipped,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenVideoDownloadFailed,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
    }

    public Event<UrlRequestReceived> WhenUrlRequestReceived { get; private set; } = null!;
    public Event<VideoDownloaded> WhenVideoDownloaded { get; private set; } = null!;
    public Event<VideoDownloadSkipped> WhenVideoDownloadSkipped { get; private set; } = null!;
    public Event<VideoDownloadFailed> WhenVideoDownloadFailed { get; private set; } = null!;

    public State DownloadRequested { get; private set; } = null!;
    public State Downloaded { get; private set; } = null!;
    public State DownloadFailed { get; private set; } = null!;
    public State DownloadSkipped { get; private set; } = null!;
}
