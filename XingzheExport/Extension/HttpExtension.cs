using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XingzheExport.Extension;

public static class HttpExtension
{
    /// <summary>
    /// 获取Json数据
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<JToken> GetJsonAsync(this HttpClient httpClient, string url)
    {
        var resp = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<JToken>(resp) ?? throw new InvalidOperationException($"Url内容无法解析为Json, Url:{url}");
    }


    public static T GetValue<T>(this JToken token, string path)
    {
        var result = token.SelectToken(path) ?? throw new InvalidOperationException($"未查询到JToken, Path:{path}");
        return result.ToObject<T>() ?? throw new InvalidOperationException($"无法转换JsonV值, 目标类型:{typeof(T)}, Json:{result}");
    }
}
