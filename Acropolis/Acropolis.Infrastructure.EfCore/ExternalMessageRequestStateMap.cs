using System.Text.Json;
using Acropolis.Application.Sagas.ExternalMessageRequest;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore;

public class ExternalMessageRequestStateMap : SagaClassMap<ExternalMessageRequestState>
{
    protected override void Configure(EntityTypeBuilder<ExternalMessageRequestState> entity, ModelBuilder model)
    {
        entity.Property(e => e.CurrentState).HasMaxLength(64);
        entity.Property(e => e.RowVersion)
            .HasDefaultValue(0)
            .IsRowVersion();

        entity.Property(e => e.MessageProps)
            .HasConversion(
                props => JsonSerializer.Serialize(props, JsonSerializerOptions.Default),
                props => JsonSerializer.Deserialize<Dictionary<string, string>>(props,JsonSerializerOptions.Default) ?? new Dictionary<string, string>());
    }
}