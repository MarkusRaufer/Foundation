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
﻿using System.Globalization;

namespace Foundation;

/// <summary>
/// Amount of money including currency
/// </summary>
/// <param name="RegionInfo"></param>
/// <param name="Amount"></param>
public sealed record Money(RegionInfo RegionInfo, decimal Amount) : IComparable<Money>
{
    public static Money New(string name, decimal amount) => new (new RegionInfo(name), amount);

    public static bool operator <(Money lhs, Money rhs) => -1 == lhs.CompareTo(rhs);
    public static bool operator <=(Money lhs, Money rhs) => 0 >= lhs.CompareTo(rhs);
    public static bool operator >(Money lhs, Money rhs) => 1 == lhs.CompareTo(rhs);
    public static bool operator >=(Money lhs, Money rhs) => 0 <= lhs.CompareTo(rhs);

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        var cmp = RegionInfo.Name.CompareTo(other.RegionInfo.Name);
        if (0 != cmp) return cmp;

        return Amount.CompareTo(other.Amount);
    }
}
