namespace XingzheExport.Extension;

internal static class DateExtension
{
    /// <summary>
    /// 将 Unix 时间戳（秒或毫秒）转换为指定时区的 DateTimeOffset。
    /// </summary>
    /// <param name="unixTime">Unix 时间戳（秒或毫秒）</param>
    /// <param name="offset">目标时区偏移</param>
    /// <returns>指定时区的 DateTimeOffset</returns>
    public static DateTime ToDateTime(this long unixTime)
    {
        if (unixTime > 9999999999)
        {
            return _unixBeginTime.AddMilliseconds(unixTime);
        }

        return _unixBeginTime.AddSeconds(unixTime);
    }

    public static DateTimeOffset ToDateTimeOffset(this long unixTime, TimeSpan offset)
    {
        var date = unixTime.ToDateTime();
        return new DateTimeOffset(date + offset, offset);
    }



    /// <summary>
    /// 将 Unix 时间戳（秒或毫秒）转换为北京时间（UTC+8）的 DateTimeOffset。
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTimeOffset ToBeijingTime(this long unixTime)
    {
        return unixTime.ToDateTimeOffset(TimeSpan.FromHours(8));
    }



    private static readonly DateTime _unixBeginTime = new(1970, 1, 1, 0, 0, 0);
}
