using Acropolis.Application.Models;
using MassTransit;

namespace Acropolis.Application.Sagas.DownloadVideo;

public class DownloadVideoState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int RowVersion { get; set; }
    public string CurrentState { get; set; } = null!;

    public string Url { get; set; } = null!;
    public DateTimeOffset RequestedTimestamp { get; set; }
    public DateTimeOffset? DownloadedTimestamp { get; set; }
    public VideoMetaData? VideoMetaData { get; set; }

    public string? ErrorMessage { get; set; }
    public DateTimeOffset? ErrorTimestamp { get; set; }
}
