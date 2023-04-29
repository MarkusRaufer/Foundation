using Foundation.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Foundation.Collections.Generic
{

    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    public class PropertyMap : PropertyMap<PropertyChanged>
    {
        public PropertyMap() : base()
        {
        }

        public PropertyMap(EquatableSortedDictionary<string, object> dictionary) : base(dictionary)
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
    public abstract class PropertyMap<TEvent> 
        : IPropertyMap<TEvent>
        , IEquatable<PropertyMap<TEvent>>
    {
        private readonly EquatableSortedDictionary<string, object> _properties;
        private readonly IList<TEvent> _events;

        protected PropertyMap() : this(new EquatableSortedDictionary<string, object>())
        {
        }

        protected PropertyMap(EquatableSortedDictionary<string, object> properties)
        {
            _properties = properties.ThrowIfNull();
            _events = new List<TEvent>();
        }

        public virtual object this[string key]
        {
            get => _properties[key];
            set
            {
                var exists = ContainsKey(key);
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

        public override bool Equals(object? obj) => Equals(obj as PropertyMap<TEvent>);
        public bool Equals(PropertyMap<TEvent>? other)
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

            var removed = _properties.Remove(key);

            if (exists && removed) _events.Add(CreateChangedEvent(key, value, CollectionActionState.Removed));

            return removed;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var removed = _properties.Remove(item);

            if(removed) _events.Add(CreateChangedEvent(item.Key, item.Value, CollectionActionState.Removed));

            return removed;
        }

        public override string ToString() => $"{string.Join(',', _properties)}";

        public virtual bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _properties.TryGetValue(key, out value);

        public ICollection<object> Values => _properties.Values;
    }
}
