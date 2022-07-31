namespace Foundation;

/// <summary>
/// This class can be used to avoid state.
/// </summary>
public static class Scope
{
    /// <summary>
    /// Returns an option from a scope. This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static Option<T> MayReturn<T>(Func<T> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns an nullable from a scope. This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static T? ReturnsNullable<T>(Func<T?> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns a list of values from a scope. This can be used to avoid state. This enables e.g. return values from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static IEnumerable<T> ReturnsMany<T>(Func<IEnumerable<T>> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns a value from a scope. This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static T Returns<T>(Func<T> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }
}
