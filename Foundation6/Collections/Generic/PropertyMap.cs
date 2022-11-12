using Foundation.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Foundation.Collections.Generic
{

    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    public class PropertyMap : PropertyMap<PropertyChangedEvent>
    {
        public PropertyMap(char pathSeparator = '/') : base(pathSeparator)
        {
        }

        public PropertyMap(SortedDictionary<string, object> dictionary, char pathSeparator = '/') : base(dictionary, pathSeparator)
        {
        }

        public override void HandleEvent(PropertyChangedEvent propertyChanged)
        {
            if (null == propertyChanged) return;

            switch (propertyChanged.ChangedState)
            {
                case PropertyChangedState.Added: Add(propertyChanged.Name, propertyChanged.Value); break;
                case PropertyChangedState.Removed: Remove(propertyChanged.Name); break;
                case PropertyChangedState.Replaced: this[propertyChanged.Name] = propertyChanged.Value!; break;
            };
        }

        protected override PropertyChangedEvent CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
        {
            return new PropertyChangedEvent(propertyName, value, state);
        }
    }


    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    /// <typeparam name="TObjectType">With this information you can distinguish maps.</typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public abstract class PropertyMap<TObjectType, TEvent>
        : PropertyMap<TEvent>
        , IPropertyMap<TObjectType>
        , IEquatable<IPropertyMap<TObjectType>>
        where TEvent : ITypedObject<TObjectType>
    {
        public PropertyMap(TObjectType objectType, char pathSeparator = '/')
            : this(objectType, new SortedDictionary<string, object>(), pathSeparator)
        {
        }

        public PropertyMap(
            TObjectType objectType,
            SortedDictionary<string, object> dictionary, 
            char pathSeparator = '/')
            : base(dictionary, pathSeparator)
        {
            ObjectType = objectType.ThrowIfNull();
        }

        public override bool Equals(object? obj) => base.Equals(obj);
        
        public bool Equals(IPropertyMap<TObjectType>? other)
        {
            return null != other && ObjectType!.Equals(other.ObjectType);
        }

        public override int GetHashCode() => System.HashCode.Combine(ObjectType);

        [NotNull]
        public TObjectType ObjectType { get; }

        public override string ToString() => $"{ObjectType}";
    }

    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    public abstract class PropertyMap<TEvent> 
        : IPropertyMap
        , IEventHandler<TEvent>
        , IEventProvider<TEvent>
    {
        private readonly IDictionary<string, object> _dictionary;
        private readonly IList<TEvent> _events;
        private readonly IMultiValueMap<string, string> _keys;

        protected PropertyMap(char pathSeparator = '/')
            : this(new SortedDictionary<string, object>(), pathSeparator)
        {
        }

        protected PropertyMap(SortedDictionary<string, object> dictionary, char pathSeparator = '/')
        {
            _dictionary = dictionary.ThrowIfNull();
            PathSeparator = pathSeparator;

            _keys = new MultiValueMap<string, string>();
            _events = new List<TEvent>();
        }

        public virtual object this[string key]
        {
            get => _dictionary[key];
            set
            {
                var exists = _dictionary.ContainsKey(key);
                _dictionary[key] = value;

                var state = exists ? PropertyChangedState.Replaced : PropertyChangedState.Added;
                _events.Add(CreateChangedEvent(key, value, state));
            }
        }

        public void Add(string key, object? value)
        {
            _dictionary.Add(key, value!);
            AddToKeys(key);

            AddEvent(CreateChangedEvent(key, value, PropertyChangedState.Added));
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item);
            AddToKeys(item.Key);

            AddEvent(CreateChangedEvent(item.Key, item.Value, PropertyChangedState.Added));
        }

        protected void AddEvent(TEvent @event)
        {
            _events.Add(@event);
        }

        private void AddToKeys(string key)
        {
            var splitted = key.Split(new[] { PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (2 > splitted.Length) return;

            var keyPart = new StringBuilder();
            var last = splitted.Last();

            foreach (var token in splitted.Take(splitted.Length - 1)
                .AfterEach(() => keyPart.Append(PathSeparator)))
            {
                keyPart.Append(token);
            }
            _keys.Add(keyPart.ToString(), last);
        }

        public void Clear() => _dictionary.Clear();
        public void ClearEvents() => _events.Clear();

        public bool Contains(KeyValuePair<string, object> item) => _dictionary.Contains(item);

        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        protected abstract TEvent CreateChangedEvent(string propertyName, object? value, PropertyChangedState state);

        public int Count => _dictionary.Count;

        public IEnumerable<TEvent> Events => _events;

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _dictionary.GetEnumerator();

        public IEnumerable<KeyValuePair<string, Type?>> PropertyTypes
            => this.Select(kvp => new KeyValuePair<string, Type?>(kvp.Key, kvp.Value?.GetType()));

        public virtual IEnumerable<object> GetValues(string key)
        {
            if (_dictionary.TryGetValue(key, out object? value))
            {
                yield return value;
                yield break;
            }

            if (_keys.TryGetValues(key, out IEnumerable<string> subkeys))
            {
                foreach (var subkey in subkeys)
                {
                    var compositeKey = $"{key}{PathSeparator}{subkey}";
                    if (_dictionary.TryGetValue(compositeKey, out value))
                        yield return value;
                }
            }
        }

        public abstract void HandleEvent(TEvent @event);

        public bool HasEvents => 0 < _events.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public ICollection<string> Keys => _dictionary.Keys;

        public char PathSeparator { get; }

        public IEnumerable<KeyValuePair<string, object>> PropertiesStartWith(string name)
        {
            foreach (var key in _dictionary.Keys)
            {
                if (key.StartsWith(name))
                    yield return new KeyValuePair<string, object>(key, _dictionary[key]);
            }
        }

        public bool Remove(string key)
        {
            var removed = _dictionary.Remove(key);
            _events.Add(CreateChangedEvent(key, null, PropertyChangedState.Removed));

            return removed;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var removed = _dictionary.Remove(item);
            _events.Add(CreateChangedEvent(item.Key, item.Value, PropertyChangedState.Removed));

            return removed;
        }

        /// <summary>
        /// Contains all properties which key does not contain a PathSeparator
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> RootProperties
        {
            get
            {
                foreach (var key in _dictionary.Keys)
                {
                    if (!key.Contains(PathSeparator))
                        yield return new KeyValuePair<string, object>(key, _dictionary[key]);
                }
            }
        }

        public virtual bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
        {
            if(_dictionary.TryGetValue(key, out value)) return true;
            
            if(_keys.TryGetValues(key, out IEnumerable<string> subkeys))
            {
                var values = new List<object>();
                foreach(var subkey in subkeys)
                {
                    if (_dictionary.TryGetValue(subkey, out value))
                        values.Add(value);
                }
                if(0 < values.Count)
                {
                    value = values;
                    return true;
                }
            }
            value = default;
            return false;
        }


        public ICollection<object> Values => _dictionary.Values;
    }
}
