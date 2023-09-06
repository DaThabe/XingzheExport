//using Newtonsoft.Json;
//using XingzheExport.Console.UI;

//namespace Thabe.Sporter;


///// <summary>
///// 行者 控制台
///// </summary>
//public static class XingZheConsole
//{
//    private record UserCookie(string Username, string Cookie);

//    private static Dictionary<long, UserCookie> _Cookies = new();

//    private static Menu _CurrentMenu;


//    static XingZheConsole()
//    {
//        Menu MainMenu = new("行者数据同步工具");
//        Menu Cookie = new("Cookie管理");
//        Menu SyncData = new("数据同步");


//        MainMenu.Add(Cookie, () =>
//        {
//            _CurrentMenu = Cookie;
//            return Task.CompletedTask;
//        });

//        MainMenu.Add(SyncData, () =>
//        {
//            if(_Cookies.Count == 0)
//            {
//                Console.WriteLine("暂无 Cookie 无法进行操作");
//                return Task.CompletedTask;
//            }

//            _CurrentMenu = SyncData;
//            return Task.CompletedTask;
//        });


//        Cookie.Add("重载", () =>
//        {
//            ReloadCookie();
//            return Task.CompletedTask;
//        });

//        Cookie.Add("添加", async () =>
//        {
//            Console.Write(">> ");
//            var cookie = Console.ReadLine();

//            if(cookie == null)
//            {
//                Console.WriteLine("Cookie错误");
//                return;
//            }

//            var userinfo = await XingZheAPI.GetUserInfoAsync(cookie);
//            _Cookies[userinfo.Id] = new(userinfo.Name, cookie);

//            await File.WriteAllTextAsync("Cookies", JsonConvert.SerializeObject(_Cookies));

//            Console.WriteLine($"添加成功: [{userinfo.Name}]");

//            ResetSyncList();
//        });

//        Cookie.Add("返回", back_to_main_menu);



//        _CurrentMenu = MainMenu;
//        ReloadCookie();



//        Task back_to_main_menu()
//        {
//            Console.Clear();
//            _CurrentMenu = MainMenu;
//            return Task.CompletedTask;
//        }

//        void ReloadCookie()
//        {
//            if (!File.Exists("Cookies"))
//            {
//                Console.WriteLine("暂无数据 无法重载");
//                return;
//            }

//            try
//            {
//                var content = File.ReadAllText("Cookies");
//                var value = JsonConvert.DeserializeObject<Dictionary<long, UserCookie>>(content);

//                if (value == null)
//                {
//                    Console.WriteLine("重载失败");
//                    return;
//                }

//                _Cookies = value;

//                Console.WriteLine("重载成功");

//                foreach (var i in _Cookies)
//                {
//                    Console.WriteLine($"{i.Value.Username} - {i.Value.Cookie}");
//                }

//                ResetSyncList();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }

//        void ResetSyncList()
//        {
//            SyncData.ClearAllOptions();

//            foreach (var i in _Cookies)
//            {
//                SyncData.Add(i.Value.Username, async () =>
//                {
//                    Console.WriteLine("正在同步中...");

//                    XingZheClinet client = new(i.Value.Cookie);
//                    DateTime start = DateTime.Now;

//                    var count = await client.SyncWorkoutDataAsync(x =>
//                    {
//                        var current_cOlor = Console.ForegroundColor;

//                        if (x.UseCache)
//                        {
//                            Console.ForegroundColor = ConsoleColor.DarkYellow;
//                            Console.WriteLine($"已存在 [{x.Path}]");
//                        }
//                        else
//                        {
//                            Console.ForegroundColor = ConsoleColor.DarkGreen;
//                            Console.WriteLine($"已保存 [{x.Path}] - {start - DateTime.Now}");
//                        }
//                    });

//                    Console.WriteLine($"成功同步 {count} 个文件");
//                });
//            }

//            SyncData.Add("返回", back_to_main_menu);
//        }

//    }



//    public static void Run()
//    {
//        while (true) _CurrentMenu.ExcuteAsync().Wait();
//    }
//}
