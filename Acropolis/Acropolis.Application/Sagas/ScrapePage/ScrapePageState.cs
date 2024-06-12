using MassTransit;

namespace Acropolis.Application.Sagas.ScrapePage;

public class ScrapePageState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int RowVersion { get; set; }
    public string CurrentState { get; set; } = null!;

    public string Url { get; set; } = null!;
    public DateTimeOffset RequestedTimestamp { get; set; }
    public DateTimeOffset? ScrapedTimestamp { get; set; }
    public string? Domain { get; set; }
    public string? Title { get; set; }
    public string? StorageLocation { get; set; }
    
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? ErrorTimestamp { get; set; }
}