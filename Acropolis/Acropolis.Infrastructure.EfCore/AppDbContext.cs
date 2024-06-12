using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Infrastructure.EfCore;

public class AppDbContext : SagaDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<InboxState> InboxStates => Set<InboxState>();
    public DbSet<OutboxState> OutboxStates => Set<OutboxState>();

    protected override IEnumerable<ISagaClassMap> Configurations =>
    [
        new DownloadVideoStateMap(),
        new ScrapePageStateMap(),
        new ExternalMessageRequestStateMap()
    ];

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseCollation("BINARY");

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .UseCollation("BINARY");
    }
}