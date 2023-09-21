using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class ArrayValue
{
    public static ArrayValue<T> From<T>(IEnumerable<T> values) =>
       new (values);

    public static ArrayValue<T> New<T>(params T[] values) =>
        new (values);

    public static ArrayValue<T> New<T>(string separator, params T[] values)
        => new (values, separator);

    public static ArrayValue<T> NewWith<T>(
        this ArrayValue<T> arrayValue,
        IEnumerable<T> replacements)
    {
        return arrayValue.Replace(replacements).ToArrayValue();
    }
}

/// <summary>
/// This is an immutable array that acts like a value. ArrayValue compares each element and its position on <see cref="Equals(ArrayValue{T})"/>.
/// <see cref="GetHashCode"/> considers all elements and their position.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ArrayValue<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<ArrayValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly string _separator;
    private readonly T[] _values;

    public ArrayValue(T[] values, string separator = ", ")
    {
        _values = values.ThrowIfNull();
        _separator = separator ?? throw new ArgumentNullException(nameof(separator)); ;
        _hashCode = HashCode.FromObjects(_values);
    }

    public ArrayValue(IEnumerable<T> values, string separator = ", ")
    {
        _values = values.ThrowIfEnumerableIsNull().ToArray();
        _separator = separator ?? throw new ArgumentNullException(nameof(separator)); ;
        _hashCode = HashCode.FromObjects(_values);
    }

    public static implicit operator ArrayValue<T>(T[] array) => ArrayValue.New(array);

    public static implicit operator T[](ArrayValue<T> array) => array._values;

    public static bool operator ==(ArrayValue<T> left, ArrayValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ArrayValue<T> left, ArrayValue<T> right)
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

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ArrayValue<T> other && Equals(other);

    /// <summary>
    /// All elements are compared with the elements of <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.SequenceEqual(other);
    }

    /// <summary>
    /// All elements are compared with the elements of <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ArrayValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty 
            && _hashCode == other._hashCode
            && _values.SequenceEqual(other._values);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator<T>();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    /// <summary>
    /// Hash code considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <summary>
    /// True if not initialized.
    /// </summary>
    public bool IsEmpty => _values is null;

    /// <summary>
    /// Number of elements.
    /// </summary>
    public int Length => IsEmpty ? 0 : _values.Length;

    /// <inheritdoc/>
    public override string? ToString()
    {
        return IsEmpty ? "" : string.Join(_separator, _values);
    }
}
