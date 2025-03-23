using Acropolis.Domain.DownloadedVideos;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Acropolis.Infrastructure.Sieve;

public class ApplicationSieveProcessor : SieveProcessor
{
    public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods, ISieveCustomFilterMethods customFilterMethods)
        : base(options, customSortMethods, customFilterMethods)
    {
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<DownloadedVideo>(e => e.MetaData.VideoTitle)
            .CanSort()
            .HasName("Title");

        mapper.Property<DownloadedVideo>(e => e.MetaData.Author)
            .CanSort()
            .HasName("Author");
        mapper.Property<DownloadedVideo>(e => e.MetaData.VideoUploadTimestamp)
            .CanSort()
            .HasName("UploadedTimestamp");
        
        return mapper;
    }
}