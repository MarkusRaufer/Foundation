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

using System.ComponentModel;

/// <summary>
/// Ok-object with a name and a value It implements INotifyPropertyChanged.
/// </summary>
public sealed class Property : Property<object>
{
    public Property(string name, object? value = null) : base(name, value)
    {
    }
}

/// <summary>
/// Ok-object with a name and a value It implements INotifyPropertyChanged.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class Property<TValue>
    : IEquatable<Property<TValue>>
    , INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _hashCode;
    private TValue? _value;

    public Property(string name, TValue? value = default)
    {
        Name = name.ThrowIfNullOrEmpty();
        _value = value;

        IsPropertyChangedActive = true;

#if NETSTANDARD2_0
        _hashCode = Foundation.HashCode.FromObject(Name, _value);
#else
        _hashCode = System.HashCode.Combine(Name, _value);
#endif
    }

    public static bool operator ==(Property<TValue>? lhs, Property<TValue>? rhs)
    {
        if (lhs is null) return rhs is null;
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Property<TValue>? lhs, Property<TValue>? rhs)
    {
        return !(lhs == rhs);
    }

    public void Deconstruct(out string name, out object? value)
    {
        name = Name;
        value = Value;
    }


    public override bool Equals(object? obj) => Equals(obj as Property);

    public bool Equals(Property<TValue>? other)
    {
        return null != other && Name.EqualsNullable(other.Name) && Value.EqualsNullable(other.Value);
    }

    public override int GetHashCode() => _hashCode;

    public bool IsDirty { get; set; }

    public bool IsPropertyChangedActive { get; set; }

    public string Name { get; }

    public override string ToString() => $"({Name}, {Value})";

    public TValue? Value
    {
        get => _value;
        set
        {
            if (_value.EqualsNullable(value)) return;

            _value = value;
            if (IsPropertyChangedActive)
            {
                IsDirty = true;
#if NETSTANDARD2_0
                _hashCode = Foundation.HashCode.FromObject(Name, _value);
#else
                _hashCode = System.HashCode.Combine(Name, _value);
#endif
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
            }
        }
    }
}

