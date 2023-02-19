namespace Foundation.Collections.Generic;

public static class ListExtensions
{
    /// <summary>
    /// Swap values at indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="lhsIndex"></param>
    /// <param name="rhsIndex"></param>
    public static void Swap<T>(this IList<T> items, int lhsIndex, int rhsIndex)
    {
        var temp = items[lhsIndex];
        items[lhsIndex] = items[rhsIndex];
        items[rhsIndex] = temp;
    }

    /// <summary>
    /// Picks an element of a list which maches a predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="predicate"></param>
    /// <param name="value"></param>
    /// <returns>true if element found, otherwise false.</returns>
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
