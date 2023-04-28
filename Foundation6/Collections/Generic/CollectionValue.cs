namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class CollectionValue
{
    public static CollectionValue<T> New<T>(params T[] values)
    {
        return new CollectionValue<T>(values);
    }
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(CollectionValue{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct CollectionValue<T>
    : ICloneable
    , IReadOnlyCollection<T>
    , IEquatable<CollectionValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly T[] _values;

    public CollectionValue(IEnumerable<T> values)
        : this(values.ToArray())
    {

    }
    public CollectionValue(T[] values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    public static implicit operator CollectionValue<T>(T[] array) => CollectionValue.New(array);

    public static implicit operator T[](CollectionValue<T> array) => array._values;

    public static bool operator ==(CollectionValue<T> left, CollectionValue<T> right) => left.Equals(right);

    public static bool operator !=(CollectionValue<T> left, CollectionValue<T> right) => !(left == right);

    public int Count => IsEmpty ? 0 : _values.Length;

    public object Clone()
    {
        return IsEmpty
            ? new ArrayValue<T>(Array.Empty<T>())
            : new ArrayValue<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CollectionValue<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.IsEqualToSet(other);
    }

    public bool Equals(CollectionValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.IsEqualToSet(other._values);
    }

    public IEnumerator<T> GetEnumerator() => IsEmpty ? Enumerable.Empty<T>().GetEnumerator() : _values.GetEnumerator<T>();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => null == _values || 0 == _values.Length;
}
