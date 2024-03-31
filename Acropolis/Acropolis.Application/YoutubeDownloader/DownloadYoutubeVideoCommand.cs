using MediatR;

namespace Acropolis.Application.YoutubeDownloader;

public record DownloadYoutubeVideoCommand(Guid IncomingRequestId, string Url) : IRequest;
