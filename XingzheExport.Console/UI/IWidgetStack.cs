namespace XingzheExport.Console.UI;



/// <summary>
/// 控件栈
/// </summary>
internal interface IWidgetStack
{
    /// <summary>
    /// 当前活跃
    /// </summary>
    IWidget Current { get; }

    /// <summary>
    /// 根控件
    /// </summary>
    IWidget Root { get; }

    /// <summary>
    /// 控件深度
    /// </summary>
    int Depth { get; }


    /// <summary>
    /// 返回上一级
    /// </summary>
    bool Back();

    /// <summary>
    /// 返回到根
    /// </summary>
    void BackToRoot();

    /// <summary>
    /// 推入控件
    /// </summary>
    void Push(IWidget widget);

    /// <summary>
    /// 清除所有控件 重新设置根控件
    /// </summary>
    void New(IWidget root);
}


internal class ActivityWidget : IWidgetStack
{
    public IWidget Current => _Widgets.Peek();

    public IWidget Root { get; private set; }

    public int Depth => _Widgets.Count - 1;


    private readonly Stack<IWidget> _Widgets = new();



    public ActivityWidget(IWidget root)
    {
        Root = root;
        _Widgets.Push(root);
    }



    public bool Back()
    {
        if(_Widgets.Count < 1) return false;
        return _Widgets.TryPop(out var _);
    }

    public void BackToRoot()
    {
        New(Root);
    }

    public void Push(IWidget widget)
    {
        _Widgets.Push(widget);
    }

    public void New(IWidget root)
    {
        Root = root;

        _Widgets.Clear();
        _Widgets.Push(root);
    }
}
