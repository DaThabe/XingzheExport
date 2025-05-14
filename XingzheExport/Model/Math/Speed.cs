namespace XingzheExport.Model.Math;


/// <summary>
/// 表示一个速度单位
/// </summary>
[Obsolete("使用MelinaAero-UnitsNet")] public class Speed
{
    #region --基础单位--

    /// <summary>
    /// 距离
    /// </summary>
    private readonly Length _Length;

    /// <summary>
    /// 所用时间
    /// </summary>
    private readonly TimeSpan _DateTime;


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="length">距离</param>
    /// <param name="time">时间</param>
    private Speed(Length length, TimeSpan time)
    {
        (_Length, _DateTime) = (length, time);
    }


    /// <summary>
    /// 表示 0 速度
    /// </summary>
    public static Speed Zero { get; } = new(Length.Zero, TimeSpan.Zero);


    #endregion

    #region --创建--

    /// <summary>
    /// 秒速
    /// </summary>
    /// <param name="length">距离</param>
    public static Speed FromTime(Length length, TimeSpan time) => new(length, time);

    /// <summary>
    /// 秒速
    /// </summary>
    /// <param name="length">距离</param>
    /// <param name="seconds">所用秒数</param>
    public static Speed FromSecond(Length length, double seconds = 1) => new(length, TimeSpan.FromSeconds(seconds));

    /// <summary>
    /// 分速
    /// </summary>
    /// <param name="length">距离</param>
    /// <param name="seconds">所用分数</param>
    public static Speed FromMinute(Length length, double minutes = 1) => new(length, TimeSpan.FromMinutes(minutes));

    /// <summary>
    /// 时速
    /// </summary>
    /// <param name="length">距离</param>
    /// <param name="seconds">所用小时</param>
    public static Speed FromHour(Length length, double hour = 1) => new(length, TimeSpan.FromHours(hour));

    /// <summary>
    /// 日速
    /// </summary>
    /// <param name="length">距离</param>
    /// <param name="seconds">所用天数</param>
    public static Speed FromDay(Length length, double day = 1) => new(length, TimeSpan.FromDays(day));

    /// <summary>
    /// 周速
    /// </summary>
    /// <param name="length">距离</param>
    /// <param name="seconds">所用周数</param>
    public static Speed FromWeek(Length length, double week = 1) => new(length, TimeSpan.FromDays(7 * week));

    #endregion

    #region --转换--

    /// <summary>
    /// 转为千米/时
    /// </summary>
    /// <returns></returns>
    public double ToKilometresPerHour() => _Length.ToKilometer() / _DateTime.TotalHours;

    /// <summary>
    /// 转为千米/分
    /// </summary>
    public double ToKilometresPerMinute() => _Length.ToKilometer() / _DateTime.TotalMinutes;

    /// <summary>
    /// 转为千米/秒
    /// </summary>
    public double ToKilometresPerSecond() => _Length.ToKilometer() / _DateTime.TotalSeconds;

    /// <summary>
    /// 转为米/分
    /// </summary>
    public double ToMetrePerMinute() => _Length.ToMetre() / _DateTime.TotalMinutes;

    /// <summary>
    /// 转为米/秒
    /// </summary>
    public double ToMetrePerSecond() => _Length.ToMetre() / _DateTime.TotalSeconds;


    /// <summary>
    /// 转为 英里|迈/时
    /// </summary>
    public double ToMilPerHour() => _Length.ToMil() / _DateTime.TotalHours;

    /// <summary>
    /// 转为 码/时
    /// </summary>
    public double ToYardPerHour() => _Length.ToYard() / _DateTime.TotalHours;





    #endregion

    #region --运算符重载--

    public static Length operator *(Speed left, TimeSpan right)
    {
        return Length.FromKilometer(left.ToKilometresPerHour() * right.TotalHours);
    }

    public static Length operator *(TimeSpan left, Speed right)
    {
        return Length.FromKilometer(right.ToKilometresPerHour() * left.TotalHours);
    }

    public override string ToString()
    {
        return $"{ToKilometresPerHour()} 千米/时";
    }

    #endregion
}