namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class EquatableArray
{
    public static EquatableArray<T> New<T>(params T[] values)
    {
        return new EquatableArray<T>(values);
    }
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(EquatableArray{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct EquatableArray<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<EquatableArray<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private string _valuesAsString;
    private readonly T[] _values;

    public EquatableArray(T[] values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_values);
        _valuesAsString = "";
    }

    public static implicit operator EquatableArray<T>(T[] array) => EquatableArray.New(array);

    public static implicit operator T[](EquatableArray<T> array) => array._values;

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => _values[index];

    public object Clone()
    {
        return IsEmpty
            ? new EquatableArray<T>(Array.Empty<T>())
            : new EquatableArray<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is EquatableArray<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.SequenceEqual(other);
    }

    public bool Equals(EquatableArray<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        return !other.IsEmpty && _values.SequenceEqual(other._values);
    }

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator<T>();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => null == _values || 0 == _values.Length;

    public int Length => IsEmpty ? 0 : _values.Length;

    public override string? ToString()
    {
        if (IsEmpty || 0 == _values.Length) return "";
        
        if(0 == _valuesAsString.Length)
            _valuesAsString = string.Join(", ", _values);

        return _valuesAsString;
    }
}
