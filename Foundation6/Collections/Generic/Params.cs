using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class Params
{
    public static Params<object> New(params object[] values) => new(values);
    public static Params<T> New<T>(params T[] values) => new(values);
}

public struct Params<T> 
    : IEnumerable<T>
    , IEquatable<Params<T>>
{
    private readonly int _hashCode;
    private readonly bool _isInitialized;
    private readonly ICollection<T> _values;

    public Params(params T[] values)
    {
        values.ThrowIfNull();

        _values = values;
        _hashCode = HashCode.FromObject(_values);
        _isInitialized = true;
    }


    public static bool operator ==(Params<T> left, Params<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Params<T> left, Params<T> right)
    {
        return !(left == right);
    }

    public int Count => _values.Count;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Params<T> other && Equals(other);

    public bool Equals(Params<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && _values.EqualsCollection(other._values);
    }

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => !_isInitialized;

    public override string ToString() => string.Join(", ", _values);
}
