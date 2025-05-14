//using XingzheExport.Model.Http.Api.V1;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XingzheExport.Extension;
using XingzheExport.Model.Http.Api.V1.Workout;
using XingzheExport.Model.Sync;
using XingzheExport.Service.Http;

namespace XingzheExport.Service;

public interface ISyncService : IHostedService
{
    Task SyncAsync(string sessionId, Func<WorkoutDetail, Task> saveAction, IProgress<ProgressValue>? progress = null);
}

internal class SyncService(
    ILogger<SyncService> logger,
    ISessionIdService sessionIdService,
    IXingzheApiV1 api
    ) : ISyncService
{
    public async Task SyncAsync(string sessionId, Func<WorkoutDetail, Task> saveAction, IProgress<ProgressValue>? progress = null)
    {
        //获取所有训练信息
        List<WorkoutSummary> workoutSummaries = [];
        for (int offset = 0, limit = 1000; true;)
        {
            var result = await api.GetWorkoutSummariesAsync(sessionId, offset, limit);
            if (result.Count == 0) break;

            workoutSummaries.AddRange(result);
            offset += limit;
        }

        //初始化列表
        var userInfo = await sessionIdService.SetAsync(sessionId);
        if(!_value.TryGetValue(userInfo.Id, out var workoutIds))
        {
            workoutIds = new HashSet<long>();
            _value[userInfo.Id] = workoutIds;
        }

        //筛选未同步的训练记录
        workoutSummaries = [.. workoutSummaries.Where(x => !workoutIds.Contains(x.Id))];
        int current = 0;

        //同步
        foreach (var i in workoutSummaries)
        {
            try
            {
                var detail = await api.GetWorkoutDetailAsync(sessionId, i.Id);

                await saveAction(detail);

                workoutIds.Add(i.Id);
                await SaveToFileAsync();

                progress?.Report(new ProgressValue()
                {
                    WorkoutDetail = detail,
                    Current = current,
                    Max = workoutSummaries.Count
                });
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex, "训练数据同步失败:{info}", i);

                progress?.Report(new ProgressValue()
                {
                    Exception = ex,
                    Current = current,
                    Max = workoutSummaries.Count
                });
            }

            current++;
        }

        //完成
        progress?.Report(new ProgressValue() { Max = 0, Current = 0 });
    }


    /// <summary>
    /// 从本地加载
    /// </summary>
    /// <returns></returns>
    public async Task LoadFromFileAsync()
    {
        if (!File.Exists(_localFilePath)) throw new InvalidOperationException("同步列表加载失败, 本地文件不存在");

        var content = await File.ReadAllTextAsync(_localFilePath);
        var instance = JsonConvert.DeserializeObject<Dictionary<long, HashSet<long>>>(content);

        if (instance == null) throw new InvalidOperationException("同步列表加载失败, 本地文件无法解析");
        _value = instance;
    }

    /// <summary>
    /// 保存到本地
    /// </summary>
    /// <returns></returns>
    public async Task SaveToFileAsync()
    {
        await _value.ToJson().SaveAsync(_localFilePath);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(_localFilePath)) return;
            await LoadFromFileAsync();
        }
        catch(Exception ex)
        {
            logger.LogWarning(ex, "同步列表初始化失败, 本地文件不存在");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    /// <summary>
    /// 已同步列表
    /// </summary>

    private Dictionary<long, HashSet<long>> _value = [];

    /// <summary>
    /// 本地文件路径
    /// </summary>
    private readonly string _localFilePath = Path.Combine(XingzheExportContext.DataDirectory, "SyncList.json");
}