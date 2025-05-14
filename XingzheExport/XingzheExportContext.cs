namespace XingzheExport;

public static class XingzheExportContext
{
    /// <summary>
    /// 根目录
    /// </summary>
    public static string BaseDirectory { get; } = AppContext.BaseDirectory;

    /// <summary>
    /// 数据目录
    /// </summary>
    public static string DataDirectory { get; } = Path.Combine(AppContext.BaseDirectory, "Data");

    /// <summary>
    /// 导出目录
    /// </summary>
    public static string ExportDirectory { get; } = Path.Combine(AppContext.BaseDirectory, "Export");
}
