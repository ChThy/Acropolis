using Acropolis.Application.Events;

namespace Acropolis.Application.YoutubeDownloader.ApplicationEvents;
public record YoutubeVideoDownloaded(Guid DownloadedVideoId, string VideoTitle) : ApplicationEvent;
