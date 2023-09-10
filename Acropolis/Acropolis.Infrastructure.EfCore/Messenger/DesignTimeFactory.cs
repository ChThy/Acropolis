using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acropolis.Infrastructure.EfCore.Messenger;

internal class DesignTimeFactory : IDesignTimeDbContextFactory<MessengerDbContext>
{
    public MessengerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MessengerDbContext>();
        optionsBuilder.UseSqlite();
        return new MessengerDbContext(optionsBuilder.Options);
    }
}
