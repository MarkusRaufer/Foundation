using System.Globalization;

namespace Foundation;

/// <summary>
/// Amount of money including currency
/// </summary>
/// <param name="RegionInfo"></param>
/// <param name="Amount"></param>
public sealed record Money(RegionInfo RegionInfo, decimal Amount) : IComparable<Money>
{
    public static Money New(string name, decimal amount) => new (new RegionInfo(name), amount);

    public static bool operator <(Money lhs, Money rhs) => -1 == lhs.CompareTo(rhs);
    public static bool operator <=(Money lhs, Money rhs) => 0 >= lhs.CompareTo(rhs);
    public static bool operator >(Money lhs, Money rhs) => 1 == lhs.CompareTo(rhs);
    public static bool operator >=(Money lhs, Money rhs) => 0 <= lhs.CompareTo(rhs);

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        var cmp = RegionInfo.Name.CompareTo(other.RegionInfo.Name);
        if (0 != cmp) return cmp;

        return Amount.CompareTo(other.Amount);
    }
}
