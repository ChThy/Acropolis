﻿using Acropolis.Application.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore;

public class DownloadVideoStateMap : SagaClassMap<DownloadVideoState>
{
    protected override void Configure(EntityTypeBuilder<DownloadVideoState> entity, ModelBuilder model)
    {
        entity.Property(e => e.CurrentState).HasMaxLength(64);
        entity.Property(e => e.RowVersion).IsRowVersion();
    }
}