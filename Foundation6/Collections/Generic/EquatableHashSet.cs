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
﻿namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
using System.Runtime.Serialization;

public static class EquatableHashSet
{
    public static EquatableHashSet<T> New<T>(IEnumerable<T> items) => new(items);
}

/// <summary>
/// This hashset considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
#if NETSTANDARD2_0
[Serializable]
#endif
public class EquatableHashSet<T>
    : HashSet<T>
    , ICollectionChanged<T>
    , IEquatable<EquatableHashSet<T>>
    , IEquatable<IEnumerable<T>>
#if NETSTANDARD2_0
    , ISerializable
#endif
{
    private int _hashCode;

    public EquatableHashSet() : base()
    {
        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items">Items are added to the hashset.</param>
    public EquatableHashSet(IEnumerable<T> items) : base(items)
    {        
        _hashCode = CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEqualityComparer<T>? comparer) : base(comparer)
    {
        _hashCode = CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection">list of items.
    /// Use T[], Collection<typeparamref name="T"/> or List<typeparamref name="T"/></param>
    /// <param name="comparer"></param>
    public EquatableHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer) : base(collection, comparer)
    {
        _hashCode = CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

#if NET6_0_OR_GREATER
    public EquatableHashSet(int capacity) : base(capacity)
    {
        _hashCode = CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }
    
    public EquatableHashSet(int capacity, IEqualityComparer<T>? comparer)
        : base(capacity, comparer)
    {
        _hashCode = CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }
#endif

#if NETSTANDARD2_0
    public EquatableHashSet(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _hashCode = CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }
#endif

    /// <inheritdoc/>
    public new bool Add(T item)
    {
        var added = base.Add(item.ThrowIfNull());
        if (added)
        {
            _hashCode = CreateHashCode();

            CollectionChanged?.Publish(new { Action = CollectionAction.Add, Element = item });
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public new void Clear()
    {
        base.Clear();

        _hashCode = CreateHashCode();

        CollectionChanged?.Publish(new { Action = CollectionAction.Clear });
    }

    public Event<Action<CollectionEvent<T>>> CollectionChanged { get; private set; }

    protected int CreateHashCode()
    {
        return  HashCode.CreateBuilder()
                        .AddHashCode(DefaultHashCode)
                        .AddOrderedObjects(this)
                        .GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableHashSet<T>).GetHashCode();

    /// <summary>
    /// Checks the equality of all elements. The position of the elements are ignored.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as EquatableHashSet<T>);

    /// <summary>
    /// Checks the equality of all elements. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EquatableHashSet<T>? other)
    {
        if (null == other) return false;
        if (_hashCode != other._hashCode) return false;

        return SetEquals(other);
    }

    public bool Equals(IEnumerable<T>? other)
    {
        if (null == other) return false;

        return SetEquals(other);
    }

    /// <summary>
    /// Considers all elements. Position of the elements are ignored.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

#if NETSTANDARD2_0
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
#endif

    /// <inheritdoc/>
    public new bool Remove(T item)
    {
        item.ThrowIfNull();

        if (base.Remove(item))
        {
            _hashCode = CreateHashCode();
            CollectionChanged?.Publish(new { Action = CollectionAction.Remove, Element = item });
            return true;
        }
        return false;
    }
}
