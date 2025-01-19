using Acropolis.Application.Shared;

namespace Acropolis.Infrastructure.EfCore.CommandHandlers;

public class SaveChangesCommandHandler(AppDbContext dbContext) : CommandHandlerBase<SaveChangesCommand, AppDbContext>(dbContext)
{
    public override Task Handle(SaveChangesCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Add is not null)
        {
            DbContext.AddRange(command.Add);
        }

        if (command.Remove is not null)
        {
            DbContext.RemoveRange(command.Remove);
        }
        
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}