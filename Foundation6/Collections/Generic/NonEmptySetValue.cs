using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
/// This immutable collection of values includes only unique values and considers the equality and number of all elements <see cref="Equals"/>. 
/// The position of the elements are ignored. The set must not be empty.
/// </summary>
public static class NonEmptySetValue
{
    /// <summary>
    /// Creates a new <see cref="=NonEmptyUniqueValues<typeparamref name="T"/>"/> from values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if values is empty.</exception>
    public static NonEmptySetValue<T> New<T>(params T[] values)
        => new(values);
}

/// <summary>
/// Immutable set that considers the equality and number of all elements <see cref="Equals"/>. The set must not be empty.
/// By definition a set only includes unique values. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T">The type of the values</typeparam>
[Serializable]
public readonly struct NonEmptySetValue<T>
    : IReadOnlyCollection<T>
    , IEquatable<NonEmptySetValue<T>>
{
    private readonly int _hashCode = 0;
    private readonly HashSet<T> _values;

    /// <inheritdoc/>
    public NonEmptySetValue(IEnumerable<T> values) : this(new HashSet<T>(values))
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if values is empty.</exception>
    public NonEmptySetValue(HashSet<T> values)
    {
        _values = values.ThrowIfNullOrEmpty();
        if(_values.Count == 0) throw new ArgumentOutOfRangeException(nameof(values), $"{nameof(values)} must have at least one element");

        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    /// <param name="comparer">Comparer to change the default comparison of the values.</param>
    public NonEmptySetValue(IEnumerable<T> values, IEqualityComparer<T>? comparer)
        : this(new HashSet<T>(values, comparer))
    {
    }

    public static bool operator ==(NonEmptySetValue<T> left, NonEmptySetValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptySetValue<T> left, NonEmptySetValue<T> right)
    {
        return !(left == right);
    }

    public static implicit operator NonEmptySetValue<T>(T[] values) => new(values);

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
        => obj is NonEmptySetValue<T> other && Equals(other);

    /// <summary>
    /// considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NonEmptySetValue<T> other)
    {
        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.SetEquals(other._values);
    }

    public override int GetHashCode() => _hashCode;

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override string ToString() => string.Join(", ", _values);
}