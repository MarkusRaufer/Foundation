using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public class LambdaEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T?, int> _hashCodeFunc;
    private readonly Func<T?, T?, bool> _predicate;

    /// <summary>
    /// Default hashCodeFunc is DefaultHashCodeFunc.
    /// </summary>
    /// <param name="predicate"></param>
    public LambdaEqualityComparer(Func<T?, T?, bool> predicate) : this(predicate, DefaultHashCodeFunc)
    {
    }

    /// <summary>
    /// Default predicate uses Equals method.
    /// </summary>
    /// <param name="hashCodeFunc"></param>
    public LambdaEqualityComparer(Func<T?, int>? hashCodeFunc)
    {
        _hashCodeFunc = hashCodeFunc.ThrowIfNull();

        _predicate = (a, b) =>
        {
            if (null == a) return null == b;

            return a.Equals(b);
        };
    }

    public LambdaEqualityComparer([DisallowNull] Func<T?, T?, bool> predicate, Func<T?, int>? hashCodeFunc = null)
    {
        _predicate = predicate.ThrowIfNull();
        _hashCodeFunc = hashCodeFunc ?? DefaultHashCodeFunc;
    }

    public static Func<T?, int> DefaultHashCodeFunc { get; } = (t => null == t ? 0 : t.GetHashCode());

    public bool Equals(T? x, T? y)
    {
        if (x is null) return y is null;
        if (y is null) return false;

        return _predicate(x, y);
    }


    public int GetHashCode([DisallowNull] T obj) => _hashCodeFunc(obj);
}


