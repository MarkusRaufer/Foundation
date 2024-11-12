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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
/// This dictionary has unique keys and values. Each key and each value exist only once.
/// </summary>
/// <typeparam name="TKey">The type of the keys. A key can not be nullable.</typeparam>
/// <typeparam name="TValue">The type of the values. A value can not be nullable.</typeparam>
public class HashMap<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    private class ValueTuple(TKey? key, TValue value) : IEquatable<ValueTuple>
    {
        public override bool Equals(object? obj) => Equals(obj as ValueTuple);

        public bool Equals(ValueTuple? other)
        {
            return other is not null && Value.Equals(other.Value);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public TKey? Key { get; } = key;

        public static ValueTuple New(KeyValuePair<TKey, TValue> item) => new(item.Key, item.Value);

        public override string ToString() => $"{nameof(Key)}: {Key}, {nameof(Value)}: {Value}";

        public TValue Value { get; } = value;
    }

    private readonly Dictionary<TKey, TValue> _dictionary = [];
    private readonly HashSet<ValueTuple> _values;

    public HashMap() : this([])
    {
    }

    public HashMap(IEnumerable<KeyValuePair<TKey, TValue>> keyValues) 
        : this(EqualityComparer<TValue>.Default)
    {
        foreach (var kvp in keyValues)
        {
            var valueTuple = ValueTuple.New(kvp);
            if (_values.Add(valueTuple)) _dictionary[kvp.Key] = kvp.Value;
        }
    }

    public HashMap(IEqualityComparer<TValue> valueComparer)
    {
        var comparer = CreateEqualityComparer(valueComparer);
        _values = new HashSet<ValueTuple>(comparer);
    }

    private static IEqualityComparer<ValueTuple> CreateEqualityComparer(IEqualityComparer<TValue> valueComparer)
    {
#if NET7_0_OR_GREATER
    return EqualityComparer<ValueTuple>.Create(equals, x => x.Value.GetHashCode());
#else
        return LambdaEqualityComparer.New<ValueTuple>(equals, x => x.GetNullableHashCode(x => x.Value.GetHashCode()));
#endif
        bool equals(ValueTuple? lhs, ValueTuple? rhs)
        {
            if (lhs is null) return rhs is null;
            if (rhs is null) return false;

            return valueComparer.Equals(lhs.Value, rhs.Value);
        }
    }

    public TValue this[TKey key] 
    {
        get => _dictionary[key];
        set => Add(key, value);
    }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        var valueTuple = new ValueTuple(key, value);

        if (_values.Contains(valueTuple)) return;

        _values.Add(valueTuple);

        _dictionary[key] = value;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _dictionary.Clear();
        _values.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        var enumerator = _dictionary.GetEnumerator();
        for (var i = arrayIndex; i < array.Length; i++)
        {
            if (!enumerator.MoveNext()) break;

            array[i] = enumerator.Current;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public bool Remove(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            _values.Remove(new ValueTuple(key, value));
            return _dictionary.Remove(key);
        }

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_dictionary.TryGetValue(item.Key, out var value) && value is not null && value.Equals(item.Value))
        {            
            _values.Remove(new ValueTuple(item.Key, value));
            return _dictionary.Remove(item.Key);
        }

        return false;
    }

#if NET5_0_OR_GREATER
    public bool TryGetKey(TValue value, [MaybeNullWhen(false)] out TKey? key)
    {
        var valueTuple = new ValueTuple(default, value);

        if (!_values.TryGetValue(valueTuple, out var existingTuple) || existingTuple.Key is null)
        {
            key = default;
            return false;
        }

        key = existingTuple.Key;
        return true;
    }
#endif

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue? value) => _dictionary.TryGetValue(key, out value);
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

}
