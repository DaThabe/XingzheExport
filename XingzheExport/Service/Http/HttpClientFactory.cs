using System.Net;

namespace XingzheExport.Service.Http;

/// <summary>
/// HttpClient 工厂
/// </summary>
internal class HttpClientFactory
{
    /// <summary>
    /// 根据参数获取 HttpClient, 如果存在则复用, 否则创建新的 HttpClient
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public static HttpClient GetFromSessionId(string sessionId)
    {
        var hashCode = HashCode.Combine(sessionId);

        if(_clients.TryGetValue(hashCode, out var client))
        {
            return client;
        }

        //添加Cookie
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Uri("https://imxingzhe.com/"), new Cookie
        {
            Name = "sessionid",
            Value = sessionId,
            Path = "/",
            Domain = "imxingzhe.com",
            HttpOnly = true,
            Expires = DateTime.Now.AddMonths(1),
        });

        //创建客户端
        var httpClient = new HttpClient(new HttpClientHandler
        {
            CookieContainer = cookieContainer
        });
        _clients[hashCode] = httpClient;

        return httpClient;
    }

    /// <summary>
    /// 根据参数删除 HttpClient, 释放资源
    /// </summary>
    /// <param name="sessionId"></param>
    public static void Remove(string sessionId)
    {
        var hashCode = HashCode.Combine(sessionId);
        _clients.Remove(hashCode);
    }


    private static readonly Dictionary<int, HttpClient> _clients = [];
}