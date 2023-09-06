using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Sporter.API.XingZhe.Data;
using System.Xml;

namespace XingzheExport;



/// <summary>
/// 行者API
/// </summary>
public static class XingzheAPI
{
    /// <summary>
    /// 获取用户ID
    /// </summary>
    /// <param name="cookie">Cookie</param>
    /// <exception cref="HttpRequestException">请求失败</exception>
    public static async Task<UserInfo> GetUserInfoAsync(string cookie)
    {
        string url = "https://www.imxingzhe.com/api/v4/account/get_user_info/";

        dynamic? root = await XingzheHttpClient.Get(cookie).GetJsonAsync(url) ??
            throw new HttpRequestException("用户信息获取失败, 请检查 Cookie 是否失效");

        return new(root);
    }

    /// <summary>
    /// 获取月锻炼信息
    /// </summary>
    /// <param name="cookie">Cookie</param>
    /// <param name="userId">用户Id</param>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <exception cref="ArgumentNullException">请求失败</exception>
    public static async Task<MonthWorkoutIInfo> GetMonthWorkoutInfoAsync(string cookie, long userId, int year, int month)
    {
        string url = $"https://www.imxingzhe.com/api/v4/user_month_info/?user_id={userId}&year={year}&month={month}";

        dynamic? root = await XingzheHttpClient.Get(cookie).GetJsonAsync(url) ?? 
            throw new HttpRequestException("月锻炼信息获取失败, 请检查 Cookie 是否失效");

        return new(root);
    }

    /// <summary>
    /// 获取锻炼信息
    /// </summary>
    /// <param name="cookie">Cookie</param>
    /// <param name="workoutId">锻炼数据Id</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">请求失败</exception>
    public static async Task<WorkoutInfo> GetWorkoutInfoAsync(string cookie, long workoutId)
    {
        try
        {
            //下载 gpx
            string gpx_url = $"http://www.imxingzhe.com/xing/{workoutId}/gpx";
            using var gpx_stream = await XingzheHttpClient.Get(cookie).GetStreamAsync(gpx_url);

            XmlDocument gpx_xml = new();
            gpx_xml.Load(gpx_stream);


            //下载外设数据 json
            string peripheral_url = $"https://www.imxingzhe.com/api/v4/segment_points/?workout_id={workoutId}";
            dynamic? peripheral_root = await XingzheHttpClient.Get(cookie).GetJsonAsync(peripheral_url);


            return new(gpx_xml, peripheral_root);
        }
        catch(Exception ex)
        {
            throw new HttpRequestException("锻炼信息获取失败, 请检查 Cookie 是否失效", ex);
        }
    }
}


/// <summary>
/// 行者 Http 请求客户端
/// </summary>
file static class XingzheHttpClient
{
    /// <summary>
    /// 浏览器标识
    /// </summary>
    public static string ChromeUserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";

    /// <summary>
    /// 所有 HttpClient
    /// </summary>
    private static readonly Dictionary<string, HttpClient> _HttpClients = new();



    /// <summary>
    /// 获取一个 HttpClient 如果指定 Cookie 已经创建 则直接返回
    /// </summary>
    /// <param name="cookie">cookie 字符串</param>
    public static HttpClient Get(string cookie)
    {
        _HttpClients.TryGetValue(cookie, out var httpClient);

        if (httpClient == null)
        {
            _HttpClients[cookie] = new HttpClient();
            _HttpClients[cookie].DefaultRequestHeaders.Add("User-Agent", ChromeUserAgent);
            _HttpClients[cookie].DefaultRequestHeaders.Add("Cookie", cookie);
        }

        return _HttpClients[cookie];
    }


    /// <summary>
    /// 获取Json并转为<see cref="JToken"/>
    /// </summary>
    /// <param name="url">请求 Url</param>
    public static async Task<JToken?> GetJsonAsync(this HttpClient client, string url)
    {
        try
        {
            var json_text = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<JToken>(json_text);
        }
        catch
        {
            return null;
        }
    }
}