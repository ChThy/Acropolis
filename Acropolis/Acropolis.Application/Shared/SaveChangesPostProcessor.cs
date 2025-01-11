using Acropolis.Shared.Commands;
using MediatR.Pipeline;

namespace Acropolis.Application.Shared;

public class SaveChangesPostProcessor<TRequest, TResponse>(ICommandHandler commandHandler) : IRequestPostProcessor<TRequest, TResponse> 
    where TRequest : notnull
{
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        return commandHandler.Handle(new SaveChangesCommand(), cancellationToken);
    }
}