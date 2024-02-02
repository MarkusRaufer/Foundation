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
﻿using System.Diagnostics.CodeAnalysis;

namespace Foundation
{
    public static class NullableKey
    {
        public static NullableKey<T> New<T>(T? value) => new (value);
    }

    /// <summary>
    /// Can be used as key for dictionaries. This allows null keys.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct NullableKey<T> : IEquatable<NullableKey<T>>
    {
        public NullableKey(T? value)
        {
            Value = value;
        }

        public static bool operator ==(NullableKey<T> left, NullableKey<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NullableKey<T> left, NullableKey<T> right)
        {
            return !(left == right);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is NullableKey<T> other && Equals(other);
        
        public bool Equals(NullableKey<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode() => Value.GetNullableHashCode();

        public bool IsNull => Value is null;

        public T? Value { get; }

        public override string ToString() => $"{Value}";
    }
}
