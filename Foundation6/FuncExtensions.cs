namespace Foundation;

public static class FuncExtensions
{
    public static Func<T, bool> And<T>(this Func<T, bool> first, params Func<T, bool>[] predicates)
    {
        return t => first(t) && predicates.All(predicate => predicate(t));
    }

    public static Func<T, bool> Or<T>(this Func<T, bool> first, params Func<T, bool>[] predicates)
    {
        return t => first(t) || predicates.Any(predicate => predicate(t));
    }

    /// <summary>
    /// Converts a Func{T} to Func{object}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Func<object?> ToObjectFunc<T>(this Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return () => func();
    }

    /// <summary>
    /// Converts a Func{T, bool} to Func{object, bool}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Func<object?, object?> ToObjectFunc<T, TResult>(this Func<T, TResult> func)
    {
        return x => x is T t ? func(t) : null;
    }

    /// <summary>
    /// Converts a Func{T, bool} to Func{object, bool}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Func<object, bool> ToObjectPredicate<T>(this Func<T, bool> func)
    {
        return v => func((T)v);
    }
}

