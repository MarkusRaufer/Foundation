using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class EnumerableConditionals
{
    private class ElseIf<T> : IElseIf<T>
    {
        private readonly IEnumerable<T> _items;

        public ElseIf(IEnumerable<T> items)
        {
            _items = items.ThrowIfNull();
        }

        public IEnumerable<T> Else() => _items;

        public IEnumerable<T> Else(Action<T> action)
        {
            foreach (var item in _items)
            {
                action(item);
                yield return item;
            }
        }

        public void EndIf()
        {
            foreach (var _ in Else())
            {
            }
        }

        IElseIf<T> IElseIf<T>.ElseIf(Func<T, bool> condition, Action<T> action)
        {
            return _items.If(condition, action);
        }
    }

    private class ElseResult<T, TResult> : IElse<T, TResult>
    {
        private readonly IEnumerable<T> _items;
        private readonly Func<T, bool> _predicate;
        private readonly Func<T, TResult> _mapIf;

        public ElseResult(
            IEnumerable<T> items,
            Func<T, bool> predicate,
            Func<T, TResult> mapIf)
        {
            _items = items.ThrowIfNull();
            _predicate = predicate.ThrowIfNull();
            _mapIf = mapIf.ThrowIfNull();
        }

        public IEnumerable<TResult> Else(Func<T, TResult> map)
        {
            foreach (var item in _items)
            {
                yield return _predicate(item) ? _mapIf(item) : map(item);
            }
        }
    }

    /// <summary>
    /// Adds an item if the list is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<T> AddIfEmpty<T>(this IEnumerable<T> items, Func<T> factory)
    {
        factory.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext())
        {
            yield return factory();
        }
        else
        {
            yield return it.Current;

            while (it.MoveNext()) yield return it.Current;
        }
    }

    /// <summary>
    /// Returns an empty enumerable if items is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? items)
    {
        return items ?? Enumerable.Empty<T>();
    }

    public static IElseIf<T> If<T>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Action<T> action)
    {
        predicate.ThrowIfNull();
        action.ThrowIfNull();

        var @else = Enumerable.Empty<T>();

        foreach (var item in items)
        {
            if (predicate(item))
            {
                action(item);
                continue;
            }
            @else = @else.Append(item);
        }
        return new ElseIf<T>(@else);
    }

    public static IElse<T, TResult> If<T, TResult>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Func<T, TResult> map)
    {
        predicate.ThrowIfNull();
        map.ThrowIfNull();

        return new ElseResult<T, TResult>(items, predicate, map);
    }

    /// <summary>
    /// If items is empty <paramref name="whenEmpty"/> is called otherwise <paramref name="whenNotEmpty"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="whenNotEmpty"></param>
    /// <param name="whenEmpty"></param>
    /// <returns></returns>
    public static TResult IfAny<T, TResult>(
        this IEnumerable<T> items,
        Func<IEnumerable<T>, TResult> whenNotEmpty,
        Func<IEnumerable<T>, TResult> whenEmpty)
    {
        return items.Any() ? whenNotEmpty(items) : whenEmpty(items);
    }

    /// <summary>
    /// Returns alternative elements if enumerable is empty..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs">if not empty this elements are the result</param>
    /// <param name="rhs">Alternative elements are the result if lhs is empty.</param>
    /// <returns></returns>
    public static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        rhs.ThrowIfNull();

        var lIt = lhs.GetEnumerator();
        if (!lIt.MoveNext())
        {
            foreach (var r in rhs)
            {
                yield return r;
            }
            yield break;
        }

        yield return lIt.Current;

        while (lIt.MoveNext())
        {
            yield return lIt.Current;
        }
    }

    /// <summary>
    /// If items is empty <paramref name="whenEmpty" /> otherwise items are returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="whenEmpty">Is called when items is empty.</param>
    /// <returns></returns>
    public static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> items, Func<IEnumerable<T>> whenEmpty)
    {
        whenEmpty.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext())
        {
            foreach (var x in whenEmpty())
            {
                yield return x;
            }
            yield break;
        }

        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// If items is empty <paramref name="whenEmpty"/> is called otherwise <paramref name="whenNotEmpty"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="whenEmpty"></param>
    /// <param name="whenNotEmpty"></param>
    /// <returns></returns>
    public static TResult IfEmpty<T, TResult>(
        this IEnumerable<T> items,
        Func<IEnumerable<T>, TResult> whenEmpty,
        Func<IEnumerable<T>, TResult> whenNotEmpty)
    {
        return items.Any() ? whenNotEmpty(items) : whenEmpty(items);
    }

    public static IEnumerable<T> IfMoreOrEqualThan<T>(this IEnumerable<T> items, int numberOfItems)
    {
        var it = items.ThrowIfNull().GetEnumerator();
        if (0 >= numberOfItems) yield break;

        var minimum = new List<T>();
        while (it.MoveNext())
        {
            minimum.Add(it.Current);
            if (minimum.Count == numberOfItems) break;
        }

        if (0 == minimum.Count || minimum.Count < numberOfItems) yield break;

        foreach (var item in minimum)
            yield return item;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    public static IEnumerable<T> IfMoreThan<T>(this IEnumerable<T> items, int numberOfItems)
    {
        var it = items.ThrowIfNull().GetEnumerator();
        if (0 >= numberOfItems) yield break;

        var minimum = new List<T>();
        while(it.MoveNext())
        {
            minimum.Add(it.Current);
            if (minimum.Count > numberOfItems) break;
        }

        if (0 == minimum.Count) yield break;

        foreach (var item in minimum)
            yield return item;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Returns true if items is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? items)
    {
        if (items == null) return true;
        return !items.Any();
    }

    /// <summary>
    /// Checks if <paramref name="rhs"/> is a subset of <paramref name="lhs"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsSubsetOf<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        rhs.ThrowIfNull();

        var search = new HashSet<T>(lhs);
        return search.IsSubsetOf(rhs);
    }

    /// <summary>
    /// Throws an ArgumentNullException if an element of the enumerable is null.
    /// Use this method only for value objects with small collections because the check is done in an eager way.
    /// Don't use <see cref="ThrowIfElementNull"/> for general collections because of performance reasons and also to keep the collection lazy. 
    /// Attention: This method runs into an endless loop when using with a generator!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> ThrowIfElementNull<T>(this IEnumerable<T> items)
    {
        return items.Any(x => null == x) ? throw new ArgumentNullException(nameof(items)) : items;
    }

    /// <summary>
    /// Throws an ArgumentNullException if the enumerable is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string name = "")
    {
        return ThrowIfEmpty(items, () => new ArgumentOutOfRangeException(name));
    }

    /// <summary>
    /// Throws an Exception if items is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="exceptionFactory"></param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfEmpty<T>(this IEnumerable<T> items, Func<Exception> exceptionFactory)
    {
        if (!items.Any())
        {
            var exception = exceptionFactory() ?? throw new ArgumentNullException("returned null", nameof(exceptionFactory));
            throw exception;
        }
        return items;
    }

    /// <summary>
    /// Throws an Exception if items is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfNull<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string name = "")
    {
        return ThrowIfNull(items, () => new ArgumentNullException(name));
    }

    /// <summary>
    /// Throws an Exception if items is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="exceptionFactory">This creates an exception when items is null.</param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfNull<T>(this IEnumerable<T> items, Func<Exception> exceptionFactory)
    {
        if (items is null) throw exceptionFactory() ?? throw new ArgumentNullException("returned null", nameof(exceptionFactory));

        return items;
    }

    /// <summary>
    /// Throws an Exception if items is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string name = "")
    {
        return items.ThrowIfNull()
                    .ThrowIfEmpty();
    }

    /// <summary>
    /// Throws an Exception if items is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="exceptionFactory"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> items, Func<Exception> exceptionFactory)
    {
        exceptionFactory.ThrowIfNull();

        return items.ThrowIfNull(exceptionFactory)
                    .ThrowIfEmpty(exceptionFactory);
    }

    /// <summary>
    /// Returns <paramref name="numberOfElements"/> if items contains exactly <paramref name="numberOfElements"/> or throws an excption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="numberOfElements"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">is thrown when number of elements differs from  <paramref name="numberOfElements"/></exception>
    public static IEnumerable<T> ThrowIfNumberNotExact<T>(this IEnumerable<T> items, int numberOfElements)
    {
        if (!items.TakeExact(numberOfElements).Any())
            throw new ArgumentException($"items does not have exact {numberOfElements} elements");

        return items;
    }

    /// <summary>
    /// Returns an empty enumerable if items is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T>? items)
    {
        return items ?? Enumerable.Empty<T>();
    }
}

public interface IElseIf<T>
{
    IEnumerable<T> Else();
    IEnumerable<T> Else(Action<T> action);
    IElseIf<T> ElseIf(Func<T, bool> condition, Action<T> action);
    void EndIf();
}

public interface IElse<T, TResult>
{
    IEnumerable<TResult> Else(Func<T, TResult> map);
}
