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

public static class Countable
{
    /// <summary>
    /// Creates a new Countable object. The counter of the value starts with 1. By default hashCodeIncludesCount is false.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value which should be counted.</param>
    /// <returns></returns>
    public static Countable<T> New<T>(T value) => new(value, 1, false);

    /// <summary>
    /// Creates a new Countable object. By default hashCodeIncludesCount is false.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value which should be counted.</param>
    /// <param name="count">The seed of the counter.</param>
    /// <returns></returns>
    public static Countable<T> New<T>(T value, int count) => new(value, count, false);

    /// <summary>
    /// Creates a new Countable object. By default count is 1.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value which should be counted.</param>
    /// <param name="hashCodeIncludesCount">If true, then the counter is included in the hash value and the comparison <see cref="Equals(Countable{T}?)"/></param>
    /// <returns></returns>
    public static Countable<T> New<T>(T value, bool hashCodeIncludesCount) => new(value, 1, hashCodeIncludesCount);

    /// <summary>
    /// Creates a new Countable object.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value which should be counted.</param>
    /// <param name="count">The seed of the counter.</param>
    /// <param name="hashCodeIncludesCount">If true, then the counter is included in the hash value and the comparison <see cref="Equals(Countable{T}?)"/></param>
    /// <returns></returns>
    public static Countable<T> New<T>(T value, int count, bool hashCodeIncludesCount) => new(value, count, hashCodeIncludesCount);
}

/// <summary>
/// This class allows a value to be counted.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class Countable<T> : IEquatable<Countable<T>>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">The countable value.</param>
    /// <param name="count">The seed of the counter.</param>
    /// <param name="hashCodeIncludesCount">If true, then the counter is included in the hash value and the comparison <see cref="Equals(Countable{T}?)"/></param>
	public Countable(T? value, int count, bool hashCodeIncludesCount)
	{
		Value = value;
		Count = count;
		HashCodeIncludesCount = hashCodeIncludesCount;
	}

    public static implicit operator T?(Countable<T> countable)
    {
        return countable.Value;
    }

    /// <summary>
    /// If HashCodeIncludesCount is true, then the counter is included in the hash value and the comparison <see cref="Equals(Countable{T}?)"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Countable<T> lhs, Countable<T> rhs)
	{
		return lhs.Equals(rhs);
	}

    public static bool operator ==(Countable<T> lhs, T? rhs)
    {
        return lhs.Value.EqualsNullable(rhs);
    }

    public static bool operator ==(T? lhs, Countable<T> rhs)
    {
        return lhs.EqualsNullable(rhs.Value);
    }

    /// <summary>
    /// If HashCodeIncludesCount is true, then the counter is included in the hash value and the comparison <see cref="Equals(Countable{T}?)"/>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Countable<T> lhs, Countable<T> rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator !=(Countable<T> lhs, T? rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator !=(T? lhs, Countable<T> rhs)
    {
        return !(lhs == rhs);
    }

    /// <summary>
    /// The counter of the value.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Decreases Count by 1.
    /// </summary>
    public void Dec()
    {
        if (0 == Count) return;
        
        Count--;
    }

    /// <summary>
    /// If HashCodeIncludesCount is true, then the counter is included in the hash value as well in the comparison.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
	public override bool Equals(object? obj) => Equals(obj as Countable<T>);

    /// <summary>
    /// If HashCodeIncludesCount is true, then the counter is included in the hash value as well in the comparison.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
	public bool Equals(Countable<T>? other)
	{
		if (other is null) return false;
		if (!Value.EqualsNullable(other.Value)) return false;
		return HashCodeIncludesCount ? Count == other.Count : true;
	}

    /// <summary>
    /// If HashCodeIncludesCount is true, then the counter is included in the hash value as well in the comparison.
    /// </summary>
    /// <returns></returns>
	public override int GetHashCode()
	{
		if(HashCodeIncludesCount) return Value is null ? System.HashCode.Combine(0, Count) : System.HashCode.Combine(Value, Count);

		return Value.GetNullableHashCode();
	}

    /// <summary>
    /// Returns the hash code of the Value.
    /// </summary>
    /// <returns></returns>
    public int GetValueHashCode() => Value.GetNullableHashCode();

    /// <summary>
    /// If HashCodeIncludesCount is true, then the counter is included in the hash value as well in the comparison.
    /// </summary>
	public bool HashCodeIncludesCount { get; }

    /// <summary>
    /// Increases Count by 1.
    /// </summary>
    public void Inc() => Count++;

    public T? Value { get; }

    /// <summary>
    /// Compares only the values.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
	public bool ValueEquals(Countable<T>? other)
	{
		return other is not null && Value.EqualsNullable(other.Value);
	}
}
