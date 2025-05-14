using Microsoft.Extensions.DependencyInjection;
using XingzheExport.Service;
using XingzheExport.Service.Http;

namespace XingzheExport;

public static class Services
{
    /// <summary>
    /// 添加行者导出服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXingzheExport(this IServiceCollection services)
    {
               //行者 Api
        services.AddSingleton<IXingzheApiV1, XingzheApiV1>();

        //Session Id
        services.AddSingleton<ISessionIdService, SessionIdService>();
        services.AddHostedService(x => x.GetRequiredService<ISessionIdService>());

        //Session Id
        services.AddSingleton<ISyncService, SyncService>();
        services.AddHostedService(x => x.GetRequiredService<ISyncService>());

        return services;
    }
}
