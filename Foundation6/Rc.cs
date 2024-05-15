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

public static class Rc
{
    /// <summary>
    /// Creates a reference counter object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Rc<T> New<T>(T obj) where T : notnull
    {
        return new Rc<T>(obj);
    }
}

/// <summary>
/// This is a reference counter.
/// </summary>
/// <typeparam name="T"></ typeparam >
public sealed class Rc<T>
    : IComparable<Rc<T>>
    , IEquatable<Rc<T>>
    where T : notnull
{
    private readonly T _value;

    public Rc(T value) : this(value, 0)
    {
    }

    public Rc(T value, int counter)
    {
        _value = value.ThrowIfNull();
        Counter = counter;
    }

    public static implicit operator T(Rc<T> rc)
    {
        return rc.Get();
    }

    public static bool operator ==(Rc<T> left, Rc<T> right)
    {
        if(left is null) return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Rc<T> left, Rc<T> right)
    {
        return !(left == right);
    }

    public int CompareTo(Rc<T>? other)
    {
        if (other is null) return 1;

        if (!ValueEquals(other)) return -1;

        return Counter.CompareTo(other.Counter);
    }

    /// <summary>
    /// The number how often Get is called.
    /// </summary>
    public int Counter { get; private set; }

    public override bool Equals(object? obj) => Equals(obj as Rc<T>);

    /// <summary>
    /// Is true if Object and Counter are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Rc<T>? other)
    {
        return other is not null
            && Counter == other.Counter
            && EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    /// <summary>
    /// On every call the counter will be incremented.
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        Counter++;
        return _value;
    }

#if NETSTANDARD2_0
    public override int GetHashCode() => Foundation.HashCode.FromObject(_value, Counter);
#else
    public override int GetHashCode() => System.HashCode.Combine(_value, Counter);
#endif
    public int GetValueHashCode() => _value.GetHashCode();

    /// <summary>
    /// Returns true if the type of the value is assignable to type. />.
    /// </summary>
    /// <param name="type">Type to which the values type can be assigned.</param>
    /// <returns></returns>
    public bool IsAssignableTo(Type type) => type.IsAssignableFrom(_value.GetType());

    public void Reset()
    {
        Counter = 0;
    }

    public override string ToString() => $"Value: {_value}, {nameof(Counter)}: {Counter}";

    /// <summary>
    /// Returns true if <paramref name="other"/> equals the internal _object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ValueEquals(T? other)
    {
        return _value.EqualsNullable(other);
    }

    /// <summary>
    /// Returns true if Object equals the other Object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ValueEquals(Rc<T>? other)
    {
        return other is not null
            && EqualityComparer<T>.Default.Equals(_value, other._value);
    }
}

