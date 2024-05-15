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
﻿using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class SortedListExtensions
{
    public static SortedList<T> ThrowIfEmpty<T>(this SortedList<T> list, [CallerArgumentExpression(nameof(list))] string paramName = "")
            => 0 < list.Count
               ? list
               : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    public static SortedList<T> ThrowIfNull<T>(this SortedList<T> list, [CallerArgumentExpression(nameof(list))] string paramName = "")
        => list ?? throw new ArgumentException($"{paramName} must not be empty");

    public static SortedList<T> ThrowIfNullOrEmpty<T>(this SortedList<T> list, [CallerArgumentExpression(nameof(list))] string paramName = "")
        => ThrowIfNull(list, paramName).ThrowIfEmpty(paramName);
}
