using XingzheExport.Math;

namespace XingzheExport.Data;



/// <summary>
/// 用户月锻炼信息
/// </summary>
public partial class MonthWorkoutIInfo
{
    /// <summary>
    /// 所有锻炼信息
    /// </summary>
    public WorkoutItem[] WorkoutInfos { get; } = Array.Empty<WorkoutItem>();

    /// <summary>
    /// 本月总里程
    /// </summary>
    public Length TotalMileage { get; } = Length.Zero;

    /// <summary>
    /// 本月总骑行时间
    /// </summary>
    public TimeSpan TotalTime { get; } = TimeSpan.Zero;


    /// <summary>
    /// 转为字符串
    /// </summary>
    public override string ToString()
    {
        if(WorkoutInfos.Length == 0)
        {
            return "本月没有数据";
        }

        return $"{WorkoutInfos[0].StartTime:yyyy年MM月} --总里程:{TotalMileage.ToKilometer()} 千米  --总时间:{TotalTime}";
    }
}

/// <summary>
/// 用户月锻炼信息解析
/// </summary>
public partial class MonthWorkoutIInfo
{
    public MonthWorkoutIInfo() { }

    /// <summary>
    /// 将服务器返回的完整json作为根节点传入
    /// </summary>
    /// <param name="root">Json根节点</param>
    internal MonthWorkoutIInfo(dynamic root)
    {
        WorkoutInfos = CreateWorkoutItems(root).ToArray();

        TotalMileage = Length.FromMetre((int)root.data.st_info.sum_distance);
        TotalTime = TimeSpan.FromSeconds((int)root.data.st_info.sum_duration);
    }

    //解析json
    private static List<WorkoutItem> CreateWorkoutItems(dynamic root)
    {
        List<WorkoutItem> all = new();

        foreach (var i in root.data.wo_info)
        {
            all.Add(CreateWorkoutItem(i));
        }

        return all;
    }

    //解析json
    private static WorkoutItem CreateWorkoutItem(dynamic wo_info_child_node)
    {
        return new WorkoutItem()
        {
            Id = wo_info_child_node.id,
            Title = wo_info_child_node.title,
            UploadTime = wo_info_child_node.upload_time,
            StartTime = XingzheUtil.UnixAsDateTime((long)wo_info_child_node.start_time, TimeSpan.FromHours(8)),
            FinishTime = XingzheUtil.UnixAsDateTime((long)wo_info_child_node.end_time, TimeSpan.FromHours(8)),
            AverageSpeed = Speed.FromHour(Length.FromKilometer((double)wo_info_child_node.avg_speed)),
            TotalMileage = Length.FromMetre((int)wo_info_child_node.distance)
        };
    }
}

/// <summary>
/// 用户月锻炼信息定义
/// </summary>
public partial class MonthWorkoutIInfo
{
    /// <summary>
    /// 锻炼信息
    /// </summary>
    public class WorkoutItem
    {
        /// <summary>
        /// 运动Id
        /// </summary>
        public required long Id { get; init; }

        /// <summary>
        /// 标题
        /// </summary>
        public required string Title { get; init; }


        /// <summary>
        /// 上传时间
        /// </summary>
        public required DateTime UploadTime { get; init; }


        /// <summary>
        /// 锻炼开始时间
        /// </summary>
        public required DateTime StartTime { get; init; }

        /// <summary>
        /// 锻炼结束时间
        /// </summary>
        public required DateTime FinishTime { get; init; }


        /// <summary>
        /// 均速
        /// </summary>
        public required Speed AverageSpeed { get; init; }

        /// <summary>
        /// 总里程
        /// </summary>
        public required Length TotalMileage { get; init; }
    }
}