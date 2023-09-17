using Acropolis.Application.Mediator;

namespace Acropolis.Application.YoutubeDownloader;

public record DownloadYoutubeVideoCommand(Guid IncomingRequestId, string Url) : ICommand;
