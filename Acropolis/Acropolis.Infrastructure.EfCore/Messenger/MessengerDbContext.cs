using Acropolis.Domain.Messenger;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Infrastructure.EfCore.Messenger;

public class MessengerDbContext : DbContext
{
    public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options)
    {
    }

    public DbSet<IncomingRequest> IncomingRequests => Set<IncomingRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseCollation("BINARY");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .UseCollation("BINARY");
    }
}
