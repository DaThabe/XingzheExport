namespace XingzheExport.Model.Http.Api.V1;

public class YearMonth
{
    /// <summary>
    /// 年份
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// 月份
    /// </summary>
    public HashSet<int> Month { get; set; } = [];


    public override string ToString()
    {
        //2025年5,6,7,8,9,10月
        return $"{Year}年{string.Join(",", Month)}月";
    }
}
