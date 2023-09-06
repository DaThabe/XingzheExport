namespace XingzheExport.Math;


/// <summary>
/// 表示一个长度单位
/// </summary>
public class Length
{
    /// <summary>
    /// 其他单位与 米 的比值
    /// </summary>
    private static class MetreScale
    {
        /// <summary>
        /// 皮米 (pm)
        /// </summary>
        public const double Picometer = 1e-12;

        /// <summary>
        /// 纳米 nm
        /// </summary>
        public const double Nanometer = 1e-9;

        /// <summary>
        /// 微米 μm
        /// </summary>
        public const double Micrometer = 1e-6;

        /// <summary>
        /// 毫米 (mm)
        /// </summary>
        public const double Millimeter = 1e-3;

        /// <summary>
        /// 厘米 (cm)
        /// </summary>
        public const double Centimeter = 1e-2;

        /// <summary>
        /// 分米 (dm)
        /// </summary>
        public const double Decimeter = 1e-1;

        /// <summary>
        /// 米 (m)
        /// </summary>
        public const double Metre = 1;

        /// <summary>
        /// 千米 (km)
        /// </summary>
        public const double Kilometer = 1e3;



        /// <summary>
        /// 光秒 (ls)
        /// </summary>
        public const double LightSecond = 299792458;

        /// <summary>
        /// 光年 (ly)
        /// </summary>
        public const double LightYear = 9.461e12;

        /// <summary>
        /// 秒差距（pc）
        /// </summary>
        public const double Parsec = 3.08567758149137e16;



        /// <summary>
        /// 密耳 (thou)
        /// </summary>
        public const double Mil = 2.54e-5;

        /// <summary>
        /// 英寸 (in)
        /// </summary>
        public const double Inch = 2.54e-2;

        /// <summary>
        /// 英尺 (ft)
        /// </summary>
        public const double Foot = 0.3048;

        /// <summary>
        /// 码 (yd)
        /// </summary>
        public const double Yard = 0.9144;

        /// <summary>
        /// 英寻 (Fathom)
        /// </summary>
        public const double Fathom = 1.8288;

        /// <summary>
        /// 迈|英里 (mi)
        /// </summary>
        public const double Mile = 1609.344;

        /// <summary>
        /// 海里 (nmi)
        /// </summary>
        public const double NauticalMile = 1852;

        /// <summary>
        /// 弗隆 (Furlong)
        /// </summary>
        public const double Furlong = 201.168;



        /// <summary>
        /// 里 (Lǐ)
        /// </summary>
        public const double Lǐ = 5.00e2;

        /// <summary>
        /// 丈 (Zhàng)
        /// </summary>
        public const double Zhàng = 3.3;

        /// <summary>
        /// 尺 (Chǐ)
        /// </summary>
        public const double Chǐ = 3.33e-1;

        /// <summary>
        /// 寸 (Cùn)
        /// </summary>
        public const double Cùn = 3.33e-2;

        /// <summary>
        /// 分 (Fēn)
        /// </summary>
        public const double Fēn = 3.33e-3;

        /// <summary>
        /// 厘 (Lí)
        /// </summary>
        public const double Lí = 3.33e-4;

        /// <summary>
        /// 毫 (Háo)
        /// </summary>
        public const double Háo = 3.33e-5;
    }


    #region --基础单位--

    /// <summary>
    /// 以(米)为基础单位
    /// </summary>
    private readonly double _TotalMetre;


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="scale">比值</param>
    private Length(double value, double scale) => _TotalMetre = value * scale;

    /// <summary>
    /// 用米初始化
    /// </summary>
    private Length(double m) => _TotalMetre = m;



    /// <summary>
    /// 空
    /// </summary>
    public static Length Zero { get; } = new(0);


    #endregion

    #region --创建--

    #region --公制单位--

    /// <summary>
    /// 皮米 (pm)
    /// </summary>
    public static Length FromPicometer(double pm) => new(pm, MetreScale.Picometer);

    /// <summary>
    /// 纳米 nm
    /// </summary>
    public static Length FromNanometer(double nm) => new(nm, MetreScale.Nanometer);

    /// <summary>
    /// 微米 μm
    /// </summary>
    public static Length FromMicrometer(double μm) => new(μm, MetreScale.Micrometer);

    /// <summary>
    /// 毫米 (mm)
    /// </summary>
    public static Length FromMillimeter(double mm) => new(mm, MetreScale.Millimeter);

    /// <summary>
    /// 厘米 (cm)
    /// </summary>
    public static Length FromCentimeter(double cm) => new(cm, MetreScale.Centimeter);

    /// <summary>
    /// 分米 (dm)
    /// </summary>
    public static Length FromDecimeter(double dm) => new(dm, MetreScale.Decimeter);

    /// <summary>
    /// 米 (m)
    /// </summary>
    public static Length FromMetre(double m) => new(m, MetreScale.Metre);

    /// <summary>
    /// 千米 (km)
    /// </summary>
    public static Length FromKilometer(double km) => new(km, MetreScale.Kilometer);


    #endregion

    #region --天文单位

    /// <summary>
    /// 光秒 (ls)
    /// </summary>
    public static Length FromLightSecond(double ls) => new(ls, MetreScale.LightSecond);

    /// <summary>
    /// 光年 (ly)
    /// </summary>
    public static Length FromLightYear(double ly) => new(ly, MetreScale.LightYear);

    /// <summary>
    /// 秒差距（pc）
    /// </summary>
    public static Length FromParsec(double pc) => new(pc, MetreScale.Parsec);

    #endregion

    #region --英制单位--

    /// <summary>
    /// 密耳 (thou)
    /// </summary>
    public static Length FromMil(double thou) => new(thou, MetreScale.Mil);

    /// <summary>
    /// 英寸 (in)
    /// </summary>
    public static Length FromInch(double @in) => new(@in, MetreScale.Inch);

    /// <summary>
    /// 英尺 (ft)
    /// </summary>
    public static Length FromFoot(double ft) => new(ft, MetreScale.Foot);

    /// <summary>
    /// 码 (yd)
    /// </summary>
    public static Length FromYard(double yd) => new(yd, MetreScale.Yard);

    /// <summary>
    /// 英寻 (Fathom)
    /// </summary>
    public static Length FromFathom(double fathom) => new(fathom, MetreScale.Fathom);

    /// <summary>
    /// 迈|英里 (mi)
    /// </summary>
    public static Length FromMile(double mi) => new(mi, MetreScale.Mile);

    /// <summary>
    /// 海里 (nmi)
    /// </summary>
    public static Length FromNauticalMile(double nmi) => new(nmi, MetreScale.NauticalMile);

    /// <summary>
    /// 弗隆 (Furlong)
    /// </summary>
    public static Length FromFurlong(double furlong) => new(furlong, MetreScale.Furlong);

    #endregion

    #region --中国单位--

    /// <summary>
    /// 里 (Lǐ)
    /// </summary>
    public static Length FromLǐ(double li) => new(li, MetreScale.Lǐ);

    /// <summary>
    /// 丈 (Zhàng)
    /// </summary>
    public static Length FromZhàng(double zhang) => new(zhang, MetreScale.Zhàng);

    /// <summary>
    /// 尺 (Chǐ)
    /// </summary>
    public static Length FromChǐ(double chi) => new(chi, MetreScale.Chǐ);

    /// <summary>
    /// 寸 (Cùn)
    /// </summary>
    public static Length FromCùn(double cun) => new(cun, MetreScale.Cùn);

    /// <summary>
    /// 分 (Fēn)
    /// </summary>
    public static Length FromFēn(double fen) => new(fen, MetreScale.Fēn);

    /// <summary>
    /// 厘 (Lí)
    /// </summary>
    public static Length FromLí(double li) => new(li, MetreScale.Lí);

    /// <summary>
    /// 毫 (Háo)
    /// </summary>
    public static Length FromHáo(double hao) => new(hao, MetreScale.Háo);


    #endregion

    #endregion

    #region --转换--

    #region --公制单位--

    /// <summary>
    /// 转为皮米 (pm)
    /// </summary>
    public double ToPicometer() => _TotalMetre / MetreScale.Picometer;

    /// <summary>
    /// 转为纳米 (nm)
    /// </summary>
    public double ToNanometer() => _TotalMetre / MetreScale.Nanometer;

    /// <summary>
    /// 转为微米 (μm)
    /// </summary>
    public double ToMicrometer() => _TotalMetre / MetreScale.Micrometer;

    /// <summary>
    /// 转为毫米 (mm)
    /// </summary>
    public double ToMillimeter() => _TotalMetre / MetreScale.Millimeter;

    /// <summary>
    /// 转为厘米 (cm)
    /// </summary>
    public double ToCentimeter() => _TotalMetre / MetreScale.Centimeter;

    /// <summary>
    /// 转为分米 (dm)
    /// </summary>
    public double ToDecimeter() => _TotalMetre / MetreScale.Decimeter;

    /// <summary>
    /// 转为米 (m)
    /// </summary>
    public double ToMetre() => _TotalMetre;

    /// <summary>
    /// 转为千米 (km)
    /// </summary>
    public double ToKilometer() => _TotalMetre / MetreScale.Kilometer;


    #endregion

    #region --天文单位--

    /// <summary>
    /// 转为光秒 (ls)
    /// </summary>
    public double ToLightSecond() => _TotalMetre / MetreScale.LightSecond;

    /// <summary>
    /// 转为光年 (ly)
    /// </summary>
    public double ToLightYear() => _TotalMetre / MetreScale.LightYear;

    /// <summary>
    /// 转为秒差距（pc）
    /// </summary>
    public double ToParsec() => _TotalMetre / MetreScale.Parsec;

    #endregion

    #region --英制单位--

    /// <summary>
    /// 转为密耳 (thou)
    /// </summary>
    public double ToMil() => _TotalMetre / MetreScale.Mil;

    /// <summary>
    /// 转为英寸 (in)
    /// </summary>
    public double ToInch() => _TotalMetre / MetreScale.Inch;

    /// <summary>
    /// 转为英尺 (ft)
    /// </summary>
    public double ToFoot() => _TotalMetre / MetreScale.Foot;

    /// <summary>
    /// 转为码 (yd)
    /// </summary>
    public double ToYard() => _TotalMetre / MetreScale.Yard;

    /// <summary>
    /// 转为英寻 (Fathom)
    /// </summary>
    public double ToFathom() => _TotalMetre / MetreScale.Fathom;

    /// <summary>
    /// 转为迈|英里 (mi)
    /// </summary>
    public double ToMile() => _TotalMetre / MetreScale.Mile;

    /// <summary>
    /// 转为海里 (nmi)
    /// </summary>
    public double ToNauticalMile() => _TotalMetre / MetreScale.NauticalMile;

    /// <summary>
    /// 转为弗隆 (Furlong)
    /// </summary>
    public double ToFurlong() => _TotalMetre / MetreScale.Furlong;

    #endregion

    #region --中国单位--

    /// <summary>
    /// 转为里 (Lǐ)
    /// </summary>
    public double ToLǐ() => _TotalMetre / MetreScale.Lǐ;

    /// <summary>
    /// 转为丈 (Zhàng)
    /// </summary>
    public double ToZhàng() => _TotalMetre / MetreScale.Zhàng;

    /// <summary>
    /// 转为尺 (Chǐ)
    /// </summary>
    public double ToChǐ() => _TotalMetre / MetreScale.Chǐ;

    /// <summary>
    /// 转为寸 (Cùn)
    /// </summary>
    public double ToCùn() => _TotalMetre / MetreScale.Cùn;

    /// <summary>
    /// 转为分 (Fēn)
    /// </summary>
    public double ToFēn() => _TotalMetre / MetreScale.Fēn;

    /// <summary>
    /// 转为厘 (Lí)
    /// </summary>
    public double ToLí() => _TotalMetre / MetreScale.Lí;

    /// <summary>
    /// 转为毫 (Háo)
    /// </summary>
    public double ToHáo() => _TotalMetre / MetreScale.Háo;


    #endregion

    #endregion

    #region --运算符重载--

    public static Length operator +(Length left, Length right)
    {
        return new(left._TotalMetre + right._TotalMetre);
    }

    public static Length operator -(Length left, Length right)
    {
        return new(left._TotalMetre - right._TotalMetre);
    }



    public static Length operator *(Length left, Length right)
    {
        return new(left._TotalMetre * right._TotalMetre);
    }

    public static Length operator *(Length left, double right)
    {
        return new(left._TotalMetre * right);
    }

    public static Length operator *(double left, Length right)
    {
        return new(left * right._TotalMetre);
    }




    public static Length operator /(Length left, Length right)
    {
        return new(left._TotalMetre / right._TotalMetre);
    }

    public static Length operator /(Length left, double right)
    {
        return new(left._TotalMetre / right);
    }

    public static Length operator /(double left, Length right)
    {
        return new(left / right._TotalMetre);
    }



    public static Speed operator /(Length left, TimeSpan right)
    {
        return Speed.FromTime(left, right);
    }



    public static Length operator ^(Length left, double right)
    {
        return new(System.Math.Pow(left._TotalMetre, right));
    }




    public static bool operator >(Length left, Length right)
    {
        return left._TotalMetre > right._TotalMetre;
    }

    public static bool operator <(Length left, Length right)
    {
        return left._TotalMetre < right._TotalMetre;
    }

    public static bool operator ==(Length left, Length right)
    {
        return left._TotalMetre == right._TotalMetre;
    }

    public static bool operator !=(Length left, Length right)
    {
        return left._TotalMetre != right._TotalMetre;
    }




    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null)
        {
            return false;
        }

        if (obj is Length len)
        {
            return _TotalMetre == len._TotalMetre;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"{_TotalMetre} 米";
    }

    #endregion
}
