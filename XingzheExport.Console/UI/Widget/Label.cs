namespace XingzheExport.Console.UI.Widget;



/// <summary>
/// 标签
/// </summary>
internal class Label : IWidget
{
    public string Title { get; }

    public string? Content { get; set; }


    public Label(string title)
    {
        Title = title;
    }



    public bool Process(WidgetProcessArgs args)
    {
        args.IsInterrupt = true;

        args.Stack.Back();
        return true;
    }

    public void Show()
    {
        System.Console.Write(Content);
    }
}
