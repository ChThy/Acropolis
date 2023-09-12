using Acropolis.Domain.Messenger;

namespace Acropolis.Domain.Repositories;

public interface IIncomingRequestRepostory
{
    Task Add(IncomingRequest request);
    Task MarkAsProcessed(Guid id);
}
