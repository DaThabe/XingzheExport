using XingzheExport.Math;

namespace XingzheExport.Data;



/// <summary>
/// 用户信息
/// </summary>
public partial class UserInfo
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Name { get; } = "";

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; } = "";

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; } 

    /// <summary>
    /// 总里程
    /// </summary>
    public Length TotalMileage { get; } = Length.Zero;


    /// <summary>
    /// 转为字符串
    /// </summary>
    public override string ToString()
    {
        return $"{Id} {Name}";
    }
}

/// <summary>
/// 用户信息解析
/// </summary>
public partial class UserInfo
{
    public UserInfo() { }

    //将服务器返回的完整json作为根节点传入
    internal UserInfo(dynamic root)
    {
        Id = root.userid;
        Name = root.username;
        Email = root.email;
        Level = (int)root.ulevel;
        TotalMileage = Length.FromMetre((long)root.total_distance);
    }
}