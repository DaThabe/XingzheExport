namespace XingzheExport.Model.Http.Api.V1.User;



/// <summary>
/// 用户信息
/// </summary>
public partial class UserInfo
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public required string Name { get; set; }


    /// <summary>
    /// 转为字符串
    /// </summary>
    public override string ToString() => $"Uid:{Id}, 用户名:{Name}";
}