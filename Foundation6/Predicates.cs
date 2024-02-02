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
namespace Foundation;

using System.Collections;

public static class Predicates
{
    public static Func<object, bool> AndAlso(Func<object, bool> left, Func<object, bool> right)
    {
        return o => left(o) && right(o);
    }

    public static Func<T, bool> AndAlso<T>(params Func<T, bool>[] predicates)
    {
        return t => predicates.All(predicate => predicate(t));
    }

    public static Func<object, bool> Contains(object value)
    {
        return o => o is IEnumerable enumerable && enumerable.OfType<object>().Contains(value);
    }

    public static Func<object, bool> Contains<T>(T value)
    {
        // TODO: Make code work with enumerables of arbitrary generic type.
        return o => o is IEnumerable<T> enumerable && enumerable.Contains(value);
    }

    public static Func<object, bool> Empty()
    {
        return o => o is IEnumerable enumerable && enumerable.OfType<object>().Any();
    }

    public static Func<object, bool> EndsWith(string suffix, StringComparison comparisonType = StringComparison.InvariantCulture)
    {
        return o => o is string str && str.EndsWith(suffix, comparisonType);
    }

    public static Func<object, bool> Equal(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) == 0;
    }

    public static Func<object, bool> False()
    {
        return o => false;
    }

    public static Func<object, bool> Not(Func<object, bool> predicate)
    {
        return o => !predicate(o);
    }

    public static Func<T, bool> OrElse<T>(params Func<T, bool>[] predicates)
    {
        return t => predicates.Any(predicate => predicate(t));
    }

    public static Func<object, bool> OrElse(Func<object, bool> left, Func<object, bool> right)
    {
        return o => left(o) || right(o);
    }

    public static Func<object, bool> LessThan(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) < 0;
    }

    public static Func<object, bool> LessThanOrEqual(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) <= 0;
    }

    public static Func<object, bool> GreaterThan(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) > 0;
    }

    public static Func<object, bool> GreaterThanOrEqual(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) >= 0;
    }

    public static Func<object, bool> StartsWith(string prefix, StringComparison comparisonType = StringComparison.InvariantCulture)
    {
        return o => o is string str && str.StartsWith(prefix, comparisonType);
    }

    public static Func<object, bool> True() => o => true;
}


