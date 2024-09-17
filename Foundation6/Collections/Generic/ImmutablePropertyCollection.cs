// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿namespace Foundation.Collections.Generic;

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

        return _properties.EqualsDictionary(other._properties);
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
#if NETSTANDARD2_0
    public bool TryGetProperty(string key, out TProperty property)
#else
    public bool TryGetProperty(string key, [MaybeNullWhen(false)] out TProperty property)
#endif
    {
        return _properties.TryGetValue(key, out property);
    }
}
