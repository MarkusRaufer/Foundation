namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class EquatableBag
{
    public static EquatableBag<T> New<T>(params T[] values)
    {
        return new EquatableBag<T>(values);
    }
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(EquatableBag{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct EquatableBag<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<EquatableBag<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private string _valuesAsString;
    private readonly T[] _values;

    public EquatableBag(T[] values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromOrderedObjects(_values);
        _valuesAsString = "";
    }

    public static implicit operator EquatableBag<T>(T[] array) => EquatableBag.New(array);

    public static implicit operator T[](EquatableBag<T> array) => array._values;

    public static bool operator ==(EquatableBag<T> left, EquatableBag<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EquatableBag<T> left, EquatableBag<T> right)
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

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is EquatableBag<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.IsEqualToSet(other);
    }

    public bool Equals(EquatableBag<T> other)
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

    public override string? ToString()
    {
        if (IsEmpty || 0 == _values.Length) return "";
        
        if(0 == _valuesAsString.Length)
            _valuesAsString = string.Join(", ", _values);

        return _valuesAsString;
    }
}
