using Foundation.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Foundation.Collections.Generic
{

    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    public class PropertyMap : PropertyMap<PropertyValueChangedEvent<Guid, object, PropertyValueChanged<object>>>
    {
        public PropertyMap() : base()
        {
        }

        public PropertyMap(EquatableSortedDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public override void HandleEvent(PropertyValueChangedEvent<Guid, object, PropertyValueChanged<object>> propertyChanged)
        {
            if (null == propertyChanged) return;

            switch (propertyChanged.PropertyChanged.ChangedState)
            {
                case PropertyChangedState.Added: Add(propertyChanged.PropertyChanged.PropertyName, propertyChanged.PropertyChanged.Value); break;
                case PropertyChangedState.Removed: Remove(propertyChanged.PropertyChanged.PropertyName); break;
                case PropertyChangedState.Replaced: this[propertyChanged.PropertyChanged.PropertyName] = propertyChanged.PropertyChanged.Value!; break;
            };
        }

        protected override PropertyValueChangedEvent<Guid, object, PropertyValueChanged<object>> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
        {
            return new PropertyValueChangedEvent<Guid, object, PropertyValueChanged<object>>(Guid.NewGuid(), new PropertyValueChanged<object>(propertyName, state, value));
        }
    }


    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    /// <typeparam name="TObjectType">With this information you can distinguish maps.</typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public abstract class PropertyMap<TObjectType, TEvent>
        : PropertyMap<TEvent>
        , IPropertyMap<TObjectType, TEvent>
        , IEquatable<PropertyMap<TObjectType, TEvent>>
        where TEvent : ITypedObject<TObjectType>
        where TObjectType : notnull
    {
        public PropertyMap(TObjectType objectType)
            : this(objectType, new EquatableSortedDictionary<string, object>())
        {
        }

        public PropertyMap(
            TObjectType objectType,
            EquatableSortedDictionary<string, object> properties)
            : base(properties)
        {
            ObjectType = objectType.ThrowIfNull();
        }

        public override bool Equals(object? obj) => Equals(obj as PropertyMap<TObjectType, TEvent>);
        
        public bool Equals(PropertyMap<TObjectType, TEvent>? other)
        {
            return null != other && ObjectType.Equals(other.ObjectType) && base.Equals(other);
        }

        public override int GetHashCode() => HashCode.CreateBuilder()
                                                     .AddObject(ObjectType)
                                                     .AddHashCode(base.GetHashCode())
                                                     .GetHashCode();

        [NotNull]
        public TObjectType ObjectType { get; }

        public override string ToString() => $"{ObjectType}";
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

                var state = exists ? PropertyChangedState.Replaced : PropertyChangedState.Added;
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

        protected abstract TEvent CreateChangedEvent(string propertyName, object? value, PropertyChangedState state);

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

            if (exists && removed) _events.Add(CreateChangedEvent(key, value, PropertyChangedState.Removed));

            return removed;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var removed = _properties.Remove(item);

            if(removed) _events.Add(CreateChangedEvent(item.Key, item.Value, PropertyChangedState.Removed));

            return removed;
        }

        public virtual bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _properties.TryGetValue(key, out value);

        public ICollection<object> Values => _properties.Values;
    }
}
