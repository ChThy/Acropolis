﻿using Acropolis.Application.Sagas.DownloadVideo;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore.Sqlite.Migrations;

public class DownloadVideoStateMap : SagaClassMap<DownloadVideoState>
{
    protected override void Configure(EntityTypeBuilder<DownloadVideoState> entity, ModelBuilder model)
    {
        entity.Property(e => e.CurrentState).HasMaxLength(64);
        entity.Property(e => e.RowVersion)
            .HasDefaultValue(0)
            .IsRowVersion();
        entity.OwnsOne(e => e.StoredVideo, ob =>
        {
            ob.Property(e => e.StorageLocation).IsRequired();
            ob.OwnsOne(e => e.MetaData, oob =>
            {
                oob.Property(e => e.VideoId).IsRequired();
            });
        });

        entity.HasIndex(e => new { e.CorrelationId, e.Url })
            .IsUnique();

        entity.HasIndex(e => e.Url)
            .IsUnique();
    }
}