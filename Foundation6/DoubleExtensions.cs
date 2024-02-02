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

public static class DoubleExtensions
{
    /// <summary>
    /// Checks if equation is within the accepted deviation.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="deviation"></param>
    /// <returns></returns>
    public static bool Equal(this double left, double right, double deviation = double.Epsilon)
    {
        return (Math.Abs(left) - Math.Abs(right)) < deviation;
    }

    /// <summary>
    /// Checks if left is greater than right within the accepted deviation.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="deviation"></param>
    /// <returns></returns>
    public static bool GreaterThan(this double left, double right, double deviation = double.Epsilon)
    {
        return (left - right) > deviation;
    }

    public static bool GreaterThanOrEqual(this double left, double right, double deviation = double.Epsilon)
    {
        return Equal(left, right, deviation) || GreaterThan(left, right, deviation);
    }

    public static bool IsZero(this double value, double deviation = double.Epsilon)
    {
        return Math.Abs(value) < deviation;
    }

    public static bool LessThan(this double left, double right, double deviation = double.Epsilon)
    {
        return (left - right) < -deviation;
    }

    public static bool LessThanOrEqual(this double left, double right, double deviation = double.Epsilon)
    {
        return Equal(left, right, deviation) || LessThan(left, right, deviation);
    }
}
