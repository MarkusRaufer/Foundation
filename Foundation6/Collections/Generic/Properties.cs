using Foundation.ComponentModel;
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
            switch (valueChanged.ActionState)
            {
                case CollectionActionState.Added: Add(valueChanged.PropertyName, valueChanged.Value); break;
                case CollectionActionState.Replaced: this[valueChanged.PropertyName] = valueChanged.Value!; break;
            };

            return;
        }
        switch (propertyChanged.ActionState)
        {
            case CollectionActionState.Removed: Remove(propertyChanged.PropertyName); break;
        };
    }

    protected override PropertyChanged CreateChangedEvent(string propertyName, object? value, CollectionActionState state)
    {
        return state switch
        {
            CollectionActionState.Added => new PropertyValueChanged(propertyName, state, value),
            CollectionActionState.Removed => new PropertyChanged(propertyName, state),
            CollectionActionState.Replaced => new PropertyValueChanged(propertyName, state, value),
            _ => throw new NotImplementedException($"{state}")

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

            var state = exists ? CollectionActionState.Replaced : CollectionActionState.Added;
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

    protected abstract TEvent CreateChangedEvent(string propertyName, object? value, CollectionActionState state);

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

        if (exists && removed) _events.Add(CreateChangedEvent(key, value, CollectionActionState.Removed));

        return removed;
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        if (!_mutableKeys) throw new InvalidOperationException("keys are immutable");

        var removed = _properties.Remove(item);

        if(removed) _events.Add(CreateChangedEvent(item.Key, item.Value, CollectionActionState.Removed));

        return removed;
    }

    public override string ToString() => $"{string.Join(',', _properties)}";

    public virtual bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _properties.TryGetValue(key, out value);

    public ICollection<object> Values => _properties.Values;
}
