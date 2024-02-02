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

public static class ObservableValue
{
    public static ObservableValue<T> New<T>(T value)
    {
        return new ObservableValue<T>(value);
    }
}

public struct ObservableValue<T> : IEquatable<ObservableValue<T>>
{
    public event Action<T>? ValueChanged;

    private T _value;

    public ObservableValue(T? value)
    {
        _value = value.ThrowIfNull();
        ValueChanged = default;
    }

    public static bool operator ==(ObservableValue<T> left, ObservableValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ObservableValue<T> left, ObservableValue<T> right)
    {
        return !(left == right);
    }

    public bool IsEmpty => null == Value;

    public bool Equals(ObservableValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ObservableValue<T> other) return false;
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && Value!.Equals(other.Value);
    }

    public override int GetHashCode() => IsEmpty ? 0 : Value!.GetHashCode();

    public T Value
    {
        get => _value;
        set
        {
            if (null != _value && _value.Equals(value)) return;

            _value = value;
            ValueChanged?.Invoke(_value);
        }
    }
}
