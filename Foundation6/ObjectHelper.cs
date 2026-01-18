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

public static class ObjectHelper
{
    /// <summary>
    /// Increments the value of <paramref name="obj"/> if <paramref name="obj"/> is a number.
    /// </summary>
    /// <param name="obj">instance of an object.</param>
    /// <returns>An Some with an incremented value of <paramref name="obj"/> if it is a number otherwise None.</returns>
    public static Option<object> Increment(this object obj)
    {
        return obj switch
        {
            byte x => Option.Some(++x),
            char x => Option.Some(++x),
            decimal x => Option.Some(++x),
            double x => Option.Some(++x),
            float x => Option.Some(++x),
            int x => Option.Some(++x),
            long x => Option.Some(++x),
            sbyte x => Option.Some(++x),
            short x => Option.Some(++x),
            uint x => Option.Some(++x),
            ulong x => Option.Some(++x),
            _ => Option.None<object>(),
        };
    }

    public static IEnumerable<object> SortByHashCode(params object[] objects)
    {
        var d = new SortedDictionary<int, object>();
        foreach (var obj in objects)
            d.Add(obj.GetHashCode(), obj);

        return d.Select(pair => pair.Value);
    }
}
