using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using XingzheExport.Extension;
using XingzheExport.Model.Http.Api.V1.Workout;
using XingzheExport.Model.Sync;
using XingzheExport.Service;
using XingzheExport.Service.Http;

namespace XingzheExport.Console.Service.Command;

internal class SyncCommand(
    ILogger<SyncCommand> logger,
    ISessionIdService sessionIdService,
    ISyncService syncService
    ) : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        logger.LogInformation("开始同步, 共{n}个用户", sessionIdService.All.Count);

        await AnsiConsole.Progress()
            .StartAsync(Process)
            .ConfigureAwait(false);

        return 0;
    }

    private async Task Process(ProgressContext ctx)
    {
        var tasks = sessionIdService.All.Select(x =>
        {
            var task = ctx.AddTask($"{x.Value.Name} 等待同步", false);
            task.Value(0);
            task.MaxValue(0);

            var progress = new Progress<ProgressValue>(x =>
            {
                task.MaxValue(x.Max);
                var cur = x.Current + 1; if (cur >= x.Max) cur = x.Max;
                task.Value(cur);

                string message = string.Empty;

                if (x.Exception != null)
                {
                    message = x.Exception.Message;
                }
                else if (x.WorkoutDetail != null)
                {
                    message = x.WorkoutDetail.ToString();
                }

                if (string.IsNullOrWhiteSpace(message)) message = DateTime.Now.ToString();
                task.Description(message);
            });

            return (x.Key, x.Value.Id, progress);
        });

        foreach (var (sessionId, userId, progress) in tasks.AsParallel().WithDegreeOfParallelism(5))
        {
            await syncService.SyncAsync(sessionId, x => SaveGpxFile(userId, x), progress);
        }
    }

    static Task SaveGpxFile(long userId, WorkoutDetail workoutDetail)
    {
        var path = Path.Combine(XingzheExportContext.ExportDirectory, userId.ToString(), $"{workoutDetail.Id}.gpx");
        return workoutDetail.ToGPXDocument().SaveAsync(path);
    }
}