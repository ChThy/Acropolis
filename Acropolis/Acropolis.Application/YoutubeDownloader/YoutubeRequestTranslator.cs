//using Acropolis.Application.Events;
//using MediatR;
//using Microsoft.Extensions.Options;

//namespace Acropolis.Application.YoutubeDownloader;

//public class YoutubeRequestTranslator : IRequestCommandTranslator
//{
//    private readonly YoutubeSettings youtubeSettings;

//    public YoutubeRequestTranslator(IOptions<YoutubeSettings> options)
//    {
//        youtubeSettings = options.Value;
//    }

//    public bool CanHandle(RequestReceived request)
//    {
//        string? message = request?.Request?.Message;
//        if (message is null)
//            return false;
//        return IsRetryRequest(message) || youtubeSettings.ValidUrls.Any(e =>
//            message.StartsWith(e, StringComparison.InvariantCultureIgnoreCase));
//    }

//    private static bool IsRetryRequest(string request) => request.Equals("retry", StringComparison.OrdinalIgnoreCase);

//    public IRequest CreateCommand(RequestReceived request)
//    {
//        string message = request.Request.Message!;
//        if (IsRetryRequest(message))
//        {
//            return new RetryFailedDownloadsCommand(request.Id);
//        }
//        else
//        {
//            return new DownloadYoutubeVideoCommand(request.Id, message);
//        }
//    }
//}