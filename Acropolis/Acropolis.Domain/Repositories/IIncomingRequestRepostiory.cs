using Acropolis.Domain.Messenger;

namespace Acropolis.Domain.Repositories;
public interface IIncomingRequestRepostiory
{
    Task Add(IncomingRequest request);
}
