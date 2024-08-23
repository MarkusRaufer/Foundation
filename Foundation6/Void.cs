namespace Foundation;
public static class Void
{
    /// <summary>
    /// Enables a void method to return always a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="value">The value which will be returned after execution.</param>
    /// <param name="action">The action which should be executed.</param>
    /// <returns>The func.</returns>
    public static T Returns<T>(T value, Action action)
    {
        action.ThrowIfNull();
        action();
        return value;
    }

    /// <summary>
    /// Enables a void method to return always a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="func">The delegate which will return a value after execution.</param>
    /// <param name="action">The action which should be executed.</param>
    /// <returns>The func.</returns>
    public static T Returns<T>(Func<T> func, Action action)
    {
        action.ThrowIfNull();
        action();
        return func();
    }

    /// <summary>
    /// Enables a void method to return always a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="func">The delegate which will return a value after execution.</param>
    /// <param name="action">The action which should be executed.</param>
    /// <returns>The func.</returns>
    public static T Returns<T>(Func<T> func, Action<T> action)
    {
        action.ThrowIfNull();
        var value = func();
        action(value);
        return value;
    }
}
