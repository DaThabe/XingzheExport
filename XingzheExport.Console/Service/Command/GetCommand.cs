using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using XingzheExport.Extension;
using XingzheExport.Service.Http;

namespace XingzheExport.Console.Service.Command;


internal class GetCommand(
    ILogger<GetCommand> logger,
    ISessionIdService sessionIdService,
    IXingzheApiV1 api
    ) : AsyncCommand<ExportCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ExportCommandSettings settings)
    {
        if (sessionIdService.All.Count > 1 && settings.UserId == null)
        {
            logger.LogWarning("当前有多个用户SessionId, 需要指定--userId=[id]");

            Table table = new();
            table.AddColumn("Id");
            table.AddColumn("名称");

            foreach (var i in sessionIdService.All) table.AddRow(i.Value.Id.ToString(), i.Value.Name);
            return -1;
        }

        var userId = settings.UserId ?? sessionIdService.All.FirstOrDefault().Value.Id;
        var session = sessionIdService.FindByUserId(userId);

        var detail = await api.GetWorkoutDetailAsync(session, settings.WorkoutId);
        
        if (settings.Format.Equals("gpx", StringComparison.CurrentCultureIgnoreCase))
        {
            var savePath = Path.Combine(settings.Path, $"{detail.Id}.gpx");
            await detail.ToGPXDocument().SaveAsync(savePath);

            logger.LogInformation("已导出Gpx, 路径:{path}", savePath);
        }
        else
        {
            var savePath = Path.Combine(settings.Path, $"{detail.Id}.tcx");
            await detail.ToTCXDocument().SaveAsync(savePath);

            logger.LogInformation("已导出Tcx, 路径:{path}", savePath);
        }

        return 0;
    }

    public override ValidationResult Validate(CommandContext context, ExportCommandSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Path))
        {
            return ValidationResult.Error("参数 --path 不可为空");
        }

        if(string.IsNullOrWhiteSpace(settings.Format))
        {
            return ValidationResult.Error("参数 --format 不可为空");
        }

        if (sessionIdService.All.Count > 1 && settings.UserId == null)
        {
            return ValidationResult.Error("参数 --userId 不可为空");
        }

        return ValidationResult.Success();
    }
}

//xzexp 123456 path [--userid 123456] [--format gpx]
internal class ExportCommandSettings : CommandSettings
{
    [CommandArgument(0, "<workoutId>")]
    public long WorkoutId { get; set; }

    [CommandArgument(1, "<path>")]
    public required string Path { get; set; }


    [CommandOption("-u|--userId <userId>")]
    public long? UserId { get; set; }


    [CommandOption("-f|--format <format>")]
    public string Format { get; set; } = "gpx";
}