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

public static class FuncExtensions
{
    public static Func<T, bool> And<T>(this Func<T, bool> first, params Func<T, bool>[] predicates)
    {
        return t => first(t) && predicates.All(predicate => predicate(t));
    }

    public static Func<T, bool> Or<T>(this Func<T, bool> first, params Func<T, bool>[] predicates)
    {
        return t => first(t) || predicates.Any(predicate => predicate(t));
    }

    /// <summary>
    /// Converts a Func{TOk} to Func{object}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Func<object?> ToObjectFunc<T>(this Func<T> func)
    {
        func.ThrowIfNull();

        return () => func();
    }

    /// <summary>
    /// Converts a Func{TOk, bool} to Func{object, bool}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Func<object?, object?> ToObjectFunc<T, TResult>(this Func<T, TResult> func)
    {
        return x => x is T t ? func(t) : null;
    }

    /// <summary>
    /// Converts a Func{TOk, bool} to Func{object, bool}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Func<object, bool> ToObjectPredicate<T>(this Func<T, bool> func)
    {
        return v => func((T)v);
    }
}

