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
using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.ComponentModel;


public static class InterceptionExtensions
{
    /// <summary>Creates a builder that enables to change an object an record the changes.
    /// </summary>
    /// <typeparam name="T">Type of the source.</typeparam>
    /// <typeparam name="TProp">Type of the property.</typeparam>
    /// <param name="source">An instance of an object.</param>
    /// <param name="propertySelector">The selector of the property.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>Returns the same changed object.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static IInterceptionBuilder<T, TProp> ChangeWith<T, TProp>(
        this T source,
        Expression<Func<T, TProp>> propertySelector,
        TProp newValue)
    {
        source.ThrowIfNull();
        propertySelector.ThrowIfNull();

        return new InterceptionBuilder<T, TProp>(
                        InterceptionBuilder.BuildMode.ChangeWith,
                        source,
                        propertySelector, newValue);
    }

    /// <summary>
    /// Creates a builder that enables the creation of a new object by copiing <paramref name="source"/>
    /// and replacing the new values. The changes are recorded.
    /// </summary>
    /// <typeparam name="T">Type of the source.</typeparam>
    /// <typeparam name="TProp">Type of the property.</typeparam>
    /// <param name="source">An instance of an object.</param>
    /// <param name="propertySelector">The selector of the property.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>An IInterceptionBuilder.</returns>    
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static IInterceptionBuilder<T, TProp> NewWith<T, TProp>(
        this T source,
        Expression<Func<T, TProp>> propertySelector,
        TProp newValue)
    {
        source.ThrowIfNull();
        propertySelector.ThrowIfNull();

        return new InterceptionBuilder<T, TProp>(
                        InterceptionBuilder.BuildMode.NewWith,
                        source,
                        propertySelector, newValue);
    }

    public static ICollection<T> NewWith<T>(this ICollection<T> source,
        Func<T> valueSelector,
        T newValue,
        Action<KeyValuePair<object, object>> onChange)
    {
        source.ThrowIfNull();
        valueSelector.ThrowIfNull();
        newValue.ThrowIfNull();
        onChange.ThrowIfNull();

        if (!source.FirstAsOption().TryGet(out var value)) return source;

        if (value.EqualsNullable(newValue)) return source;

        var type = source.GetType();
        var ctor = type.GetConstructor(Type.EmptyTypes);
        if (ctor is null) return source;

        var newInstance = (ICollection<T>)ctor.Invoke([]);

        foreach (var elem in source)
        {
            if (elem is null) continue;

            if (elem.EqualsNullable(newValue))
            {
                newInstance.Add(elem);
                continue;
            }

            newInstance.Add(newValue);
            onChange(new KeyValuePair<object, object>(elem, newValue!));
        }

        return newInstance;
    }

    //public static IDictionary<TKey, TValue> ReplaceWith<TKey, TValue>(this IDictionary<TKey, TValue> source,
    //    Func<TKey> keySelector,
    //    TValue newValue,
    //    Action<KeyValuePair<string, object?>> onChange)
    //{
    //    source.ThrowIfNull();
    //    keySelector.ThrowIfNull();
    //    onChange.ThrowIfNull();

    //    var key = keySelector();
    //    if (!source.TryGetValue(key, out var value)) return source;

    //    if (value.EqualsNullable(newValue)) return source;

    //    source[key] = newValue;

    //    onChange(new KeyValuePair<string, object?>(key, newValue));

    //    return source;
    //}
}
