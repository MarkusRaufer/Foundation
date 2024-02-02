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
﻿using Foundation.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;


/// <summary>
/// A map with properties (string, object) that can handle hierarchical properties.
/// </summary>
public class Properties : Properties<PropertyChanged>
{
    public Properties(bool mutableKeys = true) : base(mutableKeys)
    {
    }

    public Properties(IEnumerable<KeyValuePair<string, object>> keyValues, bool mutableKeys = true) : base(keyValues, mutableKeys)
    {
            
    }

    public Properties(EquatableSortedDictionary<string, object> dictionary, bool mutableKeys = true) : base(dictionary, mutableKeys)
    {
    }

    public override void HandleEvent(PropertyChanged propertyChanged)
    {
        if (null == propertyChanged) return;

        if(propertyChanged is PropertyValueChanged valueChanged)
        {
            switch (valueChanged.Action)
            {
                case DictionaryAction.Add: Add(valueChanged.PropertyName, valueChanged.Value); break;
                case DictionaryAction.Replace: this[valueChanged.PropertyName] = valueChanged.Value!; break;
            };

            return;
        }
        switch (propertyChanged.Action)
        {
            case DictionaryAction.Remove: Remove(propertyChanged.PropertyName); break;
        };
    }

    protected override PropertyChanged CreateChangedEvent(string propertyName, object? value, DictionaryAction action)
    {
        return action switch
        {
            DictionaryAction.Add => new PropertyValueChanged(propertyName, action, value),
            DictionaryAction.Remove => new PropertyChanged(propertyName, action),
            DictionaryAction.Replace => new PropertyValueChanged(propertyName, action, value),
            _ => throw new NotImplementedException($"{action}")

        };
    }
}

/// <summary>
/// A map with properties (string, object).
/// </summary>
public abstract class Properties<TEvent> 
    : IProperties<TEvent>
    , IEquatable<Properties<TEvent>>
{
    private readonly bool _mutableKeys;
    private readonly EquatableSortedDictionary<string, object> _properties;
    private readonly IList<TEvent> _events;

    protected Properties(bool mutableKeys = true) : this(new EquatableSortedDictionary<string, object>(), mutableKeys)
    {
    }

    public Properties(IEnumerable<KeyValuePair<string, object>> keyValues, bool mutableKeys = true)
        : this(new EquatableSortedDictionary<string, object>(keyValues), mutableKeys)
    {

    }

    protected Properties(EquatableSortedDictionary<string, object> properties, bool mutableKeys = true)
    {
        _properties = properties.ThrowIfNull();
        _mutableKeys = mutableKeys;
        _events = new List<TEvent>();
    }

    public virtual object this[string key]
    {
        get => _properties[key];
        set
        {
            var exists = TryGetValue(key, out object? val);
            if (!_mutableKeys && !exists) throw new InvalidOperationException("keys are immutable");

            if (exists && val.EqualsNullable(value)) return;

            _properties[key] = value;

            var state = exists ? DictionaryAction.Replace : DictionaryAction.Add;
            AddEvent(CreateChangedEvent(key, value, state));
        }
    }

    public void Add(string key, object? value)
    {
        this[key] = value!;
    }

    public void Add(KeyValuePair<string, object> item)
    {
        Add(item.Key, item.Value);
    }

    protected void AddEvent(TEvent @event)
    {
        _events.Add(@event);
    }

    public void Clear()
    {
        if (!_mutableKeys) throw new InvalidOperationException("keys are immutable");

        _properties.Clear();
        ClearEvents();
    }

    public void ClearEvents() => _events.Clear();

    public bool Contains(KeyValuePair<string, object> item) => _properties.Contains(item);

    public bool ContainsKey(string key) => _properties.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        _properties.CopyTo(array, arrayIndex);
    }

    protected abstract TEvent CreateChangedEvent(string propertyName, object? value, DictionaryAction action);

    public int Count => _properties.Count;

    public override bool Equals(object? obj) => Equals(obj as Properties<TEvent>);
    public bool Equals(Properties<TEvent>? other)
    {
        return null != other && _properties.Equals(other._properties);
    }

    public IEnumerable<TEvent> Events => _events;

    IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _properties.GetEnumerator();

    public override int GetHashCode() => _properties.GetHashCode();

    public IEnumerable<KeyValuePair<string, Type?>> PropertyTypes
        => this.Select(kvp => new KeyValuePair<string, Type?>(kvp.Key, kvp.Value?.GetType()));

    public abstract void HandleEvent(TEvent @event);

    public bool HasEvents => 0 < _events.Count;

    public bool IsReadOnly => _properties.IsReadOnly;

    public ICollection<string> Keys => _properties.Keys;

    public bool Remove(string key)
    {
        var exists = _properties.TryGetValue(key, out var value);
        if (!_mutableKeys && exists) throw new InvalidOperationException("keys are immutable");

        var removed = _properties.Remove(key);

        if (exists && removed) _events.Add(CreateChangedEvent(key, value, DictionaryAction.Remove));

        return removed;
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        if (!_mutableKeys) throw new InvalidOperationException("keys are immutable");

        var removed = _properties.Remove(item);

        if(removed) _events.Add(CreateChangedEvent(item.Key, item.Value, DictionaryAction.Remove));

        return removed;
    }

    public override string ToString() => $"{string.Join(',', _properties)}";

    public virtual bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _properties.TryGetValue(key, out value);

    public ICollection<object> Values => _properties.Values;
}
