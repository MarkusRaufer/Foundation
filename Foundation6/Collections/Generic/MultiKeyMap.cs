using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// Dictionary that allows duplicate keys.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MultiKeyMap<TKey, TValue> : IMultiKeyMap<TKey, TValue>
        where TKey : notnull
    {
        private record KeyTuple(TKey Key, int Number);

        private readonly IMultiValueMap<TKey, KeyTuple> _keys;
        private readonly IDictionary<KeyTuple, TValue> _values;

        public MultiKeyMap()
        {
            _keys = new MultiValueMap<TKey, KeyTuple>();
            _values = new Dictionary<KeyTuple, TValue>();
        }

        public MultiKeyMap(int capacity)
        {
            _keys = new MultiValueMap<TKey, KeyTuple>(capacity);
            _values = new Dictionary<KeyTuple, TValue>(capacity);
        }

        public MultiKeyMap(IEqualityComparer<TKey> comparer)
        {
            _keys = new MultiValueMap<TKey, KeyTuple>(comparer);
            _values = new Dictionary<KeyTuple, TValue>();
        }

        public MultiKeyMap(int capacity, IEqualityComparer<TKey> comparer)
        {
            _keys = new MultiValueMap<TKey, KeyTuple>(capacity, comparer, () => new List<KeyTuple>());
            _values = new Dictionary<KeyTuple, TValue>(capacity);
        }

        public MultiKeyMap(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            _keys = new MultiValueMap<TKey, KeyTuple>();
            _values = new Dictionary<KeyTuple, TValue>();

            foreach (var kvp in items)
                Add(kvp);
        }

        public TValue this[TKey key]
        {
            get
            {
                var tuple = _keys[key];
                return _values[tuple];
            }
            set
            {
                if (!((IDictionary<TKey, KeyTuple>)_keys).TryGetValue(key, out var tuple))
                    tuple = new KeyTuple(key, 0);

                _values[tuple] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            var max = 0;

            foreach (var tuple in _keys.GetValues(new[] { key }))
            {
                if (max <= tuple.Number) max = tuple.Number + 1;
            }

            var newTuple = new KeyTuple(key, max);
            _keys.Add(key, newTuple);
            _values.Add(newTuple, value);
        }

        public int Count => _values.Count;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _keys.Keys;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!_keys.TryGetValues(item.Key, out var tuples)) return false;

            var values = tuples.Select(x => _values[x]);
            return values.Any(x => x.EqualsNullable(item.Value));
        }

        public bool ContainsKey(TKey key) => _keys.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var it = GetEnumerator();
            for(var i = arrayIndex; i < array.Length; i++)
            {
                if (!it.MoveNext()) break;

                array[i] = it.Current;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach(var tuple in _keys.Values)
            {
                var value = _values[tuple];
                yield return new KeyValuePair<TKey, TValue>(tuple.Key, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<TKey> GetKeys(IEnumerable<TValue> values)
        {
            var valueArr = values.ToArray();
            var tuples = _values.Where(x => valueArr.Contains(x.Value)).Select(x => x.Key).ToArray();

            foreach (var key in _keys.GetKeys(tuples))
            {
                yield return key;
            }
        }

        public IEnumerable<TValue> GetValues(IEnumerable<TKey> keys)
        {
            foreach(var key in keys)
            {
                if (!((IDictionary<TKey, KeyTuple>)_keys).TryGetValue(key, out var tuple)) continue;

                if(_values.TryGetValue(tuple, out var value))
                    yield return value;
            }
        }

        public bool Remove(TKey key)
        {
            if (!_keys.TryGetValues(key, out var tuples)) return false;

            foreach (var tuple in tuples.ToArray())
            {
                _keys.Remove(tuple.Key, tuple);
                _values.Remove(tuple);
            }

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!_keys.TryGetValues(item.Key, out var tuples)) return false;

            var remove = new List<KeyTuple>();
            var removed = false;
            foreach(var tuple in tuples)
            {
                var value = _values[tuple];
                if(item.Value.EqualsNullable(value))
                {
                    if(_values.Remove(Pair.New(tuple, value)))
                    {
                        remove.Add(tuple);
                        removed = true;
                        break;
                    }
                }
            }

            foreach(var tuple in remove)
            {
                _keys.Remove(item.Key, tuple);
            }

            return removed;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!((IDictionary<TKey, KeyTuple>)_keys).TryGetValue(key, out var tuple))
            {
                value = default;
                return false;
            }

            value = _values[tuple];
            return true;
        }

        public ICollection<TValue> Values => _values.Values;
    }
}
