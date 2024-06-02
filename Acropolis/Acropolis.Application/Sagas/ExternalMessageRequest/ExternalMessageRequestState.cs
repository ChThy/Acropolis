using MassTransit;

namespace Acropolis.Application.Sagas.ExternalMessageRequest;

public sealed class ExternalMessageRequestState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int RowVersion { get; set; }
    public string CurrentState { get; set; } = null!;

    public string Channel { get; set; } = null!;
    public DateTimeOffset ReceivedOn { get; set; }
    public string? MessageBody { get; set; }

    public Dictionary<string, string> MessageProps { get; set; } = [];
}