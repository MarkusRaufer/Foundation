namespace Foundation.Collections;

using Foundation.Collections.Generic;
using System.Collections;

public static class EnumerableExtensions
{
    public static IEnumerable AfterEveryObject(this IEnumerable items, Action action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        var next = it.MoveNext();
        while (next)
        {
            yield return it.Current;
            next = it.MoveNext();
            if (next)
                action();
        }
    }

    /// <summary>
    /// Like <see cref="Any"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool AnyObject(this IEnumerable items)
    {
        items.ThrowIfNull();

        return items.GetEnumerator().MoveNext();
    }

    /// <summary>
    /// Like <see cref="=Any"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool AnyObject(this IEnumerable items, Func<object, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item))
                return true;
        }
        return false;
    }

    /// <summary>
    /// List <see cref="=CastTo<typeparamref name="T"/>"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> CastTo<T>(this IEnumerable items)
    {
        if(null == items) return Enumerable.Empty<T>();

        return items.SelectObject(obj => (T)obj);
    }

    /// <summary>
    /// Returns the number of elements in items.
    /// </summary>
    /// <param name="items">List of items.</param>
    /// <returns>Number of elements in list.</returns>
    public static int Count(this IEnumerable items)
    {
        if (null == items) return 0;

        var count = 0;
        foreach(var item in items)
            count++;

        return count;
    }

    /// <summary>
    /// Like <see cref="=First"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object FirstObject(this IEnumerable items)
    {
        items.ThrowIfNull();
        var item = FirstOrDefaultObject(items);

        if (!item.TryGet(out object? value)) throw new InvalidOperationException("sequence is emtpy");

        return value!;
    }

    /// <summary>
    /// Like <see cref="=FirstOrDefault"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    private static Option<object> FirstOrDefaultObject(this IEnumerable items)
    {
        items.ThrowIfNull();

        var enumerator = items.GetEnumerator();
        if (enumerator.MoveNext())
        {
            return Option.Some(enumerator.Current);
        }

        return Option.None<object>();
    }

    /// <summary>
    /// Like <see cref="ForEach"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="action"></param>
    public static void ForEachObject(this IEnumerable items, Action<object> action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        foreach (var item in items)
            action(item);
    }

    /// <summary>
    /// Ignores item if predicate returns true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item))
                continue;

            yield return item;
        }
    }

    /// <summary>
    /// Ignores items at indicies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="indices"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, params int[] indices)
    {
        items.ThrowIfNull();
        if (0 == indices.Length) yield break;

        var i = 0;
        foreach (var item in items)
        {
            if (indices.Contains(i))
                continue;

            yield return item;
            i++;
        }
    }

    /// <summary>
    /// Like <see cref="=OfType<typeparamref name="T"/>"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> OfType<T>(this IEnumerable items)
    {
        foreach (var item in items.ThrowIfNull())
        {
            if (item is T t) yield return t;
        }
    }

    public static IEnumerable<object> OfTypes(this IEnumerable items, params Type[] types)
    {
        items = items.ThrowIfNull();
        types.ThrowIfOutOfRange(() => types.Length == 0);

        foreach (var item in items)
        {
            if(null == item) continue;

            var type = item.GetType();
            if (types.Any(t => t.Equals(type) || t.IsAssignableFrom(type)))
                yield return item;
            
        }
    }

    public static IEnumerable<T> OfTypes<T>(this IEnumerable items, params Type[] types)
    {
        items.ThrowIfNull();
        foreach (var item in items.OfTypes(types))
        {
            if (item is T t) yield return t;
        }
    }

    /// <summary>
    /// Returns all items that are not null.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable SelectNotNull(this IEnumerable items)
    {
        if(items is null) yield break;

        foreach (object item in items)
        {
            if (null != item) yield return item;
        }
    }

    /// <summary>
    /// Like <see cref="=Select<typeparamref name="T"/>"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable SelectObject(this IEnumerable items, Func<object, object> selector)
    {
        items.ThrowIfNull();
        selector.ThrowIfNull();

        foreach (var item in items)
            yield return selector(item);
    }

    /// <summary>
    /// Like <see cref="=Select<typeparamref name="T"/>"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<T> SelectObject<T>(this IEnumerable items, Func<object, T> selector)
    {
        items.ThrowIfNull();
        selector.ThrowIfNull();

        foreach (var item in items)
            yield return selector(item);
    }

    public static IEnumerable SelectObjectByIndex(this IEnumerable items, Func<long, bool> selector)
    {
        items.ThrowIfNull();
        selector.ThrowIfNull();

        long i = 0;
        return items.WhereObject(item => selector(i++));
    }

    /// <summary>
    /// Like <see cref="=Single<typeparamref name="T"/>"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object SingleObject(this IEnumerable items)
    {
        items.ThrowIfNull();

        var enumerator = items.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("no element");

        var current = enumerator.Current;
        if (enumerator.MoveNext()) throw new InvalidOperationException("more than one element");

        return current;
    }

    /// <summary>
    /// Convert to typed enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> ToEnumerable<T>(this IEnumerable items) => items.CastTo<T>();

    public static IList<T> ToList<T>(this IEnumerable items)
    {
        var list = new List<T>();
        foreach (var item in items)
            list.Add((T)item);

        return list;
    }

    public static object?[] ToObjectArray(this IEnumerable items)
    {
        var list = new ArrayList();
        items.ForEachObject(i => list.Add(i));
        return list.ToArray();
    }

    public static IList ToObjectList(this IEnumerable items)
    {
        var list = new ArrayList();
        items.ForEachObject(i => list.Add(i));
        return list;
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items)
    {
        return ReadOnlyCollection.New(items);
    }
}

