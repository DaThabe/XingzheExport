//using XingzheExport.Extension;
//using XingzheExport.Model.Http.Api.V1.Workout;
//using XingzheExport.Model.Math;

//namespace XingzheExport.Service;

///// <summary>
///// 用户月训练信息解析
///// </summary>
//public class MonthWorkoutIInfoFactory
//{
//    /// <summary>
//    /// 将服务器返回的完整 Json 作为根节点传入
//    /// </summary>
//    /// <param name="root">Json根节点</param>
//    public static MonthlyWorkout CreateFromJson(dynamic root)
//    {
//        MonthlyWorkout info = new()
//        {
//            Days = CreateWorkoutItems(root).ToArray(),
//            TotalMileage = Length.FromMetre((int)root.data.st_info.sum_distance),
//            TotalTime = TimeSpan.FromSeconds((int)root.data.st_info.sum_duration)
//        };

//        return info;
//    }

//    //解析json
//    private static List<WorkoutSummary> CreateWorkoutItems(dynamic root)
//    {
//        List<WorkoutSummary> all = new();

//        foreach (var i in root.data.wo_info)
//        {
//            all.Add(CreateWorkoutItem(i));
//        }

//        return all;


//        static WorkoutSummary CreateWorkoutItem(dynamic node)
//        {
//            return new WorkoutSummary()
//            {
//                Id = node.id,
//                Title = node.title,
//                UploadTime = node.upload_time,
//                Timestamp = XingzheUtil.UnixAsDateTime((long)node.start_time, TimeSpan.FromHours(8)),
//                FinishTime = XingzheUtil.UnixAsDateTime((long)node.end_time, TimeSpan.FromHours(8)),
//                AvgSpeed = Speed.FromHour(Length.FromKilometer((double)node.avg_speed)),
//                Distance = Length.FromMetre((int)node.distance)
//            };
//        }
//    }
//}
