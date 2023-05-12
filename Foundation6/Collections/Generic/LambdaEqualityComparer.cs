using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public class LambdaEqualityComparer<T> : EqualityComparer<T>
{
    private readonly Func<T?, int> _hashCodeFunc;
    private readonly Func<T?, T?, bool> _equals;

    /// <summary>
    /// Default hashCodeFunc is DefaultHashCodeFunc.
    /// </summary>
    /// <param name="equals"></param>
    public LambdaEqualityComparer(Func<T?, T?, bool> equals) : this(equals, DefaultHashCodeFunc)
    {
    }

    /// <summary>
    /// Default predicate uses Equals method.
    /// </summary>
    /// <param name="hashCodeFunc"></param>
    public LambdaEqualityComparer(Func<T?, int>? hashCodeFunc) : this(DefaultEquals, hashCodeFunc)
    {
    }

    public LambdaEqualityComparer(Func<T?, T?, bool> equals, Func<T?, int>? hashCodeFunc)
    {
        _equals = equals.ThrowIfNull();
        _hashCodeFunc = hashCodeFunc.ThrowIfNull();
    }

    public static Func<T?, int> DefaultHashCodeFunc { get; } = x => x.GetNullableHashCode();

    public static Func<T?, T?, bool> DefaultEquals { get; } =
        (x, y) =>
        {
            if (null == x) return null == y;
            if (null == y) return false;

            return x.Equals(y);
        };

    public override bool Equals(T? x, T? y) => _equals(x, y);

    public override int GetHashCode([DisallowNull] T obj) => _hashCodeFunc(obj);
}

public class LambdaEqualityComparer<T, TKey> : IEqualityComparer<T>
{
    private readonly Func<TKey?, int> _hashCodeFunc;
    private readonly Func<T, TKey?> _selector;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="selector">value selector for equation and hashcode.</param>
    /// <param name="hashCodeFunc">a hashcode function</param>
    public LambdaEqualityComparer(Func<T, TKey?> selector, Func<TKey?, int>? hashCodeFunc = null)
    {
        _selector = selector.ThrowIfNull();
        _hashCodeFunc = hashCodeFunc ?? DefaultHashCodeFunc;
    }

    public static Func<TKey?, int> DefaultHashCodeFunc { get; } = x => x.GetNullableHashCode();

    public bool Equals(T? x, T? y)
    {
        if (null == x) return null == y;
        if (null == y) return false;
        var eq = _selector(x).EqualsNullable(_selector(y));
        return eq;
    }

    public int GetHashCode([DisallowNull] T obj) => _hashCodeFunc(_selector(obj));
}
