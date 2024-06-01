using MassTransit;

namespace Acropolis.Application.Sagas.ExternalMessageRequest;

public class ExternalMessageRequestSaga : MassTransitStateMachine<ExternalMessageRequestState>
{
    public ExternalMessageRequestSaga()
    {
        InstanceState(e => e.CurrentState);
        //TODO implement
    }
}