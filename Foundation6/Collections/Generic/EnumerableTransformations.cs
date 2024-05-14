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
        return new BreakableEnumerable<T>(items.ThrowIfEnumerableIsNull(), ref stop);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ToByteArray<T>(this IEnumerable<T> items, Encoding? encoding = null) => items.ToBytes(encoding).ToArray();

    public static IEnumerable<byte> ToBytes<T>(this IEnumerable<T> items, Encoding? encoding = null)
    {
        var typeCode = Type.GetTypeCode(typeof(T));

        return typeCode switch
        {
            TypeCode.Boolean => items.OfType<bool>().SelectMany(BitConverter.GetBytes),
            TypeCode.Byte => items.OfType<byte>(),
            TypeCode.Char => items.OfType<char>().SelectMany(BitConverter.GetBytes),
            TypeCode.DateTime => items.OfType<DateTime>().SelectMany(x => BitConverter.GetBytes(x.Ticks)),
            TypeCode.Decimal => items.OfType<decimal>().SelectMany(BitConverterExt.GetBytes),
            TypeCode.Double => items.OfType<double>().SelectMany(BitConverter.GetBytes),
            TypeCode.Int16 => items.OfType<Int16>().SelectMany(BitConverter.GetBytes),
            TypeCode.Int32 => items.OfType<Int32>().SelectMany(BitConverter.GetBytes),
            TypeCode.Int64 => items.OfType<Int64>().SelectMany(BitConverter.GetBytes),
            TypeCode.UInt16 => items.OfType<UInt16>().SelectMany(BitConverter.GetBytes),
            TypeCode.UInt32 => items.OfType<UInt32>().SelectMany(BitConverter.GetBytes),
            TypeCode.UInt64 => items.OfType<UInt64>().SelectMany(BitConverter.GetBytes),
#if NET6
            TypeCode.SByte => items.OfType<SByte>().SelectMany(x => BitConverter.GetBytes(x)),
#elif NET8

            TypeCode.SByte => items.OfType<SByte>().SelectMany(x => BitConverter.GetBytes((sbyte)x)),
#endif            
            TypeCode.Single => items.OfType<float>().SelectMany(BitConverter.GetBytes),
            TypeCode.String => encoding is null ? items.OfType<string>().SelectMany(Encoding.UTF8.GetBytes)
                                                : items.OfType<string>().SelectMany(encoding.GetBytes),
            _ => []
        };
    }

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
    /// Creates a <see cref="IMultiValueMap{TKey, TValue}"/> from an enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="items"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static IMultiValueMap<TKey, T> ToMultiValueMap<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        where TKey : notnull
    {
        items.ThrowIfEnumerableIsNull();
        keySelector.ThrowIfNull();

        return ToMultiValueMap(items, keySelector, x => x);
    }

    /// <summary>
    /// Creates a <see cref="IMultiValueMap{TKey, TValue}"/> from an enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="items"></param>
    /// <param name="keySelector"></param>
    /// <param name="valueSelector"></param>
    /// <returns></returns>
    public static IMultiValueMap<TKey, TValue> ToMultiValueMap<T, TKey, TValue>(
        this IEnumerable<T> items,
        Func<T, TKey> keySelector,
        Func<T, TValue> valueSelector)
        where TKey : notnull
    {
        items.ThrowIfEnumerableIsNull();
        keySelector.ThrowIfNull();

        var dictionary = new MultiValueMap<TKey, TValue>();
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
    /// Creates a <see cref="NonEmptySetValue{T}"/> from a list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static NonEmptySetValue<T> ToNonEmptySetValue<T>(this IEnumerable<T> items) => new (items);

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
        source.ThrowIfEnumerableIsNull();
        lhsSelector.ThrowIfNull();
        rhsSelector.ThrowIfNull();

        var one2Many = new MultiValueMap<TLhs, TRhs>();
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
        source.ThrowIfEnumerableIsNull();
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
    /// Transforms items from T to Option<TResult>. If T could not transformed to TResult a Option.None is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static IEnumerable<Option<TResult>> ToOptions<T, TResult>(
        this IEnumerable<T> items,
        Func<T, Option<TResult>> project)
    {
        items.ThrowIfEnumerableIsNull();
        project.ThrowIfNull();

        return items.Select(project);
    }

    public static IEnumerable<Ordinal<T>> ToOrdinals<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        items.ThrowIfEnumerableIsNull();
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
        items.ThrowIfEnumerableIsNull();
        var sb = new StringBuilder();
        foreach (var item in items.AfterEach(() => sb.Append(separator)))
        {
            sb.Append(item);
        }
        return sb.ToString();
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items)
    {
        items.ThrowIfEnumerableIsNull();
        return ReadOnlyCollection.New(items);
    }
}
