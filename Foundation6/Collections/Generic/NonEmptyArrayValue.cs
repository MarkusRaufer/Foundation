namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class NonEmptyArrayValue
{
    public static NonEmptyArrayValue<T> New<T>(params T[] values) =>
        new (values);

    public static NonEmptyArrayValue<T> New<T>(string separator, params T[] values)
        => new (values, separator);
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(ArrayValue{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct NonEmptyArrayValue<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<NonEmptyArrayValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly string _separator;
    private readonly T[] _values;

    public NonEmptyArrayValue(T[] values, string separator = ", ")
    {
        _values = values.ThrowIfNullOrEmpty();
        _separator = separator ?? throw new ArgumentNullException(nameof(separator));
        _hashCode = HashCode.FromObjects(_values);
    }

    public static implicit operator NonEmptyArrayValue<T>(T[] array) => NonEmptyArrayValue.New(array);

    public static implicit operator T[](NonEmptyArrayValue<T> array) => array._values;

    public static bool operator ==(NonEmptyArrayValue<T> left, NonEmptyArrayValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptyArrayValue<T> left, NonEmptyArrayValue<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => _values[index];

    public object Clone()
    {
        return IsEmpty
            ? new ArrayValue<T>(Array.Empty<T>(), _separator)
            : new ArrayValue<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is NonEmptyArrayValue<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.SequenceEqual(other);
    }

    public bool Equals(NonEmptyArrayValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.SequenceEqual(other._values);
    }

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator<T>();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => null == _values || 0 == _values.Length;

    public int Length => IsEmpty ? 0 : _values.Length;
    
    public override string? ToString()
    {
        return IsEmpty ? "" : string.Join(_separator, _values);
    }
}
