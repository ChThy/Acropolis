using Acropolis.Application.Sagas.ScrapePage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore.Sqlite.Migrations;

public class ScrapePageStateMap : SagaClassMap<ScrapePageState>
{
    protected override void Configure(EntityTypeBuilder<ScrapePageState> entity, ModelBuilder model)
    {
        entity.Property(e => e.CurrentState).HasMaxLength(64);
        entity.Property(e => e.RowVersion)
            .HasDefaultValue(0)
            .IsRowVersion();

        entity.HasIndex(e => new { e.CorrelationId, e.Url })
            .IsUnique();

        entity.HasIndex(e => e.Url)
            .IsUnique();
    }
}