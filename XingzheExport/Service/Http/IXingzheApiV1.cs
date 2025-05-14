using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnitsNet;
using XingzheExport.Extension;
using XingzheExport.Model.Http.Api.V1;
using XingzheExport.Model.Http.Api.V1.User;
using XingzheExport.Model.Http.Api.V1.Workout;

namespace XingzheExport.Service.Http;

public interface IXingzheApiV1
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns></returns>
    Task<UserInfo> GetUserInfoAsync(string sessionId);

    /// <summary>
    /// 获取包含训练训练的年月
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task<List<YearMonth>> GetYearMonthsAsync(string sessionId);

    /// <summary>
    /// 获取训练记录的简略信息
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <param name="sportType"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    Task<List<WorkoutSummary>> GetWorkoutSummariesAsync(string sessionId, int offset = 0, int limit = 24, SportType? sportType = null, int? year = null, int? month = null);

    /// <summary>
    /// 获取训练详细信息
    /// </summary>
    /// <param name="workoutId"></param>
    /// <returns></returns>
    Task<WorkoutDetail> GetWorkoutDetailAsync(string sessionId, long workoutId);
}


internal class XingzheApiV1(ILogger<XingzheApiV1> logger) : IXingzheApiV1
{
    public async Task<UserInfo> GetUserInfoAsync(string sessionId)
    {
        logger.LogTrace("正在获取用户信息, SessionId:{sessionId}", sessionId);

        const string url = "https://imxingzhe.com/api/v1/user/user_info/";
        var node = await HttpClientFactory.GetFromSessionId(sessionId).GetJsonAsync(url);
        node = node["data"] ?? throw new InvalidOperationException("无法获取用户信息");

        var id = node.GetValue<long>("id");
        var username = node.GetValue<string>("username");

        var userInfo = new UserInfo
        {
            Id = id,
            Name = username,
        };

        logger.LogInformation("用户信息获取完成, {user}", userInfo);
        return userInfo;
    }

    public async Task<List<YearMonth>> GetYearMonthsAsync(string sessionId)
    {
        logger.LogTrace("正在获取训练年月信息, SessionId:{sessionId}", sessionId);

        const string url = "https://imxingzhe.com/api/v1/pgworkout/year_month/";
        var json = await HttpClientFactory.GetFromSessionId(sessionId).GetJsonAsync(url);

        List<YearMonth> yearMonths = [];

        foreach (var yearProperty in json["data"]!.Children<JProperty>())
        {
            YearMonth yearMonth = new()
            {
                Year = int.Parse(yearProperty.Name)
            };

            foreach (int month in ((JArray)yearProperty.Value).Select(v => (int)v))
            {
                yearMonth.Month.Add(month);
            }

            if (yearMonth.Month.Count <= 0) continue;
            yearMonths.Add(yearMonth);
        }


        logger.LogInformation("训练年月信息获取完成:{info}", string.Join(" | ", yearMonths));
        return yearMonths;
    }

    public async Task<List<WorkoutSummary>> GetWorkoutSummariesAsync(string sessionId, int offset = 0, int limit = 24, SportType? sport = null, int? year = null, int? month = null)
    {
        logger.LogTrace("正在获取训练简略信息, SessionId:{session}, Offset:{offset}, Limit:{limit}, Sport:{sport}, Year:{year}, Month:{month}", sessionId, offset, limit, sport, year, month);

        var url = BuildWorkoutsSummaryUrl(offset, limit, sport, year, month);
        var json = await HttpClientFactory.GetFromSessionId(sessionId).GetJsonAsync(url);

        var dataNode = json.SelectToken("data.data") ?? throw new InvalidOperationException("无法获取训练数据");
        List<WorkoutSummary> workoutSummaries = [];

        foreach (var node in dataNode)
        {
            //训练Id
            var id = node.GetValue<long>("id");
            //标题
            var title = node.GetValue<string>("title");
            //训练开始时间 Unix 毫秒 Utc+8
            var beginTime = node.GetValue<long>("start_time");

            //训练类型
            var sportType = node.GetValue<SportType>("sport");


            //平均均速 (千米时)
            var avgSpeed = node.GetValue<double>("avg_speed");
            //距离 (米)
            var distance = node.GetValue<double>("distance");
            //训练时间 (秒)
            var duration = node.GetValue<double>("duration");


            WorkoutSummary workoutSummary = new()
            {
                Id = id,
                Title = title,
                Sport = sportType,
                Timestamp = beginTime.ToBeijingTime(),
                Distance = Length.FromMeters(distance),
                Duration = TimeSpan.FromSeconds(duration),
                AvgSpeed = Speed.FromKilometersPerHour(avgSpeed)
            };

            workoutSummaries.Add(workoutSummary);
        }


        logger.LogInformation("训练年月信息获取完成, 数量:{count}", workoutSummaries.Count);
        return workoutSummaries;
        
    }

    public async Task<WorkoutDetail> GetWorkoutDetailAsync(string sessionId, long workoutId)
    {
        logger.LogTrace("正在获取训练详情, SessionId:{sessionId}", sessionId);

        var url = BuildWorkoutDetailUrl(workoutId);
        var node = await HttpClientFactory.GetFromSessionId(sessionId).GetJsonAsync(url);
        node = node.SelectToken("data.workout") ?? throw new InvalidOperationException("无法获取训练详情");

        //标题
        var title = node.GetValue<string>("title");
        //开始时间 (Unix Utc+8)
        var start_time = node.GetValue<long>("start_time");
        //开始时间 (Unix Utc+8)
        var end_time = node.GetValue<long>("end_time");
        //训练类型
        var sport = node.GetValue<SportType>("sport");

        //平均海拔 (米)
        var avg_altitude = node.GetValue<double>("avg_altitude");
        //平均踏频 (次/分钟)
        var avg_cadence = node.GetValue<double>("avg_cadence");
        //平均心率 (次/分钟)
        var avg_heartrate = node.GetValue<double>("avg_heartrate");
        //平均速度 (千米/时)
        var avg_speed = node.GetValue<double>("avg_speed");

        //最大海拔 (米)
        var max_altitude = node.GetValue<double>("max_altitude");
        //最大踏频  (次/分钟)
        var max_cadence = node.GetValue<double>("max_cadence");
        //最大心率  (次/分钟)
        var max_heartrate = node.GetValue<double>("max_heartrate");
        //最大速度 (千米/时)
        var max_speed = node.GetValue<double>("max_speed");

        //卡路里 (千卡)
        var calories = node.GetValue<int>("calories");
        //总距离 (米)
        var distance = node.GetValue<int>("distance");
        //总时间 (秒)
        var duration = node.GetValue<int>("duration");


        //采样点
        var trackPoints = await GetTrackPointsAsync(sessionId, workoutId);


        WorkoutDetail workoutDetail = new()
        {
            Id = workoutId,
            Title = title,
            BeginTime = start_time.ToBeijingTime(),
            FinishTime = end_time.ToBeijingTime(),
            Sport = sport,
            TrackPoints = trackPoints,

            AvgAltitude = Length.FromMeters(avg_altitude),
            AvgCadence = Frequency.FromCyclesPerMinute(avg_cadence),
            AvgHeartrate = Frequency.FromBeatsPerMinute(avg_heartrate),
            AvgSpeed = Speed.FromKilometersPerHour(avg_speed),

            MaxAltitude = Length.FromMeters(max_altitude),
            MaxCadence = Frequency.FromCyclesPerMinute(max_cadence),
            MaxHeartrate = Frequency.FromBeatsPerMinute(max_heartrate),
            MaxSpeed = Speed.FromKilometersPerHour(max_speed),

            Calories = Energy.FromKilocalories(calories),
            Distance = Length.FromMeters(distance),
            Duration = TimeSpan.FromSeconds(duration)
        };

        logger.LogInformation("训练详情获取完成:{info}", workoutDetail);
        return workoutDetail;
    }


    /// <summary>
    /// 获取训练采样点
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="workoutId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<WorkoutTrackPoint>> GetTrackPointsAsync(string sessionId, long workoutId)
    {
        logger.LogTrace("正在获取训练采样节点, SessionId:{sessionId}", sessionId);

        string url = $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/stream/";
        var json = await HttpClientFactory.GetFromSessionId(sessionId).GetJsonAsync(url);

        List<WorkoutTrackPoint> trackPoints = [];

        var dataNode = json["data"] ?? throw new InvalidOperationException("无法获取训练采样数据");

        //海拔高度 (米)
        var altitudes = dataNode.GetValue<int[]>("altitude");
        
        //踏频 (次/分钟)
        var cadences = dataNode.GetValue<int[]>("cadence");
        //心率  (次/分钟)
        var heartrates = dataNode.GetValue<int[]>("heartrate");
        //温度 (摄氏度℃)
        var temperatures = dataNode.GetValue<int[]>("temperature");

        //距离 (米)
        var distance = dataNode.GetValue<float[]>("distance");
        //速度 (米/秒)
        var speeds = dataNode.GetValue<float[]>("speed");

        //左平衡
        var left_balances = dataNode.GetValue<int[]>("left_balance");
        //右平衡
        var right_balances = dataNode.GetValue<int[]>("right_balance");
        //功率
        var powers = dataNode.GetValue<int[]>("power");

        //Unix时间戳 (毫秒Utc+0)
        var timestamps = dataNode.GetValue<long[]>("timestamp");

        //位置采样点
        var trackPointBases = await GetTrackPointBasesAsync(sessionId, workoutId);



        int count = timestamps.Length;
        for (int i = 0; i < count; i++)
        {
            var basePoint = trackPointBases.ElementAtOrDefault(i);
            if (basePoint is null) continue;

            trackPoints.Add(new WorkoutTrackPoint
            {
                Latitude = basePoint.Latitude,
                Longitude = basePoint.Longitude,
                Heartrate = Frequency.FromBeatsPerMinute(heartrates.ElementAtOrDefault(i)),
                Altitude = Length.FromMeters(altitudes.ElementAtOrDefault(i)),
                Distance = Length.FromMeters(distance.ElementAtOrDefault(i)),
                Cadence = Frequency.FromCyclesPerMinute(cadences.ElementAtOrDefault(i)),
                Temperature = Temperature.FromDegreesCelsius(temperatures.ElementAtOrDefault(i)),
                Speed = Speed.FromKilometersPerHour(speeds.ElementAtOrDefault(i)),
                Power = Power.FromWatts(powers.ElementAtOrDefault(i)),
                Timestamp = timestamps[i].ToBeijingTime()
            });
        }


        logger.LogInformation("训练采样节点获取完成, 总数:{count}", trackPoints.Count);
        return trackPoints;
        
    }

    /// <summary>
    /// 获取位置采样点
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="workoutId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<WorkoutTrackPointBase>> GetTrackPointBasesAsync(string sessionId, long workoutId)
    {
        logger.LogTrace("正在获取训练位置信息, SessionId:{sessionId}", sessionId);

        string url = $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/gpx/";
        var xmlContent = await HttpClientFactory.GetFromSessionId(sessionId).GetStringAsync(url);
        XDocument xml = XDocument.Parse(xmlContent);

        var elements = xml.XPathSelectElements("//*[local-name()='trkpt']");
        if (elements is null) return [];


        List<WorkoutTrackPointBase> trackPointBases = [];

        foreach (XElement element in elements)
        {
            string latString = element.Attribute("lat")?.Value ?? "0.0";
            string lonString = element.Attribute("lon")?.Value ?? "0.0";
            string timeString = element.Value ?? throw new InvalidOperationException("无法解析的时间");

            var lat = double.Parse(latString);
            var lon = double.Parse(lonString);
            var time = Convert.ToDateTime(timeString) - TimeSpan.FromHours(8);

            trackPointBases.Add(new WorkoutTrackPointBase()
            {
                Latitude = lat,
                Longitude = lon,

                // 修复了 GitHub Issue #1：行者导出的GPX时间为北京时间但时区用的是UTC
                // 参考：https://github.com/DaThabe/XingzheExport/issues/1
                Timestamp = new DateTimeOffset(time, TimeSpan.FromHours(8))
            });
        }

        logger.LogInformation("训练位置信息获取完成, 采样点数:{count}", trackPointBases.Count);
        return trackPointBases;
    }

    /// <summary>
    /// 构建训练记录的简略信息的Url
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <param name="sport"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    private static string BuildWorkoutsSummaryUrl(int offset = 0, int limit = 24, SportType? sport = null, int? year = null, int? month = null)
    {
        const string baseUrl = "https://www.imxingzhe.com/api/v1/pgworkout/";
        List<string> @params = [];

        @params.Add($"offset={offset}");
        @params.Add($"limit={limit}");

        if (sport != null) @params.Add($"sport={(int)sport}");
        if (year != null) @params.Add($"year={year}");
        if (month != null) @params.Add($"year={month}");

        return @params.Count == 0 ? baseUrl : $"{baseUrl}?{string.Join('&', @params)}";
    }

    /// <summary>
    /// 构建训练记录的详细信息的Url
    /// </summary>
    /// <param name="workoutId"></param>
    /// <param name="segments"></param>
    /// <param name="slopes"></param>
    /// <param name="pois"></param>
    /// <param name="laps"></param>
    /// <returns></returns>
    private static string BuildWorkoutDetailUrl(long workoutId, bool? segments = null, bool? slopes = null, bool? pois = null, bool? laps = null)
    {
        string baseUrl = $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/";
        List<string> @params = [];


        if (segments != null) @params.Add($"segments={segments}");
        if (slopes != null) @params.Add($"slopes={slopes}");
        if (pois != null) @params.Add($"pois={pois}");
        if (laps != null) @params.Add($"pois={laps}");

        return @params.Count == 0 ? baseUrl : $"{baseUrl}?{string.Join('&', @params)}";
    }
}