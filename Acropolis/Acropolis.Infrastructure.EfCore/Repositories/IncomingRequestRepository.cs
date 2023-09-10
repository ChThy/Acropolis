using Acropolis.Domain.Messenger;
using Acropolis.Domain.Repositories;
using Acropolis.Infrastructure.EfCore.Messenger;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Infrastructure.EfCore.Repositories;

internal class IncomingRequestRepository : IIncomingRequestRepostiory
{
    private readonly IDbContextFactory<MessengerDbContext> dbContextFactory;

    public IncomingRequestRepository(IDbContextFactory<MessengerDbContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    private MessengerDbContext CreateDbContext() => dbContextFactory.CreateDbContext();

    public async Task Add(IncomingRequest request)
    {
        using var context = CreateDbContext();
        context.Add(request);
        await context.SaveChangesAsync();
    }
}
