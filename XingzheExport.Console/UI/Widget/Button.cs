namespace XingzheExport.Console.UI.Widget;


/// <summary>
/// 按钮
/// </summary>
internal class Button : IWidget, IActionWidget
{
    /// <summary>
    /// 按钮标题
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 回调
    /// </summary>

    public event Action<WidgetProcessArgs>? Click;



    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="onClick">操作后回调</param>
    public Button(string title)
    {
        Title = title;
    }



    public bool Process(WidgetProcessArgs args)
    {
        if (Click == null) return false;

        Click?.Invoke(args);
        return true;
    }

    public void Show()
    {
        System.Console.Title = Title;
    }
}
