using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingzheExport.Console.Service.Logging;

internal class SpectreLogger(string categoryName) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception).EscapeMarkup();
        var timestamp = $"[white][[{DateTime.Now:HH:mm:ss}]][/]";

        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                AnsiConsole.MarkupLine($"{timestamp} [grey]{message}[/]");
                break;
            case LogLevel.Information:
                AnsiConsole.MarkupLine($"{timestamp} [green]{message}[/]");
                break;
            case LogLevel.Warning:
                AnsiConsole.MarkupLine($"{timestamp} [yellow]{message}[/]");
                break;
            case LogLevel.Error:
            case LogLevel.Critical:
                AnsiConsole.MarkupLine($"{timestamp} [bold red]{message}[/]");
                break;
            default:
                AnsiConsole.WriteLine($"{timestamp} {message}");
                break;
        }
    }
}
