using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
/// Immutable set that considers the equality and number of all elements <see cref="Equals"/>. 
/// By definition a set only includes unique values. The position of the elements are ignored.
/// </summary>
public static class SetValue
{
    /// <summary>
    /// Creates a new <see cref="=UniqueValues<typeparamref name="T"/>"/> from values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values">Must have at least one value.</param>
    /// <returns>a new <see cref="=UniqueValues<typeparamref name="T"/>"</returns>
    /// <exception cref="ArgumentOutOfRangeException">if values is empty.</exception>
    public static SetValue<T> New<T>(params T[] values) => new(values);
}

/// <summary>
/// This immutable collection of values includes only unique values and considers the equality and number of all elements <see cref="Equals"/>. 
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public readonly struct SetValue<T>
    : IReadOnlyCollection<T>
    , IEquatable<SetValue<T>>
{
    private readonly int _hashCode = 0;
    private readonly HashSet<T> _values;

    /// <inheritdoc/>
    public SetValue(IEnumerable<T> values) : this(new HashSet<T>(values))
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    public SetValue(HashSet<T> values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    /// <param name="comparer">Comparer to change the default comparison of the values.</param>
    public SetValue(IEnumerable<T> values, IEqualityComparer<T>? comparer)
        : this(new HashSet<T>(values, comparer))
    {
    }

    public static bool operator ==(SetValue<T> left, SetValue<T> right) => left.Equals(right);

    public static bool operator !=(SetValue<T> left, SetValue<T> right) => !(left == right);

    public static implicit operator SetValue<T>(T[] values) => new(values);

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
        => obj is SetValue<T> other && Equals(other);

    /// <summary>
    /// considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(SetValue<T> other)
    {
        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.SetEquals(other._values);
    }

    public override int GetHashCode() => _hashCode;

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}
