using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Infrastructure.EfCore;
public class AppDbContext : SagaDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);
        modelBuilder.UseCollation("BINARY");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .UseCollation("BINARY");
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new DownloadVideoStateMap(); }
    }
}
