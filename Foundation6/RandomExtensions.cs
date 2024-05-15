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
    public static T GetItem<T>(this Random random, T[] items)
    {
        random.ThrowIfNull();
        items.ThrowIfNull();
        items.ThrowIfOutOfRange(() => 0 == items.Length, () => $"{nameof(items)} must contain at least one element");

        var min = 0;
        var max = items.Length - 1;
        
        var index = random.Next(min, max);
        return items[index];
    }

    public static IEnumerable<T> GetItems<T>(this Random random, T[] items, int length)
    {
        random.ThrowIfNull();
        items.ThrowIfNull();
        length.ThrowIfOutOfRange(() => 0 > length, $"length must be a positive number");

        var min = 0;
        var max = items.Length - 1;

        for(var i = 0; i < length; i++)
        {
            var index = random.Next(min, max);
            yield return items[index];
        }
    }

    public static IEnumerable<int> IntegersWithoutDuplicates(this Random random, int min, int max)
    {
        if (null == random) throw new ArgumentNullException(nameof(random));
        var numbers = Enumerable.Range(min, 1 + max - min).ToArray();
        return numbers.Shuffle(random);
    }

    public static bool NextBoolean(this Random random)
    {
        return 0 != random.Next(0, 1);
    }

    public static DateTime NextDateTime(this Random random, DateTime min, DateTime max)
    {
        var ticks = random.NextInt64(min.Ticks, max.Ticks);
        return new DateTime(ticks);
    }

    public static double NextDouble(this Random random, double max)
    {
        return random.NextDouble() * max;
    }

    /// <summary>
    /// Returns a random floating-point number that is greate than or equal to min, and less than max
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

#if NETSTANDARD2_0
    public static long NextInt64(this Random random, long minValue, long maxValue)
    {
        var bytes = new byte[sizeof(long)];
        random.NextBytes(bytes);
        return BitConverter.ToInt64(bytes, 0);
    }
#endif
}

