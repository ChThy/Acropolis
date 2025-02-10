using Acropolis.Application.Shared;
using Acropolis.Domain;
using Acropolis.Domain.DownloadedVideos;
using Acropolis.Shared.Commands;
using MediatR;

namespace Acropolis.Application.DownloadedVideos.CreateDownloadedVideo;

public class CreateDownloadedVideoRequestHandler(ICommandHandler commandHandler) : IRequestHandler<CreateDownloadedVideoRequest, DownloadedVideo>
{
    public async Task<DownloadedVideo> Handle(CreateDownloadedVideoRequest request, CancellationToken cancellationToken)
    {
        var downloadedVideo = new DownloadedVideo(Guid.CreateVersion7(), request.Video.Url, new VideoMetaData(
            request.Video.VideoMetaData.VideoId,
            request.Video.VideoMetaData.VideoTitle,
            request.Video.VideoMetaData.Author,
            request.Video.VideoMetaData.VideoUploadTimestamp
        ));
        
        downloadedVideo.AddResource(new Resource(Guid.CreateVersion7(), request.Video.StorageLocation, request.Video.Timestamp));

        await commandHandler.Handle(SaveChangesCommand.AddSave(downloadedVideo), cancellationToken);
        return downloadedVideo;
    }
}