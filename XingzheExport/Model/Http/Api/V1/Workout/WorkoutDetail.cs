using UnitsNet;

namespace XingzheExport.Model.Http.Api.V1.Workout;


/// <summary>
/// 训练详细信息
/// </summary>
public partial class WorkoutDetail
{
    /// <summary>
    /// Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public required DateTimeOffset BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public required DateTimeOffset FinishTime { get; set; }

    /// <summary>
    /// 运动类型
    /// </summary>
    public required SportType Sport { get; set; }

    /// <summary>
    /// 采样点
    /// </summary>
    public required List<WorkoutTrackPoint> TrackPoints { get; set; }



    /// <summary>
    /// 消耗热量
    /// </summary>
    public Energy Calories { get; set; } = Energy.Zero;

    /// <summary>
    /// 总距离
    /// </summary>
    public Length Distance { get; set; } = Length.Zero;

    /// <summary>
    /// 总时间
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;


    /// <summary>
    /// 平均速度
    /// </summary>
    public Speed AvgSpeed { get; set; }

    /// <summary>
    /// 平均海拔
    /// </summary>
    public Length AvgAltitude { get; set; } = Length.Zero;

    /// <summary>
    /// 平均踏频
    /// </summary>
    public Frequency AvgCadence { get; set; } = Frequency.Zero;

    /// <summary>
    /// 平均心率
    /// </summary>
    public Frequency AvgHeartrate { get; set; } = Frequency.Zero;


    /// <summary>
    /// 最大速度
    /// </summary>
    public Speed MaxSpeed { get; set; }

    /// <summary>
    /// 最高海拔
    /// </summary>
    public Length MaxAltitude { get; set; } = Length.Zero;

    /// <summary>
    /// 最大踏频
    /// </summary>
    public Frequency MaxCadence { get; set; } = Frequency.Zero;

    /// <summary>
    /// 最大心率
    /// </summary>
    public Frequency MaxHeartrate { get; set; } = Frequency.Zero;


    /// <summary>
    /// 转为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Title}";
}