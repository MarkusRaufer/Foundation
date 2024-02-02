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
﻿namespace Foundation.Collections.Generic;

using System.Data;
using System.Runtime.CompilerServices;
using Foundation;
using Foundation.Collections.Generic;

public static class ArrayExtensions
{
    /// <summary>
    /// Creates a new array and appends <paramref name="elem"/> to the new array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static T[] Append<T>(this T[] array, T elem)
    {
        array.ThrowIfNull();

        var newValues = new T[array.Length + 1];
        newValues[array.Length] = elem;

        Array.Copy(array, 0, newValues, 0, array.Length);

        return newValues;
    }

    public static IEnumerable<T> AsEnumerable<T>(params T[] items)
    {
        return items;
    }

    public static decimal AverageMedian<T>(this T[] array, Func<T, decimal>? converter = null)
    {
        var (opt1, opt2) = array.AverageMedianPosition();
        if (opt1.IsNone) return 0;

        var value1 = null == converter
            ? Convert.ToDecimal(opt1.OrThrow())
            : converter(opt1.OrThrow());

        if (opt2.IsNone) return value1;

        var value2 = null == converter
            ? Convert.ToDecimal(opt2.OrThrow())
            : converter(opt2.OrThrow());

        return (value1 + value2) / 2M;
    }

    public static (Option<T> pos1, Option<T> pos2) AverageMedianPosition<T>(
        this T[] array,
        IComparer<T>? comparer = null)
    {
        var sorted = new T[array.Length];
        Array.Copy(array, sorted, array.Length);

        if (null == comparer)
            Array.Sort(sorted);
        else
            Array.Sort(sorted, comparer);

        int halfIndex = sorted.Length / 2;

        return sorted.Length % 2 == 0
            ? (Option.Some(sorted[halfIndex - 1]), Option.Some(sorted[halfIndex]))
            : (Option.Some(sorted[halfIndex]), Option.None<T>());
    }

    public static T[] ConcatArray<T>(this T[] lhs, T[] rhs)
    {
        var newArray = new T[lhs.Length + rhs.Length];

        lhs.CopyTo(newArray, 0);
        rhs.CopyTo(newArray, lhs.Length);

        return newArray;
    }

    public static bool EqualsArray<T>(this T[] lhs, T[] rhs)
    {
        if (lhs is null) return rhs is null;

        if (rhs is null || lhs.Length != rhs.Length) return false;
        for (var i = 0; i < lhs.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(lhs[i], rhs[i])) return false;
        }

        return true;
    }

    public static bool EqualsArray(this byte[] lhs, byte[] rhs)
    {
        if (lhs is null) return rhs is null;

        if (rhs is null || lhs.Length != rhs.Length) return false;
        for (var i = 0; i < lhs.Length; i++)
        {
            if (lhs[i] != rhs[i]) return false;
        }

        return true;
    }

    public static IEnumerator<T> GetEnumerator<T>(this T[] array)
    {
        return array.AsEnumerable().GetEnumerator();
    }

    /// <summary>
    /// returns the indexes of the found selectors in array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndexesOf<T>(this T[] array, IEnumerable<T> selectors)
    {
        var selectorArray = selectors.ToArray();

        for (var i = 0; i < array.Length; i++)
        {
            if (selectorArray.Contains(array[i])) yield return i;
        }
    }

    public static IEnumerable<int> IndexesOf<T>(this T[] array, params T[] selectors)
    {
        return array.IndexesOf((IEnumerable<T>)selectors);
    }

    /// <summary>
    /// Creates a new array and copies all values from array and inserts value at a specific index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T[] InsertAt<T>(this T[] array, T value, int index)
    {
        index.ThrowIfOutOfRange(() => index < 0 || index > array.Length);

        var newArray = new T[array.Length + 1];

        if (0 == index)
        {
            Array.Copy(array, 0, newArray, 1, array.Length);
        }
        else if (index == array.Length)
        {
            Array.Copy(array, newArray, array.Length);
        }
        else
        {
            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index, newArray, index + 1, array.Length - index);
        }

        newArray[index] = value;

        return newArray;
    }

    /// <summary>
    /// Inserts a value into an array. Existing values will be moved.
    /// The array must big enough to move the elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public static void InsertAtSameArray<T>(this T[] array, T value, int index)
    {
        index.ThrowIfOutOfRange(() => index < 0 || index > array.Length);
        array.ThrowIfOutOfRange(() => 2 > array.Length);

        if (0 == index)
        {
            Array.Copy(array, 0, array, 1, array.Length - 1);
        }
        else if (index == array.Length - 1)
        {
        }
        else
        {
            Array.Copy(array, index, array, index + 1, array.Length - index - 1);
        }

        array[index] = value;
    }

    /// <summary>
    /// Creates a new array and prepends <paramref name="elem"/> to the new array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static T[] Prepend<T>(this T[] array, T elem)
    {
        array.ThrowIfNull();

        var newValues = new T[array.Length + 1];
        newValues[0] = elem;

        Array.Copy(array, 0, newValues, 1, array.Length);

        return newValues;
    }

    /// <summary>
    /// This method implements the Fisher-Yates Shuffle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public static T[] Shuffle<T>(this T[] array, Random? random = null)
    {
        if (null == random) random = new Random();

        var n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            var r = i + random.Next(n - i);

            (array[i], array[r]) = (array[r], array[i]);
        }

        return array;
    }

    public static void Swap<T>(this T[] items, int lhsIndex, int rhsIndex)
    {
        (items[rhsIndex], items[lhsIndex]) = (items[lhsIndex], items[rhsIndex]);
    }

    public static T[] ThrowIfEmpty<T>(this T[] arr, [CallerArgumentExpression("arr")] string paramName = "")
        => 0 == arr.Length ? throw new ArgumentOutOfRangeException($"{paramName} must not be empty") : arr;


    public static T[] ThrowIfNull<T>(this T[] arr, [CallerArgumentExpression("arr")] string paramName = "")
            => arr ?? throw new ArgumentNullException($"{paramName} must not be empty");

    public static T[] ThrowIfNullOrEmpty<T>(this T[] arr, [CallerArgumentExpression("arr")] string paramName = "")
            => arr.ThrowIfNull(paramName).ThrowIfEmpty(paramName);


    public static NonEmptyArrayValue<T> ToNonEmptyArrayValue<T>(this T[] arr) => new (arr);
    public static NonEmptySetValue<T> ToNonEmptySetValue<T>(this T[] arr) => new(arr);
}

