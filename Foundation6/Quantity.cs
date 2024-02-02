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

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity(string Unit, decimal Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();

    public static Quantity New(string unit, decimal value) => new(unit, value);

    public static Quantity<TValue> New<TValue>(string unit, TValue value) => new(unit, value);

    public static Quantity<TUnit, TValue> New<TUnit, TValue>(TUnit unit, TValue value) => new(unit, value);
}

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <typeparam name="TValue">Type of the value of the quantity.</typeparam>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity<TValue>(string Unit, TValue Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();
}

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <typeparam name="TUnit">The unit type of the quantity.</typeparam>
/// <typeparam name="TValue">Type of the value of the quantity.</typeparam>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity<TUnit, TValue>(TUnit Unit, TValue Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();
}

