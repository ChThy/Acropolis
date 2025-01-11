using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Domain.DownloadedVideos;
using MediatR;

namespace Acropolis.Application.DownloadedVideos.CreateDownloadedVideo;

public record CreateDownloadedVideoRequest(VideoDownloaded Video) : IRequest<DownloadedVideo>;