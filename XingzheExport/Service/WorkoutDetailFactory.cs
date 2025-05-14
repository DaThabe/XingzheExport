//using System.Xml;
//using XingzheExport.Model.Math;
//using XingzheExport.Model.Workout;

//namespace XingzheExport.Service;

///// <summary>
///// 训练信息解析
///// </summary>
//public class WorkoutDetailFactory
//{
//    public WorkoutDetail() { }

//    /// <summary>
//    /// 将服务器返回的 GPX 和 外设数据节点 Json 根节点 合并
//    /// </summary>
//    /// <param name="gpxDocument">GPX 文档</param>
//    /// <param name="peripheralRoot">外设 Json 根节点</param>
//    /// <exception cref="ArgumentNullException">这个训练信息没有路径</exception>
//    internal WorkoutDetail(XmlDocument gpxDocument, dynamic? peripheralRoot = null)
//    {
//        Title = gpxDocument?["gpx"]?["name"]?.InnerText ?? "";

//        var time_text = gpxDocument?["gpx"]?["time"]?.InnerText;
//        BeginTime = time_text is null ? DateTime.Now : DateTime.Parse(time_text);

//        var nodes = gpxDocument?["gpx"]?["trk"]?["trkseg"]?.ChildNodes;
//        if (nodes is null)
//        {
//            throw new ArgumentNullException(nameof(nodes), "没有训练数据节点");
//        }


//        List<WorkoutDetailPoint> points = ParserGPXPoint(nodes);
//        if (peripheralRoot != null) SetPeripheralInfo(ref points, peripheralRoot);

//        TrackPoints = points.ToArray();
//    }



//    //读取GPX文件中的数据
//    private static List<WorkoutDetailPoint> ParserGPXPoint(XmlNodeList racks_node)
//    {
//        List<WorkoutDetailPoint> all = new();

//        foreach (XmlNode node in racks_node)
//        {
//            _ = double.TryParse(node.Attributes?["lat"]?.InnerText, out var lat);
//            _ = double.TryParse(node.Attributes?["lon"]?.InnerText, out var lon);
//            _ = double.TryParse(node?["ele"]?.InnerText, out var ele);


//            var time_text = node?["time"]?.InnerText;
//            var Time = time_text is null ? DateTime.Now : DateTime.Parse(time_text);


//            all.Add(new()
//            {
//                Altitude = ele,
//                Datetime = Time,
//                Latitude = lat,
//                Longitude = lon
//            });
//        }

//        return all;
//    }

//    //设置外设数据
//    private static void SetPeripheralInfo(ref List<WorkoutTrackPoint> point, dynamic root)
//    {
//        Dictionary<DateTime, WorkoutTrackPoint> cache = 
//            new(from i in point select new KeyValuePair<DateTime, WorkoutTrackPoint>(i.Datetime, i));


//        foreach(var i in root.points)
//        {
//            //不知道为什么是UTC+16
//            var time = XingzheUtil.UnixAsDateTime((long)i.time, TimeSpan.FromHours(16));

//            cache.TryGetValue(time, out var p);
//            if (p == null) continue;

//            cache[time].Peripheral = new()
//            {
//                Heartrate = i.heartrate,
//                Power = i.power,
//                Cadence = i.cadence,
//                Speed = Speed.FromSecond(Length.FromMetre((double)i.speed)),
//            };

//        }
//    }
//}