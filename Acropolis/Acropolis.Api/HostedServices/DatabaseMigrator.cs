using Acropolis.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.HostedServices;

public class DatabaseMigrator : IHostedService
{
    private readonly IDbContextFactory<AppDbContext> messengerDbContextFactory;
    private readonly ILogger<DatabaseMigrator> logger;

    public DatabaseMigrator(IDbContextFactory<AppDbContext> messengerDbContextFactory, ILogger<DatabaseMigrator> logger)
    {
        this.messengerDbContextFactory = messengerDbContextFactory;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var messengerDbContext = await messengerDbContextFactory.CreateDbContextAsync(cancellationToken);
        await messengerDbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Done migrating databases");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
