namespace Foundation.Collections.Generic;

public static class EnumerableEvents
{

    /// <summary>
    /// Executes action after every item except the last one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> AfterEach<T>(this IEnumerable<T> items, Action action)
    {
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
    /// Executes action after every item except the last one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action">The parameter is the item before the action.</param>
    /// <returns>List of items</returns>
    public static IEnumerable<T> AfterEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        var next = it.MoveNext();
        while (next)
        {
            var prevItem = it.Current;
            yield return prevItem;

            next = it.MoveNext();
            if (next)
                action(prevItem);
        }
    }

    /// <summary>
    /// Executes action after first item except there is no second item.
    /// </summary>
    /// <typeparam name="T">The type of the items</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="action">The action which is executed after the first item.</param>
    /// <returns></returns>
    public static IEnumerable<T> AfterFirst<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;
        var first = it.Current;
        yield return first;

        if (!it.MoveNext()) yield break;
        action();
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Executes action after first item only if there is a second item.
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="action">The action which is executed after the first item.
    /// The argument is the first item.</param>
    /// <returns></returns>
    public static IEnumerable<T> AfterFirst<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;
        var first = it.Current;
        yield return first;

        if (!it.MoveNext()) yield break;
        action(first);
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Calls action on each element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEach<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        foreach (var item in items.ThrowIfNull())
        {
            action();
            yield return item;
        }
    }

    /// <summary>
    /// Calls action on each element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        foreach (var item in items.ThrowIfNull())
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Calls action if items is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEmpty<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext())
        {
            action();
            yield break;
        }
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// If list is empty onEmpty is called. After returning the single value the iteration stops.
    /// If the list is not empty it behaves as normal IEnumerable<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="onEmpty">Factory method which is called if list of items is empty.</param>
    /// <returns>List of items.</returns>
    public static IEnumerable<T> OnEmpty<T>(this IEnumerable<T> items, Func<T> onEmpty)
    {
        onEmpty.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext())
        {
            yield return onEmpty();
            yield break;
        }
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Returns a value if list is empty. After returning the single value the iteration stops.
    /// If the list is not empty it behaves as normal IEnumerable<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <typeparam name="TResult">Type of returned items.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="onEmpty">Is called if list is empty.</param>
    /// <param name="onNotEmpty">Is called if list is not empty.</param>
    /// <returns>List of items.</returns>
    public static IEnumerable<TResult> OnEmpty<T, TResult>(this IEnumerable<T> items, Func<TResult> onEmpty, Func<T, TResult> onNotEmpty)
    {
        onEmpty.ThrowIfNull();
        onNotEmpty.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext())
        {
            yield return onEmpty();
            yield break;
        }
        yield return onNotEmpty(it.Current);

        while (it.MoveNext())
        {
            yield return onNotEmpty(it.Current);
        }
    }

    /// <summary>
    /// Calls action on first item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;
        action();
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Is called on first item.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="action">Is called on first item.</param>
    /// <returns></returns>
    public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;

        action(it.Current);
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Calls action on last item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnLast<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (it.MoveNext())
        {
            while (true)
            {
                yield return it.Current;
                if (!it.MoveNext())
                {
                    action();
                    yield break;
                }
            }
        }
    }

    /// <summary>
    /// Calls action on last item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnLast<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (it.MoveNext())
        {
            while (true)
            {
                var last = it.Current;
                yield return last;
                if (!it.MoveNext())
                {
                    action(last);
                    yield break;
                }
            }
        }
    }

    /// <summary>
    /// Calls action when reached an index during iteration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="index"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnNth<T>(this IEnumerable<T> items, int index, Action<T> action)
    {
        action.ThrowIfNull();

        var counter = 0;
        foreach (var item in items.ThrowIfNull())
        {
            if (index == counter)
                action(item);

            yield return item;
            counter++;
        }
    }
}
