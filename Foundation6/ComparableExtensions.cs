namespace Foundation;

public static class ComparableExtensions
{
    /// <summary>
    /// Compare of possible null arguments.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="valueOnNull">In a sorted list it defines if the null values appear at the beginning -1 (Microsoft standard) or at the end 1.</param>
    /// <returns></returns>
    public static int CompareNullableTo(this IComparable? lhs, object? rhs, int valueOnNull = -1)
    {
        if (null == lhs) return null == rhs ? 0 : valueOnNull;
        if (null == rhs) return valueOnNull * -1;

        return lhs.CompareTo(rhs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="valueOnNull">In a sorted list it defines if the null values appear at the beginning -1 (Microsoft standard) or at the end 1.</param>
    /// <returns></returns>
    public static int CompareNullableTo<T>(this IComparable<T>? lhs, T? rhs, int valueOnNull = -1)
    {
        if (null == lhs) return null == rhs ? 0 : valueOnNull;
        if (null == rhs) return valueOnNull * -1;

        return lhs.CompareTo(rhs);
    }
}

