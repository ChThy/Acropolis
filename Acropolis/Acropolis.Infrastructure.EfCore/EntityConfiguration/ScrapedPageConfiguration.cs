using Acropolis.Domain.ScrapedPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acropolis.Infrastructure.EfCore.EntityConfiguration;

internal class ScrapedPageConfiguration : IEntityTypeConfiguration<ScrapedPage>
{
    public void Configure(EntityTypeBuilder<ScrapedPage> builder)
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