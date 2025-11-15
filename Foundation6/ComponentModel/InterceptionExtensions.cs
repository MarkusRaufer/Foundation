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
using System.Linq.Expressions;

namespace Foundation.ComponentModel;

public static class InterceptionExtensions
{
    /// <summary>Creates a builder that enables to change an object an record the changes.
    /// </summary>
    /// <typeparam name="T">Type of the source.</typeparam>
    /// <param name="source">An instance of an object.</param>
    /// <param name="propertySelector">The selector of the property.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>Returns the same changed object.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static InterceptionBuilder<T> ChangeWith<T>(
        this T source,
        Expression<Func<T, object>> propertySelector,
        object? newValue)
    {
        source.ThrowIfNull();
        propertySelector.ThrowIfNull();

        return new InterceptionBuilder<T>(
                        InterceptionBuilder.BuildMode.ChangeWith,
                        source,
                        propertySelector, newValue);
    }

    public static InterceptionBuilder<T> ChangeWith<T>(
        this T source,
        IEnumerable<KeyValuePair<string, object?>> properties)
    {
        source.ThrowIfNull();
        properties.ThrowIfNull();

        return new InterceptionBuilder<T>(
                        InterceptionBuilder.BuildMode.ChangeWith,
                        source,
                        properties);
    }

    /// <summary>
    /// Creates a builder that enables the creation of a new object by copiing <paramref name="source"/>
    /// and replacing the new values. The changes are recorded.
    /// </summary>
    /// <typeparam name="T">Type of the source.</typeparam>
    /// <param name="source">An instance of an object.</param>
    /// <param name="propertySelector">The selector of the property.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>An IInterceptionBuilder.</returns>    
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static InterceptionBuilder<T> NewWith<T>(
        this T source,
        Expression<Func<T, object>> propertySelector,
        object? newValue)
    {
        source.ThrowIfNull();
        propertySelector.ThrowIfNull();

        return new InterceptionBuilder<T>(
                        InterceptionBuilder.BuildMode.NewWith,
                        source,
                        propertySelector, newValue);
    }

    public static InterceptionBuilder<T> NewWith<T>(
        this T source,
        IEnumerable<KeyValuePair<string, object?>> properties)
    {
        source.ThrowIfNull();
        properties.ThrowIfNull();

        return new InterceptionBuilder<T>(
                        InterceptionBuilder.BuildMode.NewWith,
                        source,
                        properties);
    }
}
