namespace XingzheExport.Console.UI;



/// <summary>
/// 控件接口
/// </summary>
internal interface IWidget
{
    /// <summary>
    /// 控件标题
    /// </summary>
    string Title { get; }



    /// <summary>
    /// 显示菜单
    /// </summary>
    void Show();

    /// <summary>
    /// 操作菜单
    /// </summary>
    /// <returns>是否操作成功</returns>
    bool Process(WidgetProcessArgs args);
}
