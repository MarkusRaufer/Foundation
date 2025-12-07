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
﻿namespace Foundation.ComponentModel;

/// <summary>
/// This is an abstract entity type and can be used to quickly create a derived entity type
/// where only the Id is considered for equality.
/// </summary>
/// <typeparam name="TId">Type of the Id.</typeparam>
public abstract class Entity<TId> 
    : IEntity<TId>
    , IEquatable<IEntity<TId>>
{
    /// <summary>
    /// The creation of an entity needs an identifier <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    protected Entity(TId id)
    {
        Id = id.ThrowIfNull();
    }

    /// <summary>
    /// The identifier of the entity.
    /// </summary>
    public TId Id { get; }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as IEntity<TId>);

    /// <summary>
    /// This method only compares the Id property.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IEntity<TId>? other)
    {
        return other is not null && Id.EqualsNullable(other.Id);
    }

    /// <summary>
    /// This method returns the hash code of Id.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Id.GetNullableHashCode();

    /// <summary>
    /// Prints the Id of the entity.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(Id)}: {Id}";
}
