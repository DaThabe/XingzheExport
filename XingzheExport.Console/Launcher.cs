using System.Net.Http.Headers;
using System.Text;
using XingzheExport.Console.UI;
using XingzheExport.Console.UI.Widget;

namespace XingzheExport.Console;

internal static class Launcher
{
    private static readonly Menu MainMenu = new("行者锻炼记录导出工具");

    private static readonly Button Back = new("返回");
    private static readonly Button BackToRoot = new("返回到首页");


    private static readonly Menu Cookie = new("Cookie 管理");
    private static readonly Label CookieList = new("查看");
    private static readonly InputBox CookieAdd = new("添加", "请输入Cookie>> ");
    private static readonly Button CookieReload = new("重载");
    private static readonly Menu CookieDelete = new("删除");


    private static readonly Menu SyncData = new("开始同步");



    /// <summary>
    /// 程序入口
    /// </summary>
    public static void Main()
    {
        IWidgetStack widget_stack = new ActivityWidget(MainMenu);


        Back.Click += args => args.Stack.Back();
        BackToRoot.Click += args => args.Stack.BackToRoot();


        //主菜单
        MainMenu.AddOption(Cookie);
        MainMenu.AddOption(SyncData);

        //Cookie 菜单
        Cookie.AddOption(Back);
        Cookie.AddOption(CookieList);
        Cookie.AddOption(CookieAdd);
        Cookie.AddOption(CookieReload);
        Cookie.AddOption(CookieDelete);

        CookieAdd.Inputed += CookieAdd_Inputed;
        CookieReload.Click += CookieReload_Click;

        CookieManager.ReloadAsync().Wait();
        RecreateDeleteCookieMenu();



        WidgetProcessArgs process_args = new() { Stack = widget_stack };

        while (true)
        {
            System.Console.Clear();
            widget_stack.Current.Show();

            widget_stack.Current.Process(process_args);

            if(process_args.IsInterrupt)
            {
                System.Console.Write("\n按下任意键继续");
                System.Console.ReadLine();

                process_args.IsInterrupt = false;
            }
        }
    }


    /// <summary>
    /// 添加 Cookie
    /// </summary>
    /// <param name="args"></param>
    /// <param name="cookieString"></param>
    private static void CookieAdd_Inputed(WidgetProcessArgs args, string? cookieString)
    {
        args.IsInterrupt = true;


        if (string.IsNullOrWhiteSpace(cookieString))
        {
            System.Console.WriteLine("请输入 Cookie");
            return;
        }

        CookieManager.Set(cookieString, out var name);
        if (name != null)
        {
            System.Console.WriteLine($"添加成功: [{name}]");
        }

        RecreateDeleteCookieMenu();
    }

    //重新加载 Cookie
    private static void CookieReload_Click(WidgetProcessArgs args)
    {
        var result = CookieManager.ReloadAsync().Result;

        if (!result) return;

        System.Console.WriteLine("重载成功");

        RecreateDeleteCookieMenu();
    }


    private static void RecreateDeleteCookieMenu()
    {
        CookieDelete.ClearAllOptions();
        CookieDelete.AddOption(Back);
        CookieDelete.AddOption(BackToRoot);

        SyncData.ClearAllOptions();
        SyncData.AddOption(Back);

        StringBuilder sb = new();


        foreach(var i in CookieManager.All)
        {
            //删除Cookie
            Button delete_item = new(i.Name);
            delete_item.Click += args =>
            {
                args.IsInterrupt = true;
                var result = CookieManager.Remove(i.Name);
                CookieDelete.RemoveOption(delete_item);

                System.Console.WriteLine($"删除{(result ? "成功" : "失败")}");
            };
            CookieDelete.AddOption(delete_item);

            //用户同步
            Button sync_item = new(i.Name);
            sync_item.Click += args =>
            {
                args.IsInterrupt = true;

                XingzheClinet client = new(i.Cookie);
                DateTime start = DateTime.Now;


                client.SyncWorkoutDataAsync((info) =>
                {
                    var current_color = System.Console.ForegroundColor;

                    System.Console.WriteLine("正在同步中...");

                    if (info.UseCache)
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        System.Console.WriteLine($"已存在 [{info.Path}]");
                    }
                    else
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        System.Console.WriteLine($"已保存 [{info.Path}] - {start - DateTime.Now}");
                    }

                    System.Console.ForegroundColor = current_color;

                }).Wait();
            };
            SyncData.AddOption(sync_item);


            sb.AppendLine($"{i.Name}\n{i.Cookie}\n");
        }

        CookieList.Content = sb.ToString();
    }
}
