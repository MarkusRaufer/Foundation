namespace Foundation;

public static class Is
{
    /// <summary>
    /// Creates a Between range expression. 
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="from">Min.</param>
    /// <param name="to">Max.</param>
    /// <returns></returns>
    public static IRangeExpression<T> Between<T>(T from, T to)
        where T : struct, IComparable<T>, IEquatable<T>
    {
        return new Between<T>(from, to);
    }

    /// <summary>
    /// Creates a Between range expression. 
    /// </summary>
    /// <param name="range">includes the min and max value.</param>
    /// <returns></returns>
    public static IRangeExpression<int> Between(System.Range range)
    {
        var min = range.Start.IsFromEnd ? 0 : range.Start.Value;
        var max = range.End.IsFromEnd ? int.MaxValue : range.End.Value;
        return Between<int>(min, max);
    }

    /// <summary>
    /// Creates an Exactly range expression.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">A value.</param>
    /// <returns></returns>
    public static IRangeExpression<T> Exactly<T>(T value)
        where T : IComparable<T>, IEquatable<T>
    {
        return new Exactly<T>(value);
    }

    /// <summary>
    /// Creates a Matching range expression.
    /// </summary>
    /// <typeparam name="T">Type of the compare value.</typeparam>
    /// <param name="predicate">The match predicate.</param>
    /// <returns></returns>
    public static IRangeExpression<T> Matching<T>(Func<T, bool> predicate)
        where T : IComparable<T>, IEquatable<T>
    {
        return new Matching<T>(predicate);
    }

    /// <summary>
    /// Creates a NumericBetween range expression.
    /// </summary>
    /// <typeparam name="T">Type of the values.</typeparam>
    /// <param name="from">Min.</param>
    /// <param name="to">Max.</param>
    /// <returns></returns>
    public static IRangeExpression<T> NumericBetween<T>(T from, T to)
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        return new NumericBetween<T>(from, to);
    }

    /// <summary>
    /// Creates an OneOf range expression.
    /// </summary>
    /// <typeparam name="T">Type of the values.</typeparam>
    /// <param name="values">Values to compare.</param>
    /// <returns></returns>
    public static IRangeExpression<T> OneOf<T>(params T[] values)
        where T : IComparable<T>, IEquatable<T>
    {
        return new OneOf<T>(values);
    }

    /// <summary>
    /// Creates an TypeOf range expression.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">value to compare.</param>
    /// <returns></returns>
    public static IRangeExpression<object> OfType<T>()
    {
        return new OfType<T>();
    }
}

