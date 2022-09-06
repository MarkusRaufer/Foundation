namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class UniqueBagValue
{
    public static UniqueBagValue<T> New<T>(params T[] values)
    {
        return new UniqueBagValue<T>(values);
    }
}

/// <summary>
/// This is an immutable bag that enshures the uniqueness of all elements.
/// All elements are compared by using Equals.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct UniqueBagValue<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<UniqueBagValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private string? _valuesAsString;
    private readonly HashSet<T>? _values;

    public UniqueBagValue(T[] values)
    {
        values.ThrowIfNull();

        var enumerable = new EnumerableCounter<T>(values);
        _values = new HashSet<T>(enumerable);

        if(enumerable.Count != _values.Count) throw new ArgumentException($"duplicate values", nameof(values));

        _hashCode = HashCode.FromObjects(_values);
        _valuesAsString = "";
    }

    public static implicit operator UniqueBagValue<T>(T[] array) => UniqueBagValue.New(array);

    public static implicit operator T[](UniqueBagValue<T> array) 
        => null == array._values ? Array.Empty<T>() : array._values.ToArray();

    public static bool operator ==(UniqueBagValue<T> left, UniqueBagValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UniqueBagValue<T> left, UniqueBagValue<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => this.ElementAt(index);

    public object Clone()
    {
        return IsEmpty
            ? new UniqueBagValue<T>(Array.Empty<T>())
            : new UniqueBagValue<T>(this);
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is UniqueBagValue<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values!.SetEquals(other);
    }

    public bool Equals(UniqueBagValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        
        return !other.IsEmpty && _values!.SetEquals(other._values!);
    }

    public IEnumerator<T> GetEnumerator() => IsEmpty 
                                            ? Enumerable.Empty<T>().GetEnumerator()
                                            : _values!.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => IsEmpty
                                            ? Enumerable.Empty<T>().GetEnumerator()
                                            : _values!.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => null == _values || 0 == _values.Count;

    public int Length => IsEmpty ? 0 : _values!.Count;

    public override string? ToString()
    {
        if (IsEmpty || 0 == _values!.Count) return "";
        
        if(null == _valuesAsString)
            _valuesAsString = string.Join(", ", _values);

        return _valuesAsString;
    }
}
