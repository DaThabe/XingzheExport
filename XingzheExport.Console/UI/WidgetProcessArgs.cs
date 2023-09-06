namespace XingzheExport.Console.UI;



/// <summary>
/// 控件执行参数
/// </summary>
internal class WidgetProcessArgs
{
    /// <summary>
    /// 活跃控件
    /// </summary>
    public required IWidgetStack Stack { get; init; }

    /// <summary>
    /// 是否中断
    /// </summary>
    public bool IsInterrupt { get; set; } = false;
}
