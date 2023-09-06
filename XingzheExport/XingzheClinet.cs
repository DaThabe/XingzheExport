using Sporter.API.XingZhe.Data;

namespace XingzheExport;


/// <summary>
/// 行者客户端
/// </summary>
public class XingzheClinet
{
    /// <summary>
    /// Cookie
    /// </summary>
    private readonly string _Cookie;
    private readonly Random _Rand = new();

    /// <summary>
    /// 用户信息
    /// </summary>
    public UserInfo UserInfo { get; }


    /// <summary>
    /// 创建客户端
    /// </summary>
    /// <param name="cookie">账户Cookie</param>
    public XingzheClinet(string cookie)
    {
        _Cookie = cookie;
        UserInfo = XingzheAPI.GetUserInfoAsync(cookie).Result;
    }



    /// <summary>
    /// 同步数据
    /// </summary>
    public async Task<int> SyncWorkoutDataAsync(Action<(string Path, MonthWorkoutIInfo.WorkoutItem Workout, bool UseCache)>? savedCallback = null)
    {
        int sync_count = 0;

        for (var cur_time = DateTime.Now; cur_time.Year > 2012; cur_time = cur_time.AddMonths(-1))
        {
            var info = await XingzheAPI.GetMonthWorkoutInfoAsync(_Cookie, UserInfo.Id, cur_time.Year, cur_time.Month);
            if (info.WorkoutInfos.Length == 0) continue;


            foreach (var i in info.WorkoutInfos)
            {
                try
                {
                    var file_info = GetGPXFileName(UserInfo.Id, i.StartTime, i.Id);

                    //本地已缓存 直接跳过
                    if (File.Exists(file_info.Full))
                    {
                        savedCallback?.Invoke((file_info.Full, i, true));
                        continue;
                    }

                    var workout = await XingzheAPI.GetWorkoutInfoAsync(_Cookie, i.Id);

                    _ = Task.Run(() =>
                    {
                        var file_name = SaveWorkout(workout, file_info);

                        savedCallback?.Invoke((file_name, i, false));
                        sync_count++;
                    });

                    await Task.Delay(_Rand.Next(100, 1000));
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(ex.ToString());

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        return sync_count;
    }



    /// <summary>
    /// 储存锻炼信息
    /// </summary>
    private static string SaveWorkout(WorkoutInfo info, (string Full, string Folder, string Name) fileInfo, bool useLocal = true)
    {
        var (Full, Folder, Name) = fileInfo;

        if (File.Exists(Full) && useLocal) return Full;

        if (!Directory.Exists(Folder))
        {
            Directory.CreateDirectory(Folder);
        }

        using var fs = File.OpenWrite(Full);
        XingzheUtil.AsGPXDocument(info).Save(fs);

        return Full;
    }

    /// <summary>
    /// 获取本地GPX文件名称
    /// </summary>
    /// <param name="uid">用户Id</param>
    /// <param name="time">运动开始时间</param>
    /// <param name="workoutId">运动记录Id</param>
    private static (string Full, string Folder, string Name) GetGPXFileName(long uid, DateTime time, long workoutId)
    {
        string folder = Path.Combine("gpx", $"{uid}");
        string file_name = $"{time:yyyyMMdd}-{workoutId}.gpx";

        return (Path.Combine(folder, file_name), folder, file_name);
    }
}
