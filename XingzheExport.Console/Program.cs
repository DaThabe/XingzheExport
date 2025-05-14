using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using XingzheExport;
using XingzheExport.Console;
using XingzheExport.Console.Service.Logging;


var appHost = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(x=>
    {
        x.SetBasePath(AppContext.BaseDirectory);
        x.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureLogging(x =>
    {
        x.ClearProviders();
        x.AddProvider(new SpectreLoggerProvider());
    })
    .ConfigureServices(x => x.AddXingzheExport().AddCommand())
    .Build();

await appHost.StartAsync();
await appHost.Services.GetRequiredService<ICommandApp>().RunAsync(args);
await appHost.StopAsync();