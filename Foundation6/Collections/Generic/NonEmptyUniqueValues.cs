using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
/// This immutable collection of values includes only unique values and considers the equality and number of all elements <see cref="Equals"/>. 
/// The position of the elements are ignored. The set must not be empty.
/// </summary>
public static class NonEmptyUniqueValues
{
    /// <summary>
    /// Creates a new <see cref="=NonEmptyUniqueValues<typeparamref name="T"/>"/> from values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if values is empty.</exception>
    public static NonEmptyUniqueValues<T> New<T>(params T[] values)
        => new(values);
}

/// <summary>
/// This immutable collection of values includes only unique values and considers the equality and number of all elements <see cref="Equals"/>. 
/// The position of the elements are ignored. The set must not be empty.
/// </summary>
/// <typeparam name="T">The type of the values</typeparam>
[Serializable]
public readonly struct NonEmptyUniqueValues<T>
    : IReadOnlyCollection<T>
    , IEquatable<NonEmptyUniqueValues<T>>
{
    private readonly int _hashCode = 0;
    private readonly HashSet<T> _values;

    /// <inheritdoc/>
    public NonEmptyUniqueValues(IEnumerable<T> values) : this(new HashSet<T>(values))
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if values is empty.</exception>
    public NonEmptyUniqueValues(HashSet<T> values)
    {
        _values = values.ThrowIfNullOrEmpty();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    /// <param name="comparer">Comparer to change the default comparison of the values.</param>
    public NonEmptyUniqueValues(IEnumerable<T> values, IEqualityComparer<T>? comparer)
        : this(new HashSet<T>(values, comparer))
    {
    }

    public static bool operator ==(NonEmptyUniqueValues<T> left, NonEmptyUniqueValues<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptyUniqueValues<T> left, NonEmptyUniqueValues<T> right)
    {
        return !(left == right);
    }

    public static implicit operator NonEmptyUniqueValues<T>(T[] values) => new(values);

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
        => obj is NonEmptyUniqueValues<T> other && Equals(other);

    /// <summary>
    /// considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NonEmptyUniqueValues<T> other)
    {
        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.SetEquals(other._values);
    }

    public override int GetHashCode() => _hashCode;

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}