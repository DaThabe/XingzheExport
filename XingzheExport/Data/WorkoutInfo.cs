using System.Xml;
using XingzheExport.Math;

namespace XingzheExport.Data;



/// <summary>
/// 锻炼信息
/// </summary>
public partial class WorkoutInfo
{
    /// <summary>
    /// 锻炼记录标题
    /// </summary>
    public string Title { get; } = "";

    /// <summary>
    /// 锻炼开始时间
    /// </summary>
    public DateTime Time { get; } = DateTime.MinValue;

    /// <summary>
    /// 锻炼信息节点
    /// </summary>
    public Point[] Points { get; } = Array.Empty<Point>();



    /// <summary>
    /// 转为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Title} - {Time}";
    }
}

/// <summary>
/// 锻炼信息解析
/// </summary>
public partial class WorkoutInfo
{
    public WorkoutInfo() { }

    /// <summary>
    /// 将服务器返回的 GPX 和 外设数据节点 Json 根节点 合并
    /// </summary>
    /// <param name="gpxDocument">GPX 文档</param>
    /// <param name="peripheralRoot">外设 Json 根节点</param>
    /// <exception cref="ArgumentNullException">这个锻炼信息没有路径</exception>
    internal WorkoutInfo(XmlDocument gpxDocument, dynamic? peripheralRoot = null)
    {
        Title = gpxDocument?["gpx"]?["name"]?.InnerText ?? "";

        var time_text = gpxDocument?["gpx"]?["time"]?.InnerText;
        Time = time_text is null ? DateTime.Now : DateTime.Parse(time_text);

        var nodes = gpxDocument?["gpx"]?["trk"]?["trkseg"]?.ChildNodes;
        if (nodes is null)
        {
            throw new ArgumentNullException(nameof(nodes), "没有运动数据节点");
        }


        List<Point> points = ParserGPXPoint(nodes);
        if (peripheralRoot != null) SetPeripheralInfo(ref points, peripheralRoot);

        Points = points.ToArray();
    }



    //读取GPX文件中的数据
    private static List<Point> ParserGPXPoint(XmlNodeList racks_node)
    {
        List<Point> all = new();

        foreach (XmlNode node in racks_node)
        {
            _ = double.TryParse(node.Attributes?["lat"]?.InnerText, out var lat);
            _ = double.TryParse(node.Attributes?["lon"]?.InnerText, out var lon);
            _ = double.TryParse(node?["ele"]?.InnerText, out var ele);


            var time_text = node?["time"]?.InnerText;
            var Time = time_text is null ? DateTime.Now : DateTime.Parse(time_text);


            all.Add(new()
            {
                Altitude = ele,
                Time = Time,
                Latitude = lat,
                Longitude = lon
            });
        }

        return all;
    }

    //设置外设数据
    private static void SetPeripheralInfo(ref List<Point> point, dynamic root)
    {
        Dictionary<DateTime, Point> cache = 
            new(from i in point select new KeyValuePair<DateTime, Point>(i.Time, i));


        foreach(var i in root.points)
        {
            //不知道为什么是UTC+16
            var time = XingzheUtil.UnixAsDateTime((long)i.time, TimeSpan.FromHours(16));

            cache.TryGetValue(time, out var p);
            if (p == null) continue;

            cache[time].Peripheral = new()
            {
                Heartrate = i.heartrate,
                Power = i.power,
                Cadence = i.cadence,
                Speed = Speed.FromSecond(Length.FromMetre((double)i.speed)),
            };

        }
    }
}

/// <summary>
/// 锻炼信息定义
/// </summary>
public partial class WorkoutInfo
{
    /// <summary>
    /// 锻炼信息节点
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 维度
        /// </summary>
        public required double Latitude { get; init; }

        /// <summary>
        /// 经度
        /// </summary>
        public required double Longitude { get; init; }

        /// <summary>
        /// 海拔高度
        /// </summary>
        public required double Altitude { get; init; }

        /// <summary>
        /// 时间
        /// </summary>
        public required DateTime Time { get; init; }

        /// <summary>
        /// 外设信息
        /// </summary>
        public PeripheralInfo Peripheral { get; internal set; } = PeripheralInfo.Empty;
    }

    /// <summary>
    /// 外设信息 (心率, 功率, 踏频, 速度) 这些数据
    /// </summary>
    public class PeripheralInfo
    {
        /// <summary>
        /// 表示一个空的外设信息
        /// </summary>
        public static PeripheralInfo Empty { get; } = new();


        /// <summary>
        /// 速度
        /// </summary>
        public Speed Speed { get; init; } = Speed.Zero;

        /// <summary>
        /// 踏频
        /// </summary>
        public double Cadence { get; init; }

        /// <summary>
        /// 心率
        /// </summary>
        public double Heartrate { get; init; }

        /// <summary>
        /// 功率
        /// </summary>
        public double Power { get; init; }
    }
}