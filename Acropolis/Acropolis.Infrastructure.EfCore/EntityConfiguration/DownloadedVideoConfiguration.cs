using Acropolis.Domain.DownloadedVideos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore.EntityConfiguration;

internal class DownloadedVideoConfiguration : IEntityTypeConfiguration<DownloadedVideo>
{
    public void Configure(EntityTypeBuilder<DownloadedVideo> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Url)
            .HasMaxLength(2048);
        
        builder.OwnsOne(e => e.MetaData);

        builder.HasMany(e => e.Resources)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}