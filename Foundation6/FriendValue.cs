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
﻿// The MIT License (MIT)
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

/// <summary>
/// This value can only be changed by a friends instance.
/// Only the creator of the <see cref="FriendValue{T}"/> can change the <see cref="Value"/> after it has been set.
/// </summary>
public static class FriendValue
{
    /// <summary>
    /// Factory method to create a <see cref="FriendValue{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="friend">The friend instance.</param>
    /// <param name="value">The value of the friend relationship.</param>
    /// <returns></returns>
    public static FriendValue<T> New<T>(object friend, T value) => new(friend, value);
}

/// <summary>
/// This value can only be changed by a friends instance.
/// Only the creator of the <see cref="FriendValue{T}"/> can change the <see cref="Value"/> after it has been set.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class FriendValue<T>
{
    private readonly object _friend;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="friend">The friend who can change the value after it has been set.</param>
    /// <param name="value">The value of the friend relationship.</param>
    public FriendValue(object friend, T value)
    {
        _friend = friend.ThrowIfNull();
        Value = value;
    }

    /// <summary>
    /// <see cref="Value"/> can only be set by the <paramref name="friend"/> instance.
    /// </summary>
    /// <param name="friend">The friend relationship.</param>
    /// <returns>Returns <see cref="Value"/></returns>
    public T this[object friend]
    {
        get => Value;
        set
        {
            if (friend is not null && friend.Equals(_friend)) Value = value;
        }
    }

    /// <summary>
    /// The value of the friend relationship.
    /// </summary>
    public T Value { get; private set; }
}

/// <summary>
/// This value can only be changed by friend types means of exact type or if it is assignable to the type <typeparamref name="TFriend"/>.
/// This can be used like friend relationship in C++.
/// </summary>
/// <typeparam name="TFriend">The type of the friend.</typeparam>
/// <typeparam name="T">Type of the value.</typeparam>
/// <param name="value">The value of the friend relationship.</param>
public sealed class FriendValue<TFriend, T>(T value)
{
    private readonly Type _type = typeof(TFriend);

    /// <summary>
    /// <see cref="Value"/> can only be set by a <typeparamref name="TFriend"/>.
    /// </summary>
    /// <param name="friend">The friend relationship.</param>
    /// <returns>Returns <see cref="Value"/></returns>
    public T this[TFriend friend]
    {
        get => Value;
        set
        {
            if (friend is null) return;

            var type = friend.GetType();
            if (type == _type || _type.IsAssignableFrom(type)) Value = value;
        }
    }

    /// <summary>
    /// The value of the friend relationship.
    /// </summary>
    public T Value { get; private set; } = value.ThrowIfNull();
}
