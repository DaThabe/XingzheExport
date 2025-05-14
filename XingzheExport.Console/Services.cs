using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using XingzheExport.Console.Service;
using XingzheExport.Console.Service.Command;

namespace XingzheExport.Console;

internal static class Services
{
    public static IServiceCollection AddCommand(this IServiceCollection services)
    {
        services.AddSingleton<SyncCommand>();
        services.AddSingleton<GetCommand>();
        services.AddSingleton<OpenCommand>();

        services.AddSingleton<ICommandApp>(x =>
        {
            var typeRegistrar = new TypeRegistrar(x);
            var app = new CommandApp(typeRegistrar);

            app.Configure(config =>
            {
                config.SetApplicationName("xzexp");
                config.SetApplicationVersion("1.0.0");

                config.SetExceptionHandler((ex, ioc) =>
                {
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);
                    return -1;
                });

                config.AddCommand<GetCommand>("get")
                    .WithDescription("获取某个训练数据")
                    .WithExample("123456", @"C:\Path", "[--userId=123456]", "[--format=<gpx|tcx>]");
                
                config.AddCommand<SyncCommand>("sync")
                    .WithDescription("同步所有训练数据到本地目录");
                
                config.AddCommand<OpenCommand>("open")
                    .WithDescription("打开数据同步目录");
            });

            return app;
        });

        services.AddHostedService<CommandHostedService>();

        return services;
    }
}
