using Acropolis.Application.Events;
using Acropolis.Application.Extensions.MassTransitExteions;
using MassTransit;

namespace Acropolis.Application.Sagas;
public class DownloadVideoSaga : MassTransitStateMachine<DownloadVideoState>
{
    //TODO:
    // - clustered primary key
    // - outbox pattern


    public DownloadVideoSaga()
    {
        InstanceState(e => e.CurrentState);
        Initially(
            When(WhenVideoDownloadRequestReceived)
                .ThenAsync(async x =>
                {
                    var (saga, message) = x.Destruct();

                    saga.RequestedTimestamp = message.Timestamp;
                    saga.Url = message.Url;

                    await x.Publish(new VideoDownloadRequested(saga.Url));
                })
                .TransitionTo(DownloadRequested));

        During(DownloadRequested,
            When(WhenVideoDownloadRequestReceived)
                .ThenAsync(async x =>
                {
                    var (saga, message) = x.Destruct();

                    await x.Publish(new VideoDownloadRequested(saga.Url));  //TODO: Redownload?
                    await x.Publish(new VideoDownloadAlreadyRequested(saga.Url, saga.RequestedTimestamp));
                }),
            When(WhenVideoDownloaded)
                .Then(x =>
                {
                    var (saga, message) = x.Destruct();
                    saga.DownloadedTimestamp = message.Timestamp;
                    saga.VideoMetaData = message.VideoMetaData;
                })
                .TransitionTo(Downloaded),
            When(WhenVideoDownloadFailed)
                .Then(x =>
                {
                    var (saga, message) = x.Destruct();
                    saga.ErrorMessage = message.ErrorMessage;
                    saga.DownloadedTimestamp = message.Timestamp;
                })
        );

        During(Downloaded,
            When(WhenVideoDownloadRequestReceived)
                .ThenAsync(async x =>
                {
                    var (saga, message) = x.Destruct();
                    await x.Publish(new VideoAlreadyDownloaded(saga.Url, saga.DownloadedTimestamp!.Value, saga.VideoMetaData!));
                }),
            When(WhenVideoDownloaded)
                .Then(x =>
                {
                    var (saga, message) = x.Destruct();

                    saga.DownloadedTimestamp = message.Timestamp;
                    saga.VideoMetaData = message.VideoMetaData;
                })
        );

        Event(() => WhenVideoDownloadRequestReceived,
            e => e.CorrelateBy(ctx => ctx.Url, saga => saga.Message.Url));
        Event(() => WhenVideoDownloaded,
            e => e.CorrelateBy(ctx => ctx.Url, saga => saga.Message.Url));
        Event(() => WhenVideoDownloadFailed,
            e => e.CorrelateBy(ctx => ctx.Url, saga => saga.Message.Url));
    }

    public Event<VideoDownloadRequestReceived> WhenVideoDownloadRequestReceived { get; private set; } = null!;
    public Event<VideoDownloaded> WhenVideoDownloaded { get; private set; } = null!;
    public Event<VideoDownloadFailed> WhenVideoDownloadFailed { get; private set; } = null!;


    public State DownloadRequested { get; private set; } = null!;
    public State Downloaded { get; private set; } = null!;
    public State DownloadFailed { get; private set; } = null!;
}
