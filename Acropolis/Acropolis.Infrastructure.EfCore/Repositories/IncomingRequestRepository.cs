using Acropolis.Domain.Messenger;
using Acropolis.Domain.Repositories;
using Acropolis.Infrastructure.EfCore.Messenger;

namespace Acropolis.Infrastructure.EfCore.Repositories;
internal class IncomingRequestRepository : IIncomingRequestRepostiory
{
    private readonly MessengerDbContext dbContext;

    public IncomingRequestRepository(MessengerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(IncomingRequest request)
    {
        dbContext.Add(request);
        await dbContext.SaveChangesAsync();
    }
}
