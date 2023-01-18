namespace Foundation.Collections.Generic;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// This immutable hashset considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
public static class HashSetValue
{
    public static HashSetValue<T> New<T>(params T[] values)
    {
        return new HashSetValue<T>(values);
    }
}

/// <summary>
/// This immutable hashset considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public readonly struct HashSetValue<T>
    : IReadOnlyCollection<T>
    , IEquatable<HashSetValue<T>>
{
    private readonly int _hashCode = 0;
    private readonly HashSet<T> _values;

    public HashSetValue(IEnumerable<T> values)
        : this(new HashSet<T>(values))
    {
    }

    public HashSetValue(HashSet<T> values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    public HashSetValue(IEnumerable<T> values, IEqualityComparer<T>? comparer)
        : this(new HashSet<T>(values, comparer))
    {
    }

    public static bool operator ==(HashSetValue<T> left, HashSetValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(HashSetValue<T> left, HashSetValue<T> right)
    {
        return !(left == right);
    }

    public static implicit operator HashSetValue<T>(T[] values)
        => new(values);

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
        => obj is HashSetValue<T> other && Equals(other);

    /// <summary>
    /// considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(HashSetValue<T> other)
    {
        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.SetEquals(other._values);
    }

    public override int GetHashCode() => _hashCode;

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}
