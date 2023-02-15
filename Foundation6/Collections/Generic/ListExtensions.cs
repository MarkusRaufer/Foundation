namespace Foundation.Collections.Generic;

public static class ListExtensions
{
    public static bool TryGet<T>(this IList<T> list, Func<T, bool> predicate, out T? value)
    {
        var index = list.IndexOf(predicate);
        if(-1 == index)
        {
            value = default;
            return false;
        }
        value = list[index];
        return true;
    }
}
