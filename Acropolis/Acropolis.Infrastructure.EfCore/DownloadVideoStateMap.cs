using Acropolis.Application.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore;

public class DownloadVideoStateMap : SagaClassMap<DownloadVideoState>
{
    protected override void Configure(EntityTypeBuilder<DownloadVideoState> entity, ModelBuilder model)
    {
        entity.Property(e => e.CurrentState).HasMaxLength(64);
        entity.Property(e => e.RowVersion)
            .HasDefaultValue(0)
            .IsRowVersion();
        entity.OwnsOne(e => e.VideoMetaData, ob =>
        {
            ob.Property(e => e.VideoId).IsRequired();
        });

        entity.HasIndex(e => new { e.CorrelationId, e.Url })
            .IsUnique();

        entity.HasIndex(e => e.Url)
            .IsUnique();
    }
}
