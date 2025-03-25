using Acropolis.Domain.DownloadedVideos;
using Acropolis.Domain.ScrapedPages;
using Acropolis.Infrastructure.EfCore.Sqlite.Migrations;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Infrastructure.EfCore.Sqlite;

public class SqliteAppDbContext : SagaDbContext
{
    public SqliteAppDbContext(DbContextOptions<SqliteAppDbContext> options) : base(options)
    {
    }

    public DbSet<InboxState> InboxStates => Set<InboxState>();
    public DbSet<OutboxState> OutboxStates => Set<OutboxState>();

    public DbSet<DownloadedVideo> DownloadedVideos => Set<DownloadedVideo>();
    public DbSet<ScrapedPage> ScrapedPages => Set<ScrapedPage>();

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

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .UseCollation("BINARY");
    }
}