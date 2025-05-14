using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.Diagnostics;
using XingzheExport.Console.Extension;

namespace XingzheExport.Console.Service.Command;

internal class OpenCommand(ILogger<OpenCommand> logger) : Spectre.Console.Cli.Command
{
    public override int Execute(CommandContext context)
    {
        var path = XingzheExportContext.ExportDirectory;
        if (!Directory.Exists(path)) throw new InvalidOperationException("目录不存在");

        using Process process = path.OpenFolder();
        logger.LogInformation("导出目录已打开");

        return 0;
    }
}