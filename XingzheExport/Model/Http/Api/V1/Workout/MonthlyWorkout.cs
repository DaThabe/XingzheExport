using UnitsNet;

namespace XingzheExport.Model.Http.Api.V1.Workout;


/// <summary>
/// 整月训练数据
/// </summary>
public class MonthlyWorkout
{
    /// <summary>
    /// 所有训练信息
    /// </summary>
    public required WorkoutSummary[] Days { get; set; }

    /// <summary>
    /// 本月总里程
    /// </summary>
    public required Length TotalMileage { get; set; } 

    /// <summary>
    /// 本月总骑行时间
    /// </summary>
    public required TimeSpan TotalTime { get; set; } 


    /// <summary>
    /// 转为字符串
    /// </summary>
    public override string ToString()
    {
        if(Days.Length == 0)
        {
            return "本月没有数据";
        }

        return $"{Days[0].Timestamp:yyyy年MM月} --总里程:{TotalMileage.As(UnitsNet.Units.LengthUnit.Kilometer)} 千米  --总时间:{TotalTime}";
    }
}