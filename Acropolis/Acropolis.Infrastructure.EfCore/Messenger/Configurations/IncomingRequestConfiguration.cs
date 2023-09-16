using Acropolis.Domain.Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore.Messenger.Configurations;

public sealed class IncomingRequestConfiguration : IEntityTypeConfiguration<IncomingRequest>
{
    public void Configure(EntityTypeBuilder<IncomingRequest> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.HasIndex(e => e.ExternalId);
            

        builder.HasIndex(e => e.Source);
        builder.HasIndex(e => e.Timestamp);
        builder.HasIndex(e => e.ProcessedOn);

        builder.OwnsOne(e => e.User, ob =>
        {
            ob.Property(e => e.ExternalId)
                .HasMaxLength(256)
                .IsRequired();
            ob.HasIndex(e => e.ExternalId);

            ob.Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();
            ob.HasIndex(e => e.Name);
        });
    }
}
