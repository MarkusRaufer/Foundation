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
﻿using System.Runtime.Serialization;

namespace Foundation.Collections.Generic;

public static class EquatableList
{
    public static EquatableList<T> New<T>(params T[] values)
    {
        return [.. values];
    }
}

/// <summary>
/// This list checks the equality of all elements and their positions.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class EquatableList<T>
    : List<T>
    , IEquatable<EquatableList<T>>
    , ISerializable
{
    private const string SerializationKey = "items";

    private int _hashCode;

    public EquatableList()
    {
        CreateHashCode();
    }

    public EquatableList(IEnumerable<T> collection)
    {
        foreach (T item in collection)
            base.Add(item);

        CreateHashCode();
    }

    public EquatableList(int capacity) : base(capacity)
    {
        CreateHashCode();
    }

    public EquatableList(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(SerializationKey, typeof(List<T>)) is List<T> collection)
        {
            foreach (var item in collection)
                base.Add(item);
        }

        CreateHashCode();
    }

    public new T this[int index]
    {
        get => base[index];
        set
        {

            var existingValue = base[index];

            if (EqualityComparer<T>.Default.Equals(existingValue, value)) return;

            base[index] = value;

            CreateHashCode();
        }
    }

    public new void Add(T item)
    {
        base.Add(item);

        _hashCode = HashCode.CreateBuilder()
                            .AddHashCode(_hashCode)
                            .AddObject(item)
                            .GetHashCode();
    }

    protected void CreateHashCode()
    {
        _hashCode = HashCode.CreateBuilder()
                                    .AddObject(DefaultHashCode)
                                    .AddObjects(this)
                                    .GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableList<T>).GetHashCode();

    /// <summary>
    /// Checks the equality of all elements and their position.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as EquatableList<T>);

    /// <summary>
    /// Checks the equality of all elements and their position.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EquatableList<T>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return this.SequenceEqual(other);
    }

    /// <summary>
    /// Considers values and their position.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(SerializationKey, this);
    }

    public new bool Remove(T item)
    {
        if (base.Remove(item))
        {
            CreateHashCode();
            return true;
        }

        return false;
    }
}
