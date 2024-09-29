using Acropolis.Application.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Acropolis.Application.Tests;

public class UnitTest1
{
    [Fact(Skip = "DebugTest")]
    public async Task Test1()
    {
        var videoPath = @"D:\GitRepositories\Acropolis\Acropolis\Acropolis.Api\data\downloads\youtubedownloader\Milan Jovanović\VideoPart.20240917_Binary Search Algorithm in C#.mp4";
        var audioPath = @"D:\GitRepositories\Acropolis\Acropolis\Acropolis.Api\data\downloads\youtubedownloader\Milan Jovanović\AudioPart.20240917_Binary Search Algorithm in C#.mp4";
        var outputPath = @"D:\GitRepositories\Acropolis\Acropolis\Acropolis.Api\data\downloads\youtubedownloader\Milan Jovanović\20240917_Binary Search Algorithm in C#.mp4";

        var command = $"ffmpeg";
        var arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -c:a aac \"{outputPath}\"";

        var processService = new ProcessService(NullLogger<ProcessService>.Instance);
        
        var result = await processService.RunProcessAsync(command, arguments);

    }
}