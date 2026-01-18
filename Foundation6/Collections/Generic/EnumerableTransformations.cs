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
using System.Runtime.CompilerServices;
using System.Text;

namespace Foundation.Collections.Generic;

public static class EnumerableTransformations
{
    /// <summary>
    /// Creates a new object from a list of <see cref="KeyValuePair{TKey, TValue}"/> objects.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    /// <typeparam name="TResult">The type of the returned object.</typeparam>
    /// <param name="source">The source key values.</param>
    /// <param name="action">The <see cref="EventAction"/> which can be combined with |.</param>
    /// <param name="keyValues">The key values wich should update the result.</param>
    /// <param name="factory">The factory of the returned object.</param>
    /// <returns></returns>
    public static TResult NewWith<TKey, TValue, TResult>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source,
            EventAction action,
            IEnumerable<KeyValuePair<TKey, TValue>> keyValues,
            Func<IEnumerable<KeyValuePair<TKey, TValue>>, TResult> factory)
        where TKey : notnull
        where TResult : notnull
    {
        source.ThrowIfNull();
        keyValues.ThrowIfNull();
        factory.ThrowIfNull();

        Dictionary<TKey, TValue> newKeyValues = [];
        var sourceDictionary = source.ToDictionary();
        
        foreach (var kv in keyValues)
        {
            if (!sourceDictionary.TryGetValue(kv.Key, out var value))
            {
                if ((action & EventAction.Add) == EventAction.Add)
                {
#pragma warning disable CS8604
                    newKeyValues.Add(kv.Key, kv.Value);
#pragma warning restore
                    continue;
                }
                continue;
            }
            if ((action & EventAction.Update) == EventAction.Update)
            {
                var property = kv.Value.EqualsNullable(value) ? Pair.New(kv.Key, value) : kv;
                newKeyValues.Add(kv.Key, kv.Value);
            }
        }

        var keys = keyValues.Select(x => x.Key).ToArray();
        foreach (var property in sourceDictionary)
        {
            if (keys.Contains(property.Key)) continue;

            if ((action & EventAction.Remove) == EventAction.Remove) continue;

            newKeyValues.Add(property.Key, property.Value);
        }
        return factory(newKeyValues);
    }

    /// <summary>
    /// Splits a stream into multiple new streams. For each predicate an extra stream.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    /// <typeparam name="TResult">Type of the returned elements.</typeparam>
    /// <param name="items">List of elements.</param>
    /// <param name="projections">List of projections.</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<TResult>> SplitInto<T, TResult>(
        this IEnumerable<T> items,
        params Func<T, TResult>[] projections)
    {
        var streams = new List<IList<TResult>>();

        foreach(var _ in projections)
        {
            var stream = new List<TResult>();
            streams.Add(stream);
        }

        foreach (var item in items)
        {
            foreach (var (index, projection) in projections.Enumerate())
            {
                var stream = streams[index];
                stream.Add(projection(item));
            }
        }
        return streams;
    }

    /// <summary>
    /// Splits a stream into multiple new streams. For each predicate an extra stream.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    /// <param name="items">Stream which should be splitted.</param>
    /// <param name="predicates">List of predicates. Each predicate creates an additional stream.</param>
    /// <param name="allowSameElements">True means if multiple predicates return true on an element it will appear in multiple streams. False means if a predicate is true this element is added to this stream and will not appear in other streams. </param>
    /// <param name="removeEmptyStreams">If true empty streams are removed from result otherwise empty streams are included.</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> SplitIntoStreams<T>(
        this IEnumerable<T> items,
        IEnumerable<Func<T, bool>> predicates,
        bool allowSameElements = true,
        bool removeEmptyStreams = false)
    {
        var streams = predicates.Select(x => Enumerable.Empty<T>()).ToList();

        var stop = ObservableValue.New(false);

        foreach (var item in items)
        {
            var i = 0;
            foreach (var predicate in predicates.ToBreakable(ref stop))
            {
                if (predicate(item))
                {
                    streams[i] = streams[i].Append(item);

                    if(!allowSameElements)
                    {
                        i++;
                        stop.Value = true;
                    }
                }
                i++;
            }

            stop.Value = false;
        }

        if(!removeEmptyStreams) return streams;

        foreach (var i in streams.Enumerate()
                                 .Where(tuple => !tuple.item.Any())
                                 .Select(tuple => tuple.counter)
                                 .ToArray())
        {
            streams.RemoveAt(i);
        }
        return streams;
    }

    /// <summary>
    /// Transforms an enumerable of enumerables into an array of arrays.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lists"></param>
    /// <returns></returns>
    public static T[][] ToArrayOfArrays<T>(this IEnumerable<IEnumerable<T>> lists)
    {
        return lists.ToArrays().ToArray();
    }

    /// <summary>
    /// Transforms a list of enumerables into a list of arrays.
    /// </summary>
    /// <typeparam name="T">Type of the enumerable elements.</typeparam>
    /// <param name="lists">List of enumerables</param>
    /// <returns></returns>
    public static IEnumerable<T[]> ToArrays<T>(this IEnumerable<IEnumerable<T>> lists)
    {
        foreach (var list in lists)
            yield return list.ToArray();
    }

    /// <summary>
    /// Creates an ArrayValue from an IEnumerable<typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static ArrayValue<T> ToArrayValue<T>(this IEnumerable<T> items)
    {
        return ArrayValue.New(items.ToArray());
    }

    /// <summary>
    /// Makes the enumerable interruptible. This can be used for nested foreach loops.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="stop"></param>
    /// <returns></returns>
    public static IEnumerable<T> ToBreakable<T>(this IEnumerable<T> items, ref ObservableValue<bool> stop)
    {
        return new BreakableEnumerable<T>(items.ThrowIfNull(), ref stop);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ToByteArray<T>(this IEnumerable<T> items, Encoding? encoding = null) => items.ToBytes(encoding).ToArray();

    public static IEnumerable<byte> ToBytes<T>(this IEnumerable<T> items, Encoding? encoding = null)
    {
        var typeCode = Type.GetTypeCode(typeof(T));

        return typeCode switch
        {
            TypeCode.Boolean => items.ObjectOfType<bool>().SelectMany(BitConverter.GetBytes),
            TypeCode.Byte => items.ObjectOfType<byte>(),
            TypeCode.Char => items.ObjectOfType<char>().SelectMany(BitConverter.GetBytes),
            TypeCode.DateTime => items.ObjectOfType<DateTime>().SelectMany(x => BitConverter.GetBytes(x.Ticks)),
            TypeCode.Decimal => items.ObjectOfType<decimal>().SelectMany(BitConverterExt.GetBytes),
            TypeCode.Double => items.ObjectOfType<double>().SelectMany(BitConverter.GetBytes),
            TypeCode.Int16 => items.ObjectOfType<Int16>().SelectMany(BitConverter.GetBytes),
            TypeCode.Int32 => items.ObjectOfType<Int32>().SelectMany(BitConverter.GetBytes),
            TypeCode.Int64 => items.ObjectOfType<Int64>().SelectMany(BitConverter.GetBytes),
            TypeCode.UInt16 => items.ObjectOfType<UInt16>().SelectMany(BitConverter.GetBytes),
            TypeCode.UInt32 => items.ObjectOfType<UInt32>().SelectMany(BitConverter.GetBytes),
            TypeCode.UInt64 => items.ObjectOfType<UInt64>().SelectMany(BitConverter.GetBytes),
            TypeCode.SByte => items.ObjectOfType<SByte>().Select(x => (byte)x),           
            TypeCode.Single => items.ObjectOfType<float>().SelectMany(BitConverter.GetBytes),
            TypeCode.String => encoding is null ? items.ObjectOfType<string>().SelectMany(Encoding.UTF8.GetBytes)
                                                : items.ObjectOfType<string>().SelectMany(encoding.GetBytes),
            _ => []
        };
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TValue}"/> from a list of <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    /// <param name="items"></param>
    /// <returns>A dictionary containing items.</returns>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items)
        where TKey : notnull
        => items.ToDictionary(x => x.Key, x => x.Value);

    /// <summary>
    /// Creates a new DictionaryValue containing the key-value pairs from the specified enumerable collection.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the collection. Keys must not be null.</typeparam>
    /// <typeparam name="TValue">The type of values in the collection.</typeparam>
    /// <param name="items">An enumerable collection of key-value pairs to include in the DictionaryValue. Cannot be null.</param>
    /// <returns>A DictionaryValue<TKey, TValue> containing the key-value pairs from the specified collection.</returns>
    public static DictionaryValue<TKey, TValue> ToDictionaryValue<TKey, TValue>(
       this IEnumerable<KeyValuePair<TKey, TValue>> items)
        where TKey : notnull
        => DictionaryValue.New(items);

    /// <summary>
    /// Transforms a list of KeyValuePairs into a DictionaryValue.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="items"></param>
    /// <param name="toKey"></param>
    /// <param name="toValue"></param>
    /// <returns></returns>
    public static DictionaryValue<TKey, TValue> ToDictionaryValue<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> items,
        Func<KeyValuePair<TKey, TValue>, TKey> toKey,
        Func<KeyValuePair<TKey, TValue>, TValue> toValue) where TKey : notnull
    {
        var newItems = items.Select(x => new KeyValuePair<TKey, TValue>(toKey(x), toValue(x)));
        return DictionaryValue.New(newItems);
    }

    /// <summary>
    /// Transforms a list of items into a DictionaryValue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="items"></param>
    /// <param name="toKey"></param>
    /// <param name="toValue"></param>
    /// <returns></returns>
    public static DictionaryValue<TKey, TValue> ToDictionaryValue<T, TKey, TValue>(
        this IEnumerable<T> items,
        Func<T, TKey> toKey,
        Func<T, TValue> toValue) where TKey : notnull
    {
        var newItems = items.Select(x => new KeyValuePair<TKey, TValue>(toKey(x), toValue(x)));
        return DictionaryValue.New(newItems);
    }

    /// <summary>
    /// Creates a <see cref="HashSetValue{T}"/> from a list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    /// <param name="items">List of items to be added to the <see cref="HashSetValue{T}"/>.</param>
    /// <returns><see cref="HashSetValue{T}"/> including unique items.</returns>
    public static HashSetValue<T> ToHashSetValue<T>(this IEnumerable<T> items)
    {
        items.ThrowIfNull();
        return new HashSetValue<T>(items);
    }

    /// <summary>
    /// Creates a <see cref="IImmutableKeysDictionary{TKey, TValue}"/> from a list of key values.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    /// <param name="keyValues">List of key values which should be added to the <see cref="IImmutableKeysDictionary{TKey, TValue}"/>-</param>
    /// <returns><see cref="IImmutableKeysDictionary{TKey, TValue}"/> including the <paramref name="keyValues"/>.</returns>
    public static IImmutableKeysDictionary<TKey, TValue> ToImmutableKeysDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
    {
        keyValues.ThrowIfNull();
        return new ImmutableKeysDictionary<TKey, TValue>(keyValues);
    }

    /// <summary>
    /// Transforms key value tuples to a list of <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <param name="keyValues">List of key value tuples.</param>
    /// <returns>List of <see cref="KeyValuePair{TKey, TValue}"/>.</returns>
    public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValues<TKey, TValue>(this IEnumerable<(TKey, TValue)> keyValues)
        => keyValues.Select(x => new KeyValuePair<TKey, TValue>(x.Item1, x.Item2));

    /// <summary>
    /// Creates a <see cref="IMultiMap{TKey, TValue}"/> from an enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="items"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static IMultiMap<TKey, T> ToMultiMap<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        where TKey : notnull
    {
        items.ThrowIfNull();
        keySelector.ThrowIfNull();

        return ToMultiMap(items, keySelector, x => x);
    }

    /// <summary>
    /// Creates a <see cref="IMultiMap{TKey, TValue}"/> from an enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="items"></param>
    /// <param name="keySelector"></param>
    /// <param name="valueSelector"></param>
    /// <returns></returns>
    public static IMultiMap<TKey, TValue> ToMultiMap<T, TKey, TValue>(
        this IEnumerable<T> items,
        Func<T, TKey> keySelector,
        Func<T, TValue> valueSelector)
        where TKey : notnull
    {
        items.ThrowIfNull();
        keySelector.ThrowIfNull();

        var dictionary = new MultiMap<TKey, TValue>();
        foreach (var item in items)
            dictionary.Add(keySelector(item), valueSelector(item));

        return dictionary;
    }

    /// <summary>
    /// Creates a <see cref="NonEmptyArrayValue{T}"/> from a list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static NonEmptyArrayValue<T> ToNonEmptyArrayValue<T>(this IEnumerable<T> items) => new(items.ToArray());

    /// <summary>
    /// Creates a <see cref="NonEmptyHashSetValue{T}"/> from a list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static NonEmptyHashSetValue<T> ToNonEmptyHashSetValue<T>(this IEnumerable<T> items) => new (HashSetValue.New(items));

    /// <summary>
    /// Transforms an <see cref="IEnumerable{T}"/> into <see cref="IEnumerable{object}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<object> ToObjects<T>(this IEnumerable<T> items) => items.Select(x => (object)x.ThrowIfNull());

    /// <summary>
    /// Creates from a list with duplicate keys (n : m => relationship) a list with unique keys and multiple values (1 : n => relationship).
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TLhs"></typeparam>
    /// <typeparam name="TRhs"></typeparam>
    /// <param name="source"></param>
    /// <param name="lhsSelector"></param>
    /// <param name="rhsSelector"></param>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<TLhs, IEnumerable<TRhs>>> ToOneToMany<TSource, TLhs, TRhs>(
        this IEnumerable<TSource> source,
        Func<TSource, TLhs> lhsSelector,
        Func<TSource, TRhs> rhsSelector)
        where TLhs : notnull
    {
        source.ThrowIfNull();
        lhsSelector.ThrowIfNull();
        rhsSelector.ThrowIfNull();

        var one2Many = new MultiMap<TLhs, TRhs>();
        foreach (var sourceElem in source)
        {
            var lhsElem = lhsSelector(sourceElem);
            var rhsElem = rhsSelector(sourceElem);
            if (!one2Many.Contains(lhsElem, rhsElem))
                one2Many.Add(lhsElem, rhsElem);
        }

        return one2Many.GetKeyValues();
    }

    /// <summary>
    /// Normalizes a one to many collection (n : m => relationship) to one to one (1 : n => relationship).
    /// 
    /// A -> [1, 2, 3]
    /// B -> [4, 5]
    /// 
    /// creates tuples:
    /// 
    /// (A,1), (A,2), (A,3), (B,4), (B5)
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TLhs"></typeparam>
    /// <typeparam name="TRhs"></typeparam>
    /// <param name="source"></param>
    /// <param name="lhsSelector"></param>
    /// <param name="rhsSelector"></param>
    /// <returns></returns>
    public static IEnumerable<(TLhs, TRhs)> ToOneToOne<TSource, TLhs, TRhs>(
        this IEnumerable<TSource> source,
        Func<TSource, TLhs> lhsSelector,
        Func<TSource, IEnumerable<TRhs>> rhsSelector)
    {
        source.ThrowIfNull();
        lhsSelector.ThrowIfNull();
        rhsSelector.ThrowIfNull();

        foreach (var sourceElem in source)
        {
            var lhsElem = lhsSelector(sourceElem);
            foreach (var rhsElem in rhsSelector(sourceElem))
            {
                yield return (lhsElem, rhsElem);
            }
        }
    }

    /// <summary>
    /// Transforms items from TOk to Option<TResult>. If TOk could not transformed to TResult a Option.None is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static IEnumerable<Option<TResult>> ToOptions<T, TResult>(
        this IEnumerable<T> items,
        Func<T, Option<TResult>> project)
    {
        items.ThrowIfNull();
        project.ThrowIfNull();

        return items.Select(project);
    }

    public static IEnumerable<Ordinal<T>> ToOrdinals<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        return items.Enumerate()
                    .Where(tuple => predicate(tuple.item))
                    .Select(tuple => new Ordinal<T> { Position = tuple.counter, Value = tuple.item });
    }

    /// <summary>
    /// Creates a <see cref="Queue{T}"/> from an enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Queue<T> ToQueue<T>(this IEnumerable<T> items) => new(items);

    /// <summary>
    /// creates a string from the items separated by separator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToReadableString<T>(this IEnumerable<T> items, string separator = ",")
    {
        items.ThrowIfNull();
        var sb = new StringBuilder();
        foreach (var item in items.AfterEach(() => sb.Append(separator)))
        {
            sb.Append(item);
        }
        return sb.ToString();
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items)
    {
        items.ThrowIfNull();
        return ReadOnlyCollection.New(items);
    }

    /// <summary>
    /// Creates a dictionary from key values.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    /// <param name="keyValues">List of keyValues. Can have duplicates.</param>
    /// <param name="overWriteExisting">If true existing key values will be overwritten otherwise the first occurrence of a key value will remain.</param>
    /// <returns></returns>
    public static Dictionary<TKey, TValue> ToSaveDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> keyValues,
        bool overWriteExisting = true)
        where TKey : notnull
    {
        return keyValues.ToSaveDictionary(x => x.Key, x => x.Value, overWriteExisting);
    }

    public static Dictionary<TKey, TValue> ToSaveDictionary<T, TKey, TValue>(
        this IEnumerable<T> items,
        Func<T, TKey> keySelector,
        Func<T, TValue> valueSelector,
        bool overWriteExisting = true)
        where TKey : notnull
    {
        items.ThrowIfNull();
        keySelector.ThrowIfNull();
        valueSelector.ThrowIfNull();

        Dictionary<TKey, TValue> dictionary = [];
        foreach (var item in items)
        {
            var key = keySelector(item);
            var value = valueSelector(item);

            if (overWriteExisting)
            {
                dictionary[key] = value;
                continue;
            }
            if (dictionary.ContainsKey(key)) continue;
            dictionary.Add(key, value);
        }

        return dictionary;
    }
    /// <summary>
    /// Works like Zip with comparer. Maps only matching items.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="comparer"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> Zip<T1, T2, TResult>(
        this IEnumerable<T1> first,
        IEnumerable<T2> second,
        Func<T1, T2, bool> comparer,
        Func<T1, T2, TResult> resultSelector)
    {
        second.ThrowIfNull();
        comparer.ThrowIfNull();
        resultSelector.ThrowIfNull();

        return from firstItem in first
               from secondItem in second
               where comparer(firstItem, secondItem)
               select resultSelector(firstItem, secondItem);
    }

    /// <summary>
    /// Zips left and right together and returns a list of tuples containing all values from both sides.
    /// </summary>
    /// <typeparam name="T1">Type of the left elements.</typeparam>
    /// <typeparam name="T2">Type of the right elements.</typeparam>
    /// <typeparam name="TSelector">The selector for the comparison value.</typeparam>
    /// <param name="left">The left list.</param>
    /// <param name="right">The right list.</param>
    /// <param name="leftSelector">The selector of the left list.</param>
    /// <param name="rightSelector">The selector of the right list.</param>
    /// <returns>Returns a list of tuples containing all values from both sides.</returns>
    public static IEnumerable<(Option<T1> left, Option<T2> right)> ZipAll<T1, T2, TSelector>(
        this IEnumerable<T1> left,
        IEnumerable<T2> right,
        Func<T1, TSelector> leftSelector,
        Func<T2, TSelector> rightSelector)
        where TSelector : notnull
    {
        left.ThrowIfNull();
        right.ThrowIfNull();
        leftSelector.ThrowIfNull();
        rightSelector.ThrowIfNull();

        var rhs = right.NotNull().ToSaveDictionary(x => rightSelector(x), x => Countable.New(x));
        
        foreach (var lhs in left)
        {
            if (lhs is null) continue;

            if (rhs.TryGetValue(leftSelector(lhs), out var element) && element is not null)
            {
               element.Inc();
               yield return (Option.Some(lhs), Option.Some(element.Value!));
               continue;
            }

            yield return (Option.Some(lhs), Option.None<T2>());
        }

        foreach (var r in rhs.Values)
        {
           if (r.Count == 1) yield return (Option.None<T1>(), Option.Some(r.Value!));
        }
    }
}
