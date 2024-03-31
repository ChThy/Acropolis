using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Infrastructure.EfCore;
public class AppDbContext : SagaDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new DownloadVideoStateMap(); }
    }
}
