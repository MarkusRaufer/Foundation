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

public static class ComparableExtensions
{
    /// <summary>
    /// Compare of possible null arguments.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="valueOnNull">In a sorted list it defines if the null values appear at the beginning -1 (Microsoft standard) or at the end 1.</param>
    /// <returns></returns>
    public static int CompareToNullable(this IComparable? lhs, object? rhs, int valueOnNull = -1)
    {
        if (null == lhs) return null == rhs ? 0 : valueOnNull;
        if (null == rhs) return valueOnNull * -1;

        return lhs.CompareTo(rhs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="valueOnNull">In a sorted list it defines if the null values appear at the beginning -1 (Microsoft standard) or at the end 1.</param>
    /// <returns></returns>
    public static int CompareNullableTo<T>(this IComparable<T>? lhs, T? rhs, int valueOnNull = -1)
    {
        if (null == lhs) return null == rhs ? 0 : valueOnNull;
        if (null == rhs) return valueOnNull * -1;

        return lhs.CompareTo(rhs);
    }
}

