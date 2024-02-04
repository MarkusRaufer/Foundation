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
/// Inherits from <see cref=OneOf{}/> and represents any number type.
/// </summary>
public sealed class Number : OneOf<byte, char, decimal, int, long, sbyte, short, uint, ulong, ushort>
{
    public Number()
    {
    }

    public Number(byte t1) : base(t1)
    {
    }

    public Number(char t2) : base(t2)
    {
    }

    public Number(decimal t3) : base(t3)
    {
    }

    public Number(int t4) : base(t4)
    {
    }

    public Number(long t5) : base(t5)
    {
    }

    public Number(sbyte t6) : base(t6)
    {
    }

    public Number(short t7) : base(t7)
    {
    }

    public Number(uint t8) : base(t8)
    {
    }

    public Number(ulong t9) : base(t9)
    {
    }

    public Number(ushort t10) : base(t10)
    {
    }

    public static implicit operator Number(byte number) => new(number);
    public static implicit operator Number(char number) => new(number);
    public static implicit operator Number(decimal number) => new(number);
    public static implicit operator Number(int number) => new(number);
    public static implicit operator Number(long number) => new(number);
    public static implicit operator Number(sbyte number) => new(number);
    public static implicit operator Number(short number) => new(number);
    public static implicit operator Number(uint number) => new(number);
    public static implicit operator Number(ulong number) => new(number);
    public static implicit operator Number(ushort number) => new(number);

    public static Number New(byte number) => new (number);
    public static Number New(char number) => new(number);
    public static Number New(decimal number) => new (number);
    public static Number New(int number) => new (number);
    public static Number New(long number) => new (number);
    public static Number New(sbyte number) => new(number);
    public static Number New(short number) => new (number);
    public static Number New(uint number) => new(number);
    public static Number New(ulong number) => new(number);
    public static Number New(ushort number) => new (number);
}
