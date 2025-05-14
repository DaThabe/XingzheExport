using UnitsNet;

namespace XingzheExport.Model.Http.Api.V1.Workout;

/// <summary>
/// 训练信息采样点
/// </summary>
public class WorkoutTrackPointBase
{
    /// <summary>
    /// 时间
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; set; }


    /// <summary>
    /// 转为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] 经度{Longitude}, 维度:{Latitude}";
    }
}

/// <summary>
/// 训练信息采样点
/// </summary>
public class WorkoutTrackPoint : WorkoutTrackPointBase
{
    /// <summary>
    /// 海拔高度
    /// </summary>
    public Length Altitude { get; set; } = Length.Zero;

    /// <summary>
    /// 速度
    /// </summary>
    public Speed Speed { get; set; } = Speed.Zero;

    /// <summary>
    /// 距离
    /// </summary>
    public Length Distance { get; set; } = Length.Zero;

    /// <summary>
    /// 踏频
    /// </summary>
    public Frequency Cadence { get; set; } = Frequency.Zero;

    /// <summary>
    /// 心率
    /// </summary>
    public Frequency Heartrate { get; set; } = Frequency.Zero;

    /// <summary>
    /// 温度
    /// </summary>
    public Temperature Temperature { get; set; } = Temperature.Zero;

    /// <summary>
    /// 功率
    /// </summary>
    public Power Power { get; init; } = Power.Zero;



    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] 经度{Longitude}, 维度:{Latitude}, 海拔:{Altitude}, 速度:{Speed}, 距离:{Distance}, 踏频:{Cadence}, 心率:{Heartrate}, 温度:{Temperature}, 功率:{Power}";
    }
}
