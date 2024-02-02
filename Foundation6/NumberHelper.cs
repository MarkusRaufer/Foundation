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

public static class NumberHelper
{
    public static IEnumerable<int> GetDigits(int value)
    {
        return GetDigits(value, n => n);
    }

    public static IEnumerable<TResult> GetDigits<TResult>(int value, Func<int, TResult> projection)
    {
        projection.ThrowIfNull();

        var digits = new Stack<TResult>();

        int number = value;
        while (number > 0)
        {
            var digit = number % 10;
            digits.Push(projection(digit));

            number /= 10;
        }

        foreach (var d in digits)
            yield return d;
    }

    public static IEnumerable<TResult> GetDigits<TResult>(long value, Func<long, TResult> projection)
    {
        projection.ThrowIfNull();

        var digits = new Stack<TResult>();

        long number = value;
        while (number > 0)
        {
            var digit = number % 10;
            digits.Push(projection(digit));

            number /= 10;
        }

        foreach (var d in digits)
            yield return d;
    }
}
