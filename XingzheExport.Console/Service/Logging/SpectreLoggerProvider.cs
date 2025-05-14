using Microsoft.Extensions.Logging;

namespace XingzheExport.Console.Service.Logging;

public class SpectreLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new SpectreLogger(categoryName);
    public void Dispose() { }
}
