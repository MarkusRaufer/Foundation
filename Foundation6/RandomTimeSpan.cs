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

/// <summary>
/// A random TimeSpan generator.
/// </summary>
public readonly struct RandomTimeSpan
{
    private readonly Random _random;

    public RandomTimeSpan(TimeSpan min, TimeSpan max) : this(new Random(), min, max)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="seed">The seed value for Random.</param>
    public RandomTimeSpan(TimeSpan min, TimeSpan max, int seed) : this(new Random(seed), min, max)
    {
    }

    private RandomTimeSpan(Random random, TimeSpan min, TimeSpan max)
    {
        _random = random.ThrowIfNull();
        if (min > max) throw new ArgumentOutOfRangeException("max must be greater than min");

        Min = min;
        Max = max;
    }

    public bool IsEmpty => null == _random;

    public TimeSpan Max { get; }

    public TimeSpan Min { get; }

    public TimeSpan Next()
    {
        var delayInMilliseconds = _random.NextDouble(Min.TotalMilliseconds, Max.TotalMilliseconds);
        return TimeSpan.FromMilliseconds(delayInMilliseconds);
    }
}

