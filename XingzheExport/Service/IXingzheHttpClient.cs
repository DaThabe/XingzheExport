//using Flurl.Http;
//using Microsoft.Extensions.Configuration;
//using System.Net.Http;
//using System.Xml;
//using XingzheExport.Extension;
//using XingzheExport.Model.Http.Api.V1;
//using XingzheExport.Model.Math;
//using XingzheExport.Model.Workout;

//namespace XingzheExport.Service;



///// <summary>
///// 行者API
///// </summary>
//public interface IXingzheHttpClient
//{
//    /// <summary>
//    /// 用户信息
//    /// </summary>
//    UserInfo UserInfo { get; }

//    /// <summary>
//    /// 获取月训练信息
//    /// </summary>
//    /// <param name="userId">用户Id</param>
//    /// <param name="year">训练年份</param>
//    /// <param name="month">训练月份</param>
//    /// <returns>整个月的训练信息</returns>
//    Task<MonthlyWorkout> GetMonthlyWorkoutAsync(int year, int month);

//    /// <summary>
//    /// 获取训练明细
//    /// </summary>
//    /// <param name="workoutId">训练Id</param>
//    /// <returns></returns>
//    Task<WorkoutDetail> GetWorkoutDetailAsync(long workoutId);
//}


///// <summary>
///// 行者 Http 请求客户端
///// </summary>
//internal class XingzheHttpClient : IXingzheHttpClient
//{
//    private XingzheHttpClient() { }

//    public static async Task<XingzheHttpClient> GetInstance(string cookie)
//    {
//        var httpClient = new HttpClient();
//        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
//        httpClient.DefaultRequestHeaders.Add("Cookie", cookie);

        
        

//        return new XingzheHttpClient()
//        {
//            HttpClient = httpClient,
//            UserInfo = userInfo
//        };
//    }




//    public required UserInfo UserInfo { get; set; }

//    public required HttpClient HttpClient { get; set; }


//    public async Task<UserInfo> UpdateUserInfo()
//    {
//        string url = "https://www.imxingzhe.com/api/v4/account/get_user_info/";
//        dynamic root = await HttpClient.GetJsonAsync(url);

//        UserInfo = new UserInfo()
//        {
//            Id = root.userid,
//            Name = root.username,
//            Email = root.email,
//            Level = (int)root.ulevel,
//            TotalMileage = Length.FromMetre((long)root.total_distance)
//        };

//        return UserInfo;
//    }

//    public async Task<MonthlyWorkout> GetMonthlyWorkoutAsync(int year, int month)
//    {
//        string url = $"https://www.imxingzhe.com/api/v4/user_month_info/?user_id={UserInfo.Id}&year={year}&month={month}";

//        dynamic? root = await XingzheHttpClient.Get(cookie).GetJsonAsync(url) ??
//            throw new HttpRequestException("月训练信息获取失败, 请检查 Cookie 是否失效");

//        return new(root);
//    }
    
//    public async Task<WorkoutDetail> GetWorkoutDetailAsync(long workoutId)
//    {
//        try
//        {
//            //下载 gpx
//            string gpx_url = $"http://www.imxingzhe.com/xing/{workoutId}/gpx";
//            using var gpx_stream = await XingzheHttpClient.Get(cookie).GetStreamAsync(gpx_url);

//            XmlDocument gpx_xml = new();
//            gpx_xml.Load(gpx_stream);


//            //下载外设数据 json
//            string peripheral_url = $"https://www.imxingzhe.com/api/v4/segment_points/?workout_id={workoutId}";
//            dynamic? peripheral_root = await XingzheHttpClient.Get(cookie).GetJsonAsync(peripheral_url);


//            return new(gpx_xml, peripheral_root);
//        }
//        catch (Exception ex)
//        {
//            throw new HttpRequestException("训练信息获取失败, 请检查 Cookie 是否失效", ex);
//        }
//    }
//}