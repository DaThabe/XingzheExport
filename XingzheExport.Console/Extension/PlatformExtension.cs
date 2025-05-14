using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XingzheExport.Console.Extension;

internal static class PlatformExtension
{
    /// <summary>
    /// 打开目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Process OpenFolder(this string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Process.Start("explorer", path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Process.Start("open", path);
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Process.Start("xdg-open", path);
        }

        throw new InvalidOperationException("无法打开目录");
    }
}
