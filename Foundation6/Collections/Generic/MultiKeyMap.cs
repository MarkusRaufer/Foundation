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
        private readonly bool _allowDuplicates;
        private readonly IDictionary<TKey, IList<int>> _keyIndices;
        private readonly IList<KeyValuePair<TKey, TValue>> _keyValues;

        public MultiKeyMap(bool allowDuplicates = false)
        {
            _allowDuplicates = allowDuplicates;
            _keyIndices = new Dictionary<TKey, IList<int>>();
            _keyValues = new List<KeyValuePair<TKey, TValue>>();
        }

        public MultiKeyMap(int capacity, bool allowDuplicates = false)
        {
            _allowDuplicates = allowDuplicates;
            _keyIndices = new Dictionary<TKey, IList<int>>(capacity);
            _keyValues = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        public MultiKeyMap(IEqualityComparer<TKey> comparer, bool allowDuplicates = false)
        {
            _allowDuplicates = allowDuplicates;
            _keyIndices = new Dictionary<TKey, IList<int>>(comparer);
            _keyValues = new List<KeyValuePair<TKey, TValue>>();
        }

        public MultiKeyMap(int capacity, IEqualityComparer<TKey> comparer, bool allowDuplicates = false)
        {
            _allowDuplicates = allowDuplicates;
            _keyIndices = new Dictionary<TKey, IList<int>>(capacity, comparer);
            _keyValues = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        public MultiKeyMap(IEnumerable<KeyValuePair<TKey, TValue>> items, bool allowDuplicates = false)
        {
            _allowDuplicates = allowDuplicates;
            _keyIndices = new Dictionary<TKey, IList<int>>();
            _keyValues = new List<KeyValuePair<TKey, TValue>>();

            foreach (var kvp in items)
                Add(kvp);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!_keyIndices.TryGetValue(key, out var indices)) return default;

                return _keyValues[indices.First(x => -1 != x)].Value;
            }
            set
            {
                if (_keyIndices.TryGetValue(key, out var indices))
                {
                    foreach(var index in indices.Where(x => -1 != x))
                    {
                        var kvp = _keyValues[index];
                        if(kvp.Key.Equals(key) && kvp.Value.EqualsNullable(value))
                        {
                            _keyValues[index] = new KeyValuePair<TKey, TValue>(key, value);
                            return;
                        }
                    }

                    var count = _keyValues.Count;
                    _keyValues.Add(new KeyValuePair<TKey, TValue>(key, value));
                    indices.Add(count);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (!_keyIndices.TryGetValue(key, out var indices))
            {
                indices = new List<int> { 0 };
                _keyIndices[key] = indices;

                _keyValues.Add(new KeyValuePair<TKey, TValue>(key, value));
                return;
            }

            if(!_allowDuplicates)
            {
                var kvps = _keyValues.Nths(indices.Where(x => -1 != x));
                if (kvps.Any(x => x.Value.EqualsNullable(value))) return;
            }

            var count = _keyValues.Count;
            _keyValues.Add(new KeyValuePair<TKey, TValue>(key, value));

            indices.Add(count);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _keyIndices.Clear();
            _keyValues.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item.Key, item.Value);
        }

        public bool Contains(TKey key, TValue value)
        {
            if (!_keyIndices.TryGetValue(key, out var indices)) return false;

            var kvps = _keyValues.Nths(indices.Where(x => -1 != x));
            return kvps.Any(x => x.Value.EqualsNullable(value));
        }

        public bool ContainsKey(TKey key) => _keyIndices.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var it = GetEnumerator();
            for (var i = arrayIndex; i < array.Length; i++)
            {
                if (!it.MoveNext()) break;

                array[i] = it.Current;
            }
        }

        public int Count => _keyIndices.Values.SelectMany(x => x).Where(x => -1 != x).Count();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach(var indices in _keyIndices.Values)
            {
                foreach(var index in indices.Where(x => -1 != x))
                {
                    yield return _keyValues[index];
                }
            }
        }
        

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<TKey> GetKeys(IEnumerable<TValue> values)
        {
            var valueArr = values.ToArray();

            foreach(var indices in _keyIndices.Values)
            {
                foreach(var index in indices.Where(x => -1 != x))
                {
                    var kvp = _keyValues[index];
                    if (valueArr.Contains(kvp.Value)) yield return kvp.Key;
                }
            }
        }

        public IEnumerable<TValue> GetValues(IEnumerable<TKey> keys)
        {
            foreach(var key in keys)
            {
                if(!_keyIndices.TryGetValue(key, out var indices)) continue;

                foreach(var index in indices)
                {
                    if (-1 == index) continue;

                    yield return _keyValues[index].Value;
                }
            }
        }

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _keyValues.Select(x => x.Key).ToList();

        public bool Remove(TKey key)
        {
            if (!_keyIndices.TryGetValue(key, out var indices)) return false;

            if(!_keyIndices.Remove(key)) return false;

            foreach(var index in indices)
            {
                _keyValues.RemoveAt(index);
            }

            indices.Clear();
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!_keyIndices.TryGetValue(item.Key, out var indices)) return false;

            foreach (var index in indices.Where(x => -1 != x))
            {
                var value = _keyValues[index];
                if (!value.Value.EqualsNullable(item.Value)) continue;

                indices[index] = -1;
                
                if (!_allowDuplicates) break;
            }

            if (indices.All(x => -1 == x))
            {
                _keyIndices.Remove(item.Key);

                foreach(var kvp in _keyValues.Where(x => x.Key.EqualsNullable(item.Key)).ToArray())
                {
                    _keyValues.Remove(kvp);
                }
            }

            return true;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!_keyIndices.TryGetValue(key, out var indices))
            {
                value = default;
                return false;
            }

            var kvp = _keyValues[indices.First(x => -1 != x)];
            value = kvp.Value;

            return true;
        }

        public ICollection<TValue> Values
        {
            get
            {
                var values = new List<TValue>();
                foreach (var indices in _keyIndices.Values)
                {
                    foreach(var index in indices)
                    {
                        if(-1 == index) continue;

                        var kvp = _keyValues[index];
                        values.Add(kvp.Value);
                    }
                }

                return values;
            }

        }
        
    }
}
