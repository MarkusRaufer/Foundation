using System.Collections;

namespace Foundation.Collections;

public static class EnumerableEvents
{
    /// <summary>
    /// Returns true if items is empty.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsEmpty(this IEnumerable items)
    {
        return !items.GetEnumerator().MoveNext();
    }

    /// <summary>
    /// Returns true if items is null.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsNull(this IEnumerable items)
    {
        return null == items;

    }
    /// <summary>
    /// Returns true if items is null or empty
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this IEnumerable items)
    {
        if (IsNull(items)) return true;
        return IsEmpty(items);
    }

    /// <summary>
    /// Calls action on first item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable OnFirstObject(this IEnumerable items, Action action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext()) yield break;

        action();
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Calls action on first item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable OnFirstObject(this IEnumerable items, Action<object> action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext()) yield break;

        action(it.Current);
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Like <see cref="=Where"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable WhereObject(this IEnumerable items, Func<object, bool> selector)
    {
        selector.ThrowIfNull();

        foreach (var item in items)
        {
            if (selector(item))
                yield return item;
        }
    }
}
