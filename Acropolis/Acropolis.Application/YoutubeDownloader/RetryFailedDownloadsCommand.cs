using MediatR;

namespace Acropolis.Application.YoutubeDownloader;

public record RetryFailedDownloadsCommand(Guid IncomingRequestId) : IRequest;
