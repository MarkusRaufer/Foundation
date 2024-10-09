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
﻿namespace Foundation;

using Foundation.Collections.Generic;

public static class RandomExtensions
{
    private static readonly char _maxLowerChar = 'z';
    private static readonly char _minUpperChar = 'A';

    private static char[] AlphaChars { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    /// <summary>
    /// Selects an item randomly from an array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="random"></param>
    /// <param name="items">List of items.</param>
    /// <returns></returns>
    public static T GetItem<T>(this Random random, ReadOnlySpan<T> items)
    {
        random.ThrowIfNull();
        if (items.Length == 0) throw new ArgumentOutOfRangeException(nameof(items), $"{nameof(items)} must contain at least one element");

        var min = 0;
        var max = items.Length - 1;
        
        var index = random.Next(min, max);
        return items[index];
    }

    /// <summary>
    /// Selects <paramref name="length"/> items randomly from an array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="random"></param>
    /// <param name="items"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<T> GetItemsLazy<T>(this Random random, T[] items, int length)
    {
        random.ThrowIfNull();
        if (items.Length < 0) throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} must be a positive number");

        var min = 0;
        var max = items.Length - 1;

        for(var i = 0; i < length; i++)
        {
            var index = random.Next(min, max);
            yield return items[index];
        }
    }

    /// <summary>
    /// Selects items from a list between <paramref name="leftIndex"/> and <paramref name="rightIndex"/> and returns them in a random order.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    /// <param name="random"></param>
    /// <param name="items"></param>
    /// <param name="leftIndex">left index where to start the selection.</param>
    /// <param name="rightIndex">right index where to stop the selection.</param>
    /// <returns></returns>
    public static IEnumerable<T> GetItemsLazy<T>(this Random random, IEnumerable<T> items, int leftIndex, int rightIndex)
    {
        random.ThrowIfNull();
        items.ThrowIfNull();
        leftIndex.ThrowIfOutOfRange(() => 0 > leftIndex, $"{nameof(leftIndex)} must be a positive number");
        rightIndex.ThrowIfOutOfRange(() => rightIndex < leftIndex, $"{nameof(rightIndex)} must be equal or greater than {nameof(leftIndex)}");

        var indices = random.IntegersWithoutDuplicates(leftIndex, rightIndex).ToArray();
        var values = new T[rightIndex];
        var foundMax = 0;

        foreach (var (index, item) in items.Enumerate())
        {
            var idx = Array.IndexOf(indices, index);
            if (-1 == idx) continue;

            foundMax = foundMax < idx ? idx : foundMax;
            values[idx] = item;

            if (index >= rightIndex) break;
        }

        return values.Take(foundMax + 1);
    }

    /// <summary>
    /// Returns a list of integers from <paramref name="min"/> to <paramref name="max"/> in a random order.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static IEnumerable<int> IntegersWithoutDuplicates(this Random random, int min, int max)
    {
        random.ThrowIfNull();

        var numbers = Enumerable.Range(min, 1 + max - min).ToArray();
        return numbers.Shuffle(random);
    }

    /// <summary>
    /// Returns a random boolean.
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static bool NextBoolean(this Random random)
    {
        return 0 != random.Next(0, 1);
    }

    /// <summary>
    /// Returns a random alpha char between 'A' and 'z'.
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static char NextAlphaChar(this Random random)
    {
        return random.NextAlphaChar(_minUpperChar, _maxLowerChar);
    }


    /// <summary>
    /// Returns a random alpha char between <paramref name="min"/> and <paramref name="max"/>.
    /// If <paramref name="min"/> is smaller than 'A', 'A' is taken. If <paramref name="max"/> is greater than 'z', 'z' is taken.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static char NextAlphaChar(this Random random, char min, char max)
    {
        if (!AlphaChars.Contains(min)) min = _minUpperChar;
        if (!AlphaChars.Contains(max)) max = _maxLowerChar;

        max++;

        return random.GetItem<char>(AlphaChars);
    }

    /// <summary>
    /// Returns a random DateOnly between leftIndex and rightIndex.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static DateOnly NextDateOnly(this Random random, DateOnly min, DateOnly max)
    {
        var diff = max.Subtract(min);
        var ticks = random.NextInt64(0L, diff.Ticks);
        return min.Add(TimeSpan.FromTicks(ticks));
    }

    /// <summary>
    /// Returns a random DateTime between leftIndex and rightIndex.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static DateTime NextDateTime(this Random random, DateTime min, DateTime max)
    {
        var ticks = random.NextInt64(min.Ticks, max.Ticks);
        return new DateTime(ticks);
    }

    /// <summary>
    /// Returns a random double that is less than rightIndex.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double NextDouble(this Random random, double max)
    {
        return random.NextDouble() * max;
    }

    /// <summary>
    /// Returns a random double that is greater than or equal to leftIndex, and less than rightIndex
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double NextDouble(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Creates deterministic guids often needed for tests. Duplicates are possible.
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static Guid NextGuid(this Random random)
    {
        return NextGuid(random, new byte[16]);
    }

    /// <summary>
    /// Creates deterministic guids often needed for tests. Duplicates are possible.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="guidBuffer">A reusable buffer for creating Guids.</param>
    /// <returns></returns>
    public static Guid NextGuid(this Random random, byte[] guidBuffer)
    {
        random.ThrowIfNull();
        guidBuffer.ThrowIfNull();
        guidBuffer.ThrowIf(() => 16 != guidBuffer.Length, nameof(guidBuffer), "buffer for guid must have the size of 16");

        random.NextBytes(guidBuffer);
        return new Guid(guidBuffer);
    }

    public static long NextInt64(this Random random, long minValue, long maxValue)
    {
        var bytes = new byte[sizeof(long)];
        random.NextBytes(bytes);
        return BitConverter.ToInt64(bytes, 0);
    }

    /// <summary>
    /// Returns a random TimeOnly between leftIndex and rightIndex.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static TimeOnly NextTimeOnly(this Random random, TimeOnly min, TimeOnly max)
    {
        var diff = max.Subtract(min);
        var ticks = random.NextInt64(0L, diff.Ticks);
        return min.Add(TimeSpan.FromTicks(ticks));
    }
}

