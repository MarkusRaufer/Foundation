namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class NonEmptyBagValue
{
    public static NonEmptyBagValue<T> New<T>(params T[] values)
    {
        return new NonEmptyBagValue<T>(values);
    }
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(NonEmptyBagValue{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct NonEmptyBagValue<T>
    : ICloneable
    , IReadOnlyCollection<T>
    , IEquatable<NonEmptyBagValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly T[] _values;

    public NonEmptyBagValue(IEnumerable<T> values)
        : this(values.ToArray())
    {

    }
    public NonEmptyBagValue(T[] values)
    {
        _values = values.ThrowIfNullOrEmpty();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    public static implicit operator NonEmptyBagValue<T>(T[] array) => NonEmptyBagValue.New(array);

    public static implicit operator T[](NonEmptyBagValue<T> array) => array._values;

    public static bool operator ==(NonEmptyBagValue<T> left, NonEmptyBagValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptyBagValue<T> left, NonEmptyBagValue<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => _values[index];

    public int Count => _values.Length;

    public object Clone()
    {
        return IsEmpty
            ? new ArrayValue<T>(Array.Empty<T>())
            : new ArrayValue<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is NonEmptyBagValue<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.IsEqualToSet(other);
    }

    public bool Equals(NonEmptyBagValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.IsEqualToSet(other._values);
    }

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator<T>();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => null == _values || 0 == _values.Length;

    public int Length => IsEmpty ? 0 : _values.Length;

    public override string ToString() => string.Join(", ", _values);
}
