using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XingzheExport.Model.Http.Api.V1.User;

namespace XingzheExport.Service.Http;


/// <summary>
/// Session Id 业务
/// </summary>
public interface ISessionIdService : IHostedService
{
    /// <summary>
    /// 所有 SessionId
    /// </summary>
    IReadOnlyDictionary<string, UserInfo> All { get; }

    /// <summary>
    /// 添加 SessionId
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task<UserInfo> SetAsync(string sessionId);

    /// <summary>
    /// 根据用户Id查询SessionId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    string FindByUserId(long userId);
}

internal class SessionIdService(
    ILogger<SessionIdService> logger,
    IConfiguration configuration,
    IXingzheApiV1 api
    ) : ISessionIdService
{
    public IReadOnlyDictionary<string, UserInfo> All => _value;

    public async Task<UserInfo> SetAsync(string sessionId)
    {
        logger.LogTrace("正在添加SessionId: {id}", sessionId);

        try
        {
            var userInfo = await api.GetUserInfoAsync(sessionId);
            _value[sessionId] = userInfo;

            logger.LogInformation("SessionId已添加, SessionId:{id}, User:{user}", sessionId, userInfo);
            return userInfo;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, "添加失败, 无效SessionId:{id}", sessionId);
            _value.Remove(sessionId);

            throw;
        }
    }

    public string FindByUserId(long userId)
    {
        return _value.FirstOrDefault(x => x.Value.Id == userId).Key ?? throw new InvalidOperationException($"查询用户不存在, Id:{userId}");
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var sessionIds = configuration.GetSection("Api:SessionIds").Get<string[]>();

        if (sessionIds is null || sessionIds.Length <= 0)
        {
            logger.LogWarning("配置文件未包含用户SessionId");
            return;
        }

        foreach (var sessionId in sessionIds)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) continue;

            try
            {
                _value[sessionId] = await api.GetUserInfoAsync(sessionId);
                logger.LogInformation("SessionId 验证成功: {value}", sessionId);
            }
            catch
            {
                logger.LogWarning("SessionId 失效: {value}", sessionId);
            }
        }
    }

    /// <summary>
    /// 停止
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    private readonly Dictionary<string, UserInfo> _value = [];
}
