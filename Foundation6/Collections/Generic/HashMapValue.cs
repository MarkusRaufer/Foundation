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
﻿// The MIT License (MIT)
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
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
///  This is an immutable <see cref="HashMap{TKey, TValue}"/>. Each key and each value exist only once.
///  Using <see cref="Equals"/> considers the equality of all keys and values.
///  The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey">Type of the keys.</typeparam>
/// <typeparam name="TValue">Type of the values.</typeparam>
public class HashMapValue<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>
    , IEquatable<HashMapValue<TKey, TValue>>
    where TKey : notnull
    where TValue : notnull
{
    private readonly HashMap<TKey, TValue> _map;
    private readonly int _hashCode;

    public HashMapValue(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        _map = [..keyValues];
        _hashCode = HashCode.FromObjects(_map);
    }

    public HashMapValue(HashMap<TKey, TValue> map)
    {
        _map = map.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_map);
    }

    #region IReadOnlyDictionary<TKey, TValue>
    public TValue this[TKey key] => _map[key];

    public IEnumerable<TKey> Keys => _map.Keys;

    public IEnumerable<TValue> Values => _map.Values;

    public int Count => _map.Count;

    public bool ContainsKey(TKey key) => _map.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _map.GetEnumerator();

#if NET5_0_OR_GREATER
    public bool TryGetKey(TValue value, [MaybeNullWhen(false)] out TKey? key) => _map.TryGetKey(value, out key);
#endif

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _map.TryGetValue(key, out value);
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    IEnumerator IEnumerable.GetEnumerator() => _map.GetEnumerator();
    #endregion IReadOnlyDictionary<TKey, TValue>

    public override int GetHashCode() => _hashCode;

    public override bool Equals(object? obj) => Equals(obj as HashMapValue<TKey, TValue>);

    public bool Equals(HashMapValue<TKey, TValue>? other)
    {
        return other is not null
            && GetHashCode() == other.GetHashCode()
            && _map.EqualsDictionary(other._map);
    }

    public override string ToString() => string.Join(", ", _map);
}
