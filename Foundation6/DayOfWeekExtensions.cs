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

using Foundation.Collections.Generic;

public static class DayOfWeekExtensions
{
    public static DayOfWeek GetWeekDay(this DayOfWeek dayOfWeek, int days)
    {
        if(0 > days) throw new ArgumentOutOfRangeException(nameof(days));

        var i = 0;

        foreach (DayOfWeek d in DayOfWeekHelper.AllDaysOfWeek().Cycle())
        {
            i++;
            if (i == days) return d;
        }

        throw new NotFiniteNumberException();
    }

    public static DayOfWeek LastDayOfWeek(this DayOfWeek start)
    {
        if (DayOfWeek.Monday == start)
            return DayOfWeek.Sunday;

        if (DayOfWeek.Sunday == start)
            return DayOfWeek.Saturday;

        throw new ArgumentException($"start can be {DayOfWeek.Sunday} or {DayOfWeek.Monday}");
    }
}

