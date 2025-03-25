using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.EfCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.HostedServices;

public class DatabaseMigrator : IHostedService
{
    private readonly IDbContextFactory<AppDbContext> messengerDbContextFactory;
    private readonly IDbContextFactory<SqliteAppDbContext> sqliteDbContextFactory;
    private readonly ILogger<DatabaseMigrator> logger;

    public DatabaseMigrator(IDbContextFactory<AppDbContext> messengerDbContextFactory,
        IDbContextFactory<SqliteAppDbContext> sqliteDbContextFactory,
        ILogger<DatabaseMigrator> logger)
    {
        this.messengerDbContextFactory = messengerDbContextFactory;
        this.sqliteDbContextFactory = sqliteDbContextFactory;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var messengerDbContext = await messengerDbContextFactory.CreateDbContextAsync(cancellationToken);
        await messengerDbContext.Database.MigrateAsync(cancellationToken);

        await using var sqliteDbContext = await sqliteDbContextFactory.CreateDbContextAsync(cancellationToken);
        await sqliteDbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Done migrating databases");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
