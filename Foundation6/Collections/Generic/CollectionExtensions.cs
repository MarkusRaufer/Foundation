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
﻿using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class CollectionExtensions
{
    public static IEnumerable<T> FindAll<T>(this ICollection<T> collection, LambdaExpression lambda)
    {
        if (!lambda.ThrowIfNull().IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        if (1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"exact one parameter expected");

        if (lambda.Parameters.First().Type != typeof(T))
            throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");

        var predicate = (Func<T, bool>)lambda.Compile();

        return predicate is null ? Enumerable.Empty<T>() : collection.Where(predicate);
    }

    /// <summary>
    /// Throws an exception when collection is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ICollection<T> ThrowIfEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string paramName = "")
        => 0 < collection.Count
        ? collection
        : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    /// <summary>
    /// Throws an exception when collection is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ICollection<T> ThrowIfNull<T>(this ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string paramName = "")
        => collection ?? throw new ArgumentException($"{paramName} must not be empty");

    /// <summary>
    /// Throws an exception when collection is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    public static ICollection<T> ThrowIfNullOrEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression(nameof(collection))] string paramName = "")
        => ThrowIfNull(collection, paramName).ThrowIfEmpty(paramName);
}
