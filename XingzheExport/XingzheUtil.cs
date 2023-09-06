using Sporter.API.XingZhe.Data;
using System.Xml;

namespace XingzheExport;



/// <summary>
/// 行者工具
/// </summary>
public static class XingzheUtil
{
    /// <summary>
    /// 将WorkoutInfo转为GPX文档 <br/>
    /// 致谢项目 <a href="https://github.com/Harry-Chen/xingzhe-gpx-processor/blob/master/merge.py">xingzhe-gpx-processor</a> 给出GPX1.1版本可以附加心率,踏频等信息
    /// </summary>
    /// <param name="info">锻炼信息实例</param>
    public static XmlDocument AsGPXDocument(this WorkoutInfo info)
    {
        // 创建一个新的 XmlDocument 对象
        XmlDocument xmlDoc = new();

        // 创建 XML 声明节点并添加到文档中
        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

        // 创建 GPX 根元素，并添加默认命名空间
        XmlElement gpx = xmlDoc.CreateElement("gpx", "http://www.topografix.com/GPX/1/1");

        // 添加其他属性到 GPX 根元素
        gpx.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        gpx.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
        gpx.SetAttribute("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
        gpx.SetAttribute("version", "1.1");
        gpx.SetAttribute("creator", "[imxingzhe.com, DaThabe]");


        //名称
        XmlElement name = xmlDoc.CreateElement("name");
        name.InnerText = info.Title;

        //描述
        XmlElement desc = xmlDoc.CreateElement("desc");
        desc.InnerText = "由 XingzheExport 根据 行者骑行软件 生成";

        //时间
        XmlElement time = xmlDoc.CreateElement("time");
        time.InnerText = info.Time.AddHours(-8).ToString("yyyy-MM-ddTHH:mm:ssZ");



        //路径根节点
        XmlElement trk = xmlDoc.CreateElement("trk");

        //路径名称
        XmlElement trk_name = xmlDoc.CreateElement("name");
        trk_name.InnerText = "Thabe.Sporter";

        //路径组根节点
        XmlElement trk_trkseg = xmlDoc.CreateElement("trkseg");

        trk.AppendChild(trk_name);
        trk.AppendChild(trk_trkseg);


        //写入点信息
        foreach (var p in info.Points)
        {
            //维
            var att_lat = xmlDoc.CreateAttribute("lat");
            att_lat.InnerText = p.Latitude.ToString();

            //经
            var att_lon = xmlDoc.CreateAttribute("lon");
            att_lon.InnerText = p.Longitude.ToString();

            //海拔
            var ele = xmlDoc.CreateElement("ele");
            ele.InnerText = p.Altitude.ToString();

            //时间
            var p_time = xmlDoc.CreateElement("time");
            p_time.InnerText = p.Time.AddHours(-8).ToString("yyyy-MM-ddTHH:mm:ssZ");

            //扩展信息
            var extensions = xmlDoc.CreateElement("extensions");

            //速度 米/秒
            var speed = xmlDoc.CreateElement("speed");
            speed.InnerText = p.Peripheral.Speed.ToMetrePerSecond().ToString();

            //踏频
            var cadence = xmlDoc.CreateElement("cadence");
            cadence.InnerText = p.Peripheral.Cadence.ToString();

            //心率
            var heartrate = xmlDoc.CreateElement("heartrate");
            heartrate.InnerText = p.Peripheral.Heartrate.ToString();

            //功率
            var power = xmlDoc.CreateElement("power");
            power.InnerText = p.Peripheral.Power.ToString();

            extensions.AppendChild(speed);
            extensions.AppendChild(cadence);
            extensions.AppendChild(heartrate);
            extensions.AppendChild(power);


            //创建点元素
            var trkpt = xmlDoc.CreateElement("trkpt");
            trkpt.Attributes.Append(att_lat);
            trkpt.Attributes.Append(att_lon);


            trkpt.AppendChild(ele);
            trkpt.AppendChild(p_time);
            trkpt.AppendChild(extensions);


            trk_trkseg.AppendChild(trkpt);

        }


        gpx.AppendChild(name);
        gpx.AppendChild(desc);
        gpx.AppendChild(time);
        gpx.AppendChild(trk);


        xmlDoc.AppendChild(xmlDeclaration);
        xmlDoc.AppendChild(gpx);

        return xmlDoc;
    }

    /// <summary>
    /// 将 Unix 时间转为 DateTime
    /// </summary>
    /// <param name="unix">Unix 时间</param>
    /// <param name="offset">时区偏移量</param>
    public static DateTime UnixAsDateTime(long unix, TimeSpan offset)
    {
        var date_time = DateTimeOffset.FromUnixTimeMilliseconds(unix).UtcDateTime;
        return date_time.Add(offset);
    }
}
