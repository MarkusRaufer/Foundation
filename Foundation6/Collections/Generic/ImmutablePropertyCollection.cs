namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class ImmutablePropertyCollection : ImmutablePropertyCollection<ImmutableProperty>
{
    public ImmutablePropertyCollection(IEnumerable<KeyValuePair<string, object?>> keyValues)
        : this(keyValues.Select(x => new ImmutableProperty(x.Key, x.Value)))
    {
    }

    public ImmutablePropertyCollection(IEnumerable<ImmutableProperty> properties) : base(properties)
    { 
    }
}

/// <summary>
/// This immutable collection considers the equality of all properties <see cref="Equals"/>.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ImmutablePropertyCollection<TProperty>
    : IReadOnlyCollection<TProperty>
    , IEquatable<ImmutablePropertyCollection<TProperty>>
    where TProperty : ImmutableProperty
{
    private readonly IDictionary<string, TProperty> _properties;
    private readonly int _hashCode;

    public ImmutablePropertyCollection(IEnumerable<TProperty> properties)
    {
        _properties = new Dictionary<string, TProperty>();
        foreach(var  property in properties)
        {
            _properties[property.Name] = property;
        }

        _hashCode = HashCode.FromObjects(_properties);
    }

    /// <inheritdoc/>
    public TProperty this[string key] => _properties[key];

    /// <inheritdoc/>
    public int Count => _properties.Count;

    /// <inheritdoc/>
    public bool ContainsProperty(string name) => _properties.ContainsKey(name);

    protected static int DefaultHashCode { get; } = typeof(ImmutablePropertyCollection<TProperty>).GetHashCode();

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as ImmutablePropertyCollection<TProperty>);

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ImmutablePropertyCollection<TProperty>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _properties.IsEqualToSet(other._properties);
    }

    /// <inheritdoc/>
    public IEnumerator<TProperty> GetEnumerator() => _properties.Values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();

    /// <summary>
    /// Hash code considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <inheritdoc/>
    public IEnumerable<string> PropertyNames => _properties.Keys;

    /// <inheritdoc/>
    public override string ToString() => string.Join(", ", _properties.Values);

    /// <inheritdoc/>
    public bool TryGetProperty(string key, [MaybeNullWhen(false)] out TProperty property)
    {
        return _properties.TryGetValue(key, out property);
    }
}
