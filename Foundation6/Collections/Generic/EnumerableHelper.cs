namespace Foundation.Collections.Generic;

public static class EnumerableHelper
{
    public static IEnumerable<T> AsEnumerable<T>(params T[] items)
    {
        return items;
    }
}

