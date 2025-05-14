using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace XingzheExport.Console.Service;

internal class CommandHostedService(
    ILogger<CommandHostedService> logger,
    ICommandApp commandApp) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var args = System.Console.ReadLine()?.Split([' ', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries);
                if (args == null || args.Length <= 0)
                {
                    logger.LogWarning("请输入有效代码");
                    continue;
                }

                await commandApp.RunAsync(args);
            }

        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
