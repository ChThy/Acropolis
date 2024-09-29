using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Acropolis.Application.Services;

public class ProcessService
{
    // See: https://gist.github.com/georg-jung/3a8703946075d56423e418ea76212745
    
    private readonly ILogger<ProcessService> logger;

    public ProcessService(ILogger<ProcessService> logger)
    {
        this.logger = logger;
    }
    
    public async Task<ProcessResult> RunProcessAsync(string command, string arguments, int timeout = -1)
    {
        var result = new ProcessResult();

        using var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        //process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;

        var outputBuilder = new StringBuilder();
        var outputCloseEvent = new TaskCompletionSource<bool>();

        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data == null)
            {
                outputCloseEvent.SetResult(true);
            }
            else
            {
                outputBuilder.Append(e.Data);
                logger.LogTrace(e.Data);
            }
        };

        var errorBuilder = new StringBuilder();
        var errorCloseEvent = new TaskCompletionSource<bool>();

        process.ErrorDataReceived += (s, e) =>
        {
            if (e.Data == null)
            {
                errorCloseEvent.SetResult(true);
            }
            else
            {
                errorBuilder.Append(e.Data);
                logger.LogTrace(e.Data);
            }
        };
        
        logger.LogInformation("Starting process {process} with arguments {arguments}", command, arguments);
        var isStarted = process.Start();
        if (!isStarted)
        {
            result.ExitCode = process.ExitCode;
            return result;
        }

        // Reads the output stream first and then waits because deadlocks are possible
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // Creates task to wait for process exit using timeout
        var waitForExit = WaitForExitAsync(process, timeout);

        logger.LogDebug("Waiting for process to exit");
        
        // Create task to wait for process exit and closing all output streams
        var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

        // Waits process completion and then checks it was not completed by timeout
        if (await Task.WhenAny(Task.Delay(timeout), processTask) == processTask && waitForExit.Result)
        {
            result.ExitCode = process.ExitCode;
            result.Output = outputBuilder.ToString();
            result.Error = errorBuilder.ToString();
            
            logger.LogInformation("Process exited with code {exitCode}", result.ExitCode);
        }
        else
        {
            try
            {
                // Kill hung process
                process.Kill();
                
                logger.LogWarning("Process is hanging. Killing process {process} with arguments {arguments}", command, arguments);
            }
            catch
            {
                logger.LogError("Failed to kill process {process} with arguments {arguments}", command, arguments);
            }
        }

        return result;
    }


    private static Task<bool> WaitForExitAsync(Process process, int timeout)
    {
        return Task.Run(() => process.WaitForExit(timeout));
    }


    public record struct ProcessResult
    {
        public int? ExitCode;
        public string Output;
        public string Error;
    }
}