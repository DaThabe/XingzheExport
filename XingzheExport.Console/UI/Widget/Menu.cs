using System.Text;

namespace XingzheExport.Console.UI.Widget;



/// <summary>
/// 控制台交互菜单
/// </summary>
internal class Menu : IWidget
{
    /// <summary>
    /// 菜单标题
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 选项列表
    /// </summary>
    private readonly List<IWidget> _Options = new();


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="title">菜单标题</param>
    public Menu(string title)
    {
        Title = title;
    }



    /// <summary>
    /// 添加一个选项
    /// </summary>
    /// <param name="name">选项名称</param>
    /// <param name="action">选项动作</param>
    public Menu AddOption(IWidget widget)
    {
        _Options.Add(widget);
        return this;
    }

    public bool RemoveOption(IWidget widget)
    {
        return _Options.Remove(widget);
    }

    /// <summary>
    /// 清除所有选项
    /// </summary>
    public void ClearAllOptions()
    {
        _Options.Clear();
    }




    /// <summary>
    /// 显示菜单
    /// </summary>
    public void Show()
    {
        System.Console.Title = Title;

        StringBuilder sb = new();

        for (int i = 0; i < _Options.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {_Options[i].Title}");
        }

        System.Console.Write($"{sb}\n请输入[序号]>> ");
    }

    /// <summary>
    /// 操作菜单
    /// </summary>
    public bool Process(WidgetProcessArgs args)
    {
        if (!int.TryParse(System.Console.ReadLine(), out var id)) return false;
        if (id < 1 || id > _Options.Count) return false;

        var widget = _Options[id - 1];

        if (widget is not IActionWidget)
        {
            args.Stack.Push(widget);
            return true;
        }

        widget.Process(args);
        return true;
    }
}
