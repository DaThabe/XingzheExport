namespace XingzheExport.Console.UI.Widget;



/// <summary>
/// 输入框
/// </summary>
internal class InputBox : IWidget
{
    /// <summary>
    /// 输入框标题
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 输入框提示
    /// </summary>
    public string Tips { get; }

    /// <summary>
    /// 输入回调
    /// </summary>
    public event Action<WidgetProcessArgs, string?>? Inputed;


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="tips"></param>
    public InputBox(string title, string tips)
    {
        (Title, Tips) = (title, tips);
    }


    public bool Process(WidgetProcessArgs args)
    {
        var input = System.Console.ReadLine();
        Inputed?.Invoke(args, input);

        args.Stack.Back();

        return true;
    }

    public void Show()
    {
        System.Console.Title = Title;
        System.Console.Write($"{Tips}");
    }
}
