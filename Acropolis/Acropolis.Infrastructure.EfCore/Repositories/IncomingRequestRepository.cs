using Acropolis.Domain.Exceptions;
using Acropolis.Domain.Messenger;
using Acropolis.Domain.Repositories;
using Acropolis.Infrastructure.EfCore.Messenger;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Acropolis.Infrastructure.EfCore.Repositories;

internal class IncomingRequestRepository : IIncomingRequestRepostory
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

    public async Task Update(Guid id, Action<IncomingRequest> update)
    {
        using var context = CreateDbContext();
        var request = await context.IncomingRequests.FindAsync(id) 
            ?? throw new NotFoundException(typeof(IncomingRequest).Name, id.ToString());

        update(request);
        await context.SaveChangesAsync();
    }
}
