namespace Foundation;

public static class MathExt
{
    public static (T min, T max) MinMax<T>(T lhs, T rhs)
        where T : IComparable<T>
    {
        var cmp = lhs.CompareNullableTo(rhs);

        return 1 > cmp ? (lhs, rhs) : (rhs, lhs);
    }
}
