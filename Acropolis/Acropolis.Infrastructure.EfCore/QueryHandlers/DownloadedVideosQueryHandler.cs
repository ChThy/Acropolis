using Acropolis.Application.DownloadedVideos;
using Acropolis.Domain.DownloadedVideos;

namespace Acropolis.Infrastructure.EfCore.QueryHandlers;

public class DownloadedVideosQueryHandler(AppDbContext dbContext) : QueryHandlerBase<DownloadedVideosQuery, DownloadedVideo[], AppDbContext>(dbContext)
{
    public override async Task<DownloadedVideo[]> Handle(DownloadedVideosQuery query, CancellationToken cancellationToken = default)
    {
        // TODO
        return [];
    }
}