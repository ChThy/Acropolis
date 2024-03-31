using Acropolis.Application.Events;
using MassTransit;

namespace Acropolis.Application.Sagas;
public class DownloadVideoSaga : MassTransitStateMachine<DownloadVideoState>
{

    public DownloadVideoSaga()
    {
        InstanceState(e => e.CurrentState);
        Initially(
            When(WhenVideoDownloadRequested)
            .Then(x =>
                {
                    var saga = x.Saga;

                    saga.RequestedTimestamp = x.Message.Timestamp;
                    saga.Url = x.Message.Url;
                })
            .TransitionTo(DownloadRequested));

        Event(() => WhenVideoDownloadRequested, e => e.CorrelateBy<string>(ctx => ctx.Url, saga => saga.Message.Url));
    }

    public Event<VideoDownloadRequested> WhenVideoDownloadRequested { get; private set; } = null!;

    public State DownloadRequested { get; private set; } = null!;
}

public class DownloadVideoState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public byte[] RowVersion { get; set; } = null!;
    public string CurrentState { get; set; } = null!;

    public DateTimeOffset RequestedTimestamp { get; set; }
    public string Url { get; set; } = null!;
}
