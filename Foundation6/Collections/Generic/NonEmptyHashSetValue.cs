namespace Foundation.Collections.Generic;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// This immutable hashset considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
public static class NonEmptyHashSetValue
{
    public static NonEmptyHashSetValue<T> New<T>(params T[] values)
        => new(values);
}

/// <summary>
/// This immutable hashset considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public readonly struct NonEmptyHashSetValue<T>
    : IReadOnlyCollection<T>
    , IEquatable<NonEmptyHashSetValue<T>>
{
    private readonly int _hashCode = 0;
    private readonly HashSet<T> _values;

    public NonEmptyHashSetValue(IEnumerable<T> values)
        : this(new HashSet<T>(values))
    {
    }

    public NonEmptyHashSetValue(HashSet<T> values)
    {
        _values = values.ThrowIfNullOrEmpty();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    public NonEmptyHashSetValue(IEnumerable<T> values, IEqualityComparer<T>? comparer)
        : this(new HashSet<T>(values, comparer))
    {
    }

    public static bool operator ==(NonEmptyHashSetValue<T> left, NonEmptyHashSetValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptyHashSetValue<T> left, NonEmptyHashSetValue<T> right)
    {
        return !(left == right);
    }

    public static implicit operator NonEmptyHashSetValue<T>(T[] values)
        => NonEmptyHashSetValue.New(values);

    /// <summary>
    /// Returns true if NonEmptyHashSetValue contains the item. 
    /// </summary>
    /// <param name="item">searchable item.</param>
    /// <returns>true if found.</returns>
    public bool Contains(T item) => _values.Contains(item);

    /// <summary>
    /// Number of values.
    /// </summary>
    public int Count => _values.Count;

    /// <summary>
    /// considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is NonEmptyHashSetValue<T> other && Equals(other);

    /// <summary>
    /// considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NonEmptyHashSetValue<T> other)
    {
        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.SetEquals(other._values);
    }

    public override int GetHashCode() => _hashCode;

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}
