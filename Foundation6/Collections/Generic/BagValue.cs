namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class BagValue
{
    public static BagValue<T> New<T>(params T[] values)
    {
        return new BagValue<T>(values);
    }
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(BagValue{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct BagValue<T>
    : ICloneable
    , IReadOnlyCollection<T>
    , IEquatable<BagValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly T[] _values;

    public BagValue(IEnumerable<T> values)
        : this(values.ToArray())
    {

    }
    public BagValue(T[] values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    public static implicit operator BagValue<T>(T[] array) => BagValue.New(array);

    public static implicit operator T[](BagValue<T> array) => array._values;

    public static bool operator ==(BagValue<T> left, BagValue<T> right) => left.Equals(right);

    public static bool operator !=(BagValue<T> left, BagValue<T> right) => !(left == right);

    public int Count => IsEmpty ? 0 : _values.Length;

    public object Clone()
    {
        return IsEmpty
            ? new ArrayValue<T>(Array.Empty<T>())
            : new ArrayValue<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is BagValue<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.IsEqualToSet(other);
    }

    public bool Equals(BagValue<T> other)
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
