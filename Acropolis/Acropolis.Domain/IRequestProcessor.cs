using Acropolis.Domain.Messenger;

namespace Acropolis.Domain;
public interface IRequestProcessor
{
    ValueTask Process(IncomingRequest request, CancellationToken cancellationToken);
}
