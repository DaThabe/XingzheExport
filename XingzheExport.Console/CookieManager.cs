using Newtonsoft.Json;

namespace XingzheExport.Console;


/// <summary>
/// 用户 Cookie 管理
/// </summary>
internal static class CookieManager
{
    private static Dictionary<string, string> _Cookies = new();

    public static IEnumerable<(string Name, string Cookie)> All => from i in _Cookies select (i.Key, i.Value);



    /// <summary>
    /// 设置一个 Cookie 如果 Cookie所属用户存在就替换之前的
    /// </summary>
    public static void Set(string cookie, out string? Username, bool save = true)
    {
        try
        {
            var info = XingzheAPI.GetUserInfoAsync(cookie).Result;
            _Cookies[info.Name] = cookie;

            Username = info.Name;
            if (save) _ = SaveAsync();
        }
        catch(HttpRequestException)
        {
            System.Console.WriteLine("Cookie 有问题, 无法请求行者服务器");
            Username = null;
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"出问题了, {ex}");
            Username = null;
        }
    }

    /// <summary>
    /// 用户名
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public static bool Remove(string username, bool save = true)
    {
        var result = _Cookies.Remove(username);

        if(result && save)
        {
            _ = SaveAsync();
        }

        return result;
    }

    /// <summary>
    /// 保存 Cookie 数据
    /// </summary>
    public static async Task<bool> SaveAsync()
    {
        try
        {
            var json_text = JsonConvert.SerializeObject(_Cookies);
            await File.WriteAllTextAsync("Cookies", json_text);

            return true;
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"保存失败: {ex}");
            return false;
        }
    }

    /// <summary>
    /// 重新加载
    /// </summary>
    public static async Task<bool> ReloadAsync()
    {
        try
        {
            var json_text = await File.ReadAllTextAsync("Cookies");

            var cookie = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_text)
                ?? throw new Exception("Cookie 文件结构不符合预期, 建议删除重新添加");

            _Cookies = cookie;
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"加载失败: {ex}");
            return false;
        }
    }


    
}