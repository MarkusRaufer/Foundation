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

public interface INumeric<T>
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    T Add(T a, T b);
    T Divide(T a, T b);
    T Multiply(T a, T b);
    T Subtract(T a, T b);
}

public struct NumByte : INumeric<byte>
{
    public byte Add(byte a, byte b) => (byte)(a + b);

    public byte Divide(byte a, byte b) => (byte)(a / b);

    public byte Multiply(byte a, byte b) => (byte)(a * b);

    public byte Subtract(byte a, byte b) => (byte)(a - b);
}

public struct NumDecimal : INumeric<decimal>
{
    public decimal Add(decimal a, decimal b) => a + b;

    public decimal Divide(decimal a, decimal b) => a / b;

    public decimal Multiply(decimal a, decimal b) => a * b;

    public decimal Subtract(decimal a, decimal b) => a - b;
}

public struct NumDouble : INumeric<double>
{
    public double Add(double a, double b) => a + b;

    public double Divide(double a, double b) => a / b;

    public double Multiply(double a, double b) => a * b;

    public double Subtract(double a, double b) => a - b;

}

public struct NumFloat : INumeric<float>
{
    public float Add(float a, float b) => a + b;

    public float Divide(float a, float b) => a / b;

    public float Multiply(float a, float b) => a * b;

    public float Subtract(float a, float b) => a - b;
}

public struct NumInt : INumeric<int>
{
    public int Add(int a, int b) => a + b;

    public int Divide(int a, int b) => a / b;

    public int Multiply(int a, int b) => a * b;

    public int Subtract(int a, int b) => a - b;
}

public struct NumLong : INumeric<long>
{
    public long Add(long a, long b) => a + b;

    public long Divide(long a, long b) => a / b;

    public long Multiply(long a, long b) => a * b;

    public long Subtract(long a, long b) => a - b;
}

public struct NumShort : INumeric<short>
{
    public short Add(short a, short b) => (short)(a + b);

    public short Divide(short a, short b) => (short)(a / b);

    public short Multiply(short a, short b) => (short)(a * b);

    public short Subtract(short a, short b) => (short)(a - b);
}

