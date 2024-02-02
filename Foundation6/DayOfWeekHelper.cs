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
using System.Globalization;

public class DayOfWeekHelper
{

    public static IEnumerable<DayOfWeek> CycleDaysOfWeek(DayOfWeek start = DayOfWeek.Sunday)
    {
        return AllDaysOfWeek().Cycle().Skip((int)start);
    }

    /// <summary>
    /// Returns days that can be uses as first day of week.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<DayOfWeek> FirstDaysOfWeek()
    {
        yield return DayOfWeek.Sunday;
        yield return DayOfWeek.Monday;
    }

    public static IEnumerable<DayOfWeek> AllDaysOfWeek(DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
    {
        if(DayOfWeek.Sunday == firstDayOfWeek)
        {
            foreach (DayOfWeek weekDay in Enum.GetValues(typeof(DayOfWeek)))
                yield return weekDay;

            yield break;
        }

        yield return DayOfWeek.Monday;
        yield return DayOfWeek.Tuesday;
        yield return DayOfWeek.Wednesday;
        yield return DayOfWeek.Thursday;
        yield return DayOfWeek.Friday;
        yield return DayOfWeek.Saturday;
        yield return DayOfWeek.Sunday;
    }
    
    public static bool IsValidFirstDayOfWeek(DayOfWeek dayOfWeek)
    {
        return DayOfWeek.Sunday == dayOfWeek || DayOfWeek.Monday == dayOfWeek;
    }

    public static bool IsValidFirstDayOfWeek(DayOfWeek dayOfWeek, CultureInfo cultureInfo)
    {
        return cultureInfo.DateTimeFormat.FirstDayOfWeek == dayOfWeek;
    }
}

