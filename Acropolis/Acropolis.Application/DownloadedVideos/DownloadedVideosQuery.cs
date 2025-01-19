using Acropolis.Domain.DownloadedVideos;
using Acropolis.Shared.Queries;

namespace Acropolis.Application.DownloadedVideos;

//TODO: use or clean up
public record DownloadedVideosQuery : IQuery<DownloadedVideo[]>;