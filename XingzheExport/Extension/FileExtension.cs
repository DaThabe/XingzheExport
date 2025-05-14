using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Linq;
using UnitsNet.Units;
using XingzheExport.Model.Http.Api.V1;
using XingzheExport.Model.Http.Api.V1.Workout;

namespace XingzheExport.Extension;



/// <summary>
/// Gpx 扩展方法
/// </summary>
public static class FileExtension
{
    /// <summary>
    /// 将WorkoutInfo转为GPX文档 <br/>
    /// 致谢项目 <a href="https://github.com/Harry-Chen/xingzhe-gpx-processor/blob/master/merge.py">xingzhe-gpx-processor</a> 给出GPX1.1版本可以附加心率,踏频等信息
    /// </summary>
    /// <param name="info">训练信息实例</param>
    public static XDocument ToGPXDocument(this WorkoutDetail info)
    {
        XNamespace ns = "http://www.topografix.com/GPX/1/1";
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

        var gpx = new XElement(ns + "gpx",
            new XAttribute(XNamespace.Xmlns + "xsi", xsi),
            new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
            new XAttribute(xsi + "schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd"),
            new XAttribute("version", "1.1"),
            new XAttribute("creator", "[imxingzhe.com, XingzheExport]"),

            new XElement(ns + "name", info.Title),
            new XElement(ns + "desc", "由 [XingzheExport] 根据 [行者] 软件 生成"),
            new XElement(ns + "time", info.BeginTime),

            new XElement(ns + "trk",
                new XElement(ns + "name", "XingzheExport"),
                new XElement(ns + "trkseg",
                    from p in info.TrackPoints
                    select new XElement(ns + "trkpt",
                        new XAttribute("lat", p.Latitude),
                        new XAttribute("lon", p.Longitude),
                        new XElement(ns + "ele", p.Altitude.As(LengthUnit.Meter)),
                        new XElement(ns + "time", p.Timestamp),
                        new XElement(ns + "extensions",
                            new XElement(ns + "speed", p.Speed.As(SpeedUnit.MeterPerSecond)),
                            new XElement(ns + "cadence", p.Cadence.As(FrequencyUnit.CyclePerMinute)),
                            new XElement(ns + "heartrate", p.Heartrate.As(FrequencyUnit.BeatPerMinute)),
                            new XElement(ns + "power", p.Power.As(PowerUnit.Watt))
                        )
                    )
                )
            )
        );

        return new XDocument(new XDeclaration("1.0", "UTF-8", null), gpx);
    }

    /// <summary>
    /// 将WorkoutInfo转为TCX文档
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public static XDocument ToTCXDocument(this WorkoutDetail info)
    {
        XNamespace ns = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";

        var sportTypeString = info.Sport switch
        {
            SportType.Run => "Running",
            SportType.Ride => "Cycling",
            SportType.Hike => "Hiking",
            SportType.Swim => "Swimming",
            SportType.Ski => "Skiing",
            SportType.Workout => "Fitness",
            _ => "Other"
        };

        var gpx = new XElement(ns + "TrainingCenterDatabase",
            new XElement(ns + "Activities",
                new XElement(ns + "Activity",
                    new XAttribute("Sport", sportTypeString),
                    new XElement(ns + "Id", info.BeginTime),
                    new XElement(ns + "Lap",
                        new XAttribute("StartTime", info.BeginTime),
                        new XElement(ns + "TotalTimeSeconds", info.Duration.TotalSeconds),
                        new XElement(ns + "DistanceMeters", info.Distance.As(LengthUnit.Meter)),
                        new XElement(ns + "Calories", info.Calories.As(EnergyUnit.Joule)),
                        new XElement(ns + "Intensity", "Active"),
                        new XElement(ns + "TriggerMethod", "Manual"),
                        new XElement(ns + "Track",
                            from p in info.TrackPoints
                            select new XElement(ns + "Trackpoint",
                                new XElement(ns + "Time", p.Timestamp),
                                new XElement(ns + "Position",
                                    new XElement(ns + "LatitudeDegrees", p.Latitude),
                                    new XElement(ns + "LongitudeDegrees", p.Longitude)
                                ),
                                new XElement(ns + "AltitudeMeters", p.Altitude.As(LengthUnit.Meter)),
                                new XElement(ns + "DistanceMeters", p.Distance.As(LengthUnit.Meter)),
                                new XElement(ns + "HeartRateBpm",
                                    new XElement(ns + "Value", p.Heartrate.As(FrequencyUnit.BeatPerMinute))
                                ),
                                new XElement(ns + "Cadence", p.Cadence.As(FrequencyUnit.CyclePerMinute)),
                                new XElement(ns + "Power", p.Power.As(PowerUnit.Watt))
                            )
                        )
                    )
                )
            )
        );

        return new XDocument(new XDeclaration("1.0", "UTF-8", null), gpx);
    }


    /// <summary>
    /// 保存文档到指定路径
    /// </summary>
    /// <param name="document"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task SaveAsync(this XDocument document, string path)
    {
        var folder = Path.GetDirectoryName(path);
        if(!string.IsNullOrWhiteSpace(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using var fs = File.OpenWrite(path);
        await document.SaveAsync(fs, SaveOptions.None, default);
    }




    /// <summary>
    /// 转为Json
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static JToken ToJson(this object value)
    {
        return JToken.FromObject(value);
    }
    
    /// <summary>
    /// 保存Json到指定路径
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task SaveAsync(this JToken jToken, string path)
    {
        var content = JsonConvert.SerializeObject(jToken);

        var folder = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(folder)) Directory.CreateDirectory(folder);
        await File.WriteAllTextAsync(path, content);
    }
}