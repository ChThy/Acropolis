using Acropolis.Application.Mediator;

namespace Acropolis.Application.YoutubeDownloader;

public record RetryFailedDownloadsCommand(Guid IncomingRequestId) : ICommand;
