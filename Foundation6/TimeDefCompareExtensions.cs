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
﻿using Foundation.Collections.Generic;

namespace Foundation;

public static class TimeDefCompareExtensions
{

    public static int CompareSpanTimeDefs(this TimeDef lhs, TimeDef rhs)
    {
        lhs.ThrowIfOutOfRange(() => !lhs.IsSpanTimeDef());
        rhs.ThrowIfOutOfRange(() => !rhs.IsSpanTimeDef());

        var leftValue = TimeDefHelper.GetValueOfSpanTimeDef(lhs);
        var rightValue = TimeDefHelper.GetValueOfSpanTimeDef(rhs);

        return leftValue.CompareTo(rightValue);
    }

    public static int CompareTo(this TimeDef lhs, TimeDef rhs)
    {
        var leftWeight = TimeDefHelper.ChronologicalOrderWeight(lhs);
        var rightWeight = TimeDefHelper.ChronologicalOrderWeight(rhs);
        var cmp = leftWeight.CompareTo(rightWeight);
        if (0 != cmp) return cmp;

        if (lhs is TimeDef.QuantityTimeDef qLeft)
        {
            var qRight = (TimeDef.QuantityTimeDef)rhs;
            return qLeft.CompareTo(qRight);
        }

        if (lhs.IsSpanTimeDef()) return lhs.CompareSpanTimeDefs(rhs);

        if (lhs.IsValueCollectionTimeDef())
        {
            var leftValues = TimeDefHelper.GetValuesOfValueCollectionTimeDef(lhs);
            var rightValues = TimeDefHelper.GetValuesOfValueCollectionTimeDef(rhs);
            return leftValues.CompareTo(rightValues);
        }

        if(lhs is TimeDef.BinaryTimeDef binLeft)
        {
            var binRight = (TimeDef.BinaryTimeDef)rhs;
            return binLeft.CompareTo(binRight);
        }
        return 0;
    }

    public static int CompareTo(this TimeDef.BinaryTimeDef lhs, TimeDef.BinaryTimeDef rhs)
    {
        var cmp = lhs.Lhs.CompareTo(rhs.Lhs);
        if (0 != cmp) return cmp;

        return lhs.Rhs.CompareTo(rhs.Rhs);
    }

    public static int CompareTo(this TimeDef.DateTimeSpan lhs, TimeDef.DateTimeSpan rhs)
    {
        var cmp = lhs.From.CompareTo(rhs.From);
        if (0 != cmp) return cmp;

        return lhs.To.CompareTo(rhs.To);
    }

    public static int CompareTo(this TimeDef.Day lhs, TimeDef.Day rhs)
    {
        var cmp = lhs.DaysOfMonth.Count.CompareTo(rhs.DaysOfMonth.Count);
        if (0 != cmp) return cmp;

        return lhs.DaysOfMonth.Sum().CompareTo(rhs.DaysOfMonth.Sum());
    }

    public static int CompareTo(this TimeDef.Hour lhs, TimeDef.Hour rhs)
    {
        var cmp = lhs.HoursOfDay.Count.CompareTo(rhs.HoursOfDay.Count);
        if (0 != cmp) return cmp;

        return lhs.HoursOfDay.Sum().CompareTo(rhs.HoursOfDay.Sum());
    }

    public static int CompareTo(this TimeDef.Hours lhs, TimeDef.Hours rhs)
    {
        return lhs.Quantity.CompareTo(rhs.Quantity);
    }

    public static int CompareTo(this TimeDef.Minute lhs, TimeDef.Minute rhs)
    {
        var cmp = lhs.MinutesOfHour.Count.CompareTo(rhs.MinutesOfHour.Count);
        if (0 != cmp) return cmp;

        return lhs.MinutesOfHour.Sum().CompareTo(rhs.MinutesOfHour.Sum());
    }

    public static int CompareTo(this TimeDef.Month lhs, TimeDef.Month rhs)
    {
        var cmp = lhs.MonthsOfYear.Count.CompareTo(rhs.MonthsOfYear.Count);
        if (0 != cmp) return cmp;

        return lhs.MonthsOfYear.Cast<int>().Sum().CompareTo(rhs.MonthsOfYear.Cast<int>().Sum());
    }

    public static int CompareTo(this TimeDef.QuantityTimeDef lhs, TimeDef.QuantityTimeDef rhs)
    {
        var cmp = lhs.Quantity.CompareTo(rhs.Quantity);
        if (0 != cmp) return cmp;

        var leftWeeks = lhs as TimeDef.Weeks;
        var rightWeeks = rhs as TimeDef.Weeks;

        if(null != leftWeeks && null != rightWeeks) return leftWeeks.WeekStartsWith.CompareTo(rightWeeks.WeekStartsWith);
        
        var leftWeight = TimeDefHelper.ChronologicalOrderWeight(lhs);
        var rightWeight = TimeDefHelper.ChronologicalOrderWeight(rhs);
        return leftWeight.CompareTo(rightWeight);
    }

#if NET6_0_OR_GREATER
    public static int CompareTo(this TimeDef.Timespan lhs, TimeDef.Timespan rhs)
    {
        var cmp = lhs.From.CompareTo(rhs.From);
        if (0 != cmp) return cmp;

        return lhs.To.CompareTo(rhs.To);
    }
#endif

    public static int CompareTo(this TimeDef.Weekday lhs, TimeDef.Weekday rhs)
    {
        var cmp = lhs.DaysOfWeek.Count.CompareTo(rhs.DaysOfWeek.Count);
        if (0 != cmp) return cmp;

        return lhs.DaysOfWeek.Cast<int>().Sum().CompareTo(rhs.DaysOfWeek.Cast<int>().Sum());
    }

    public static int CompareTo(this TimeDef.WeekOfMonth lhs, TimeDef.WeekOfMonth rhs)
    {
        var cmp = lhs.WeekStartsWith.CompareTo(rhs.WeekStartsWith);
        if (0 != cmp) return cmp;

        cmp = lhs.Week.Count.CompareTo(rhs.Week.Count);
        if (0 != cmp) return cmp;

        return lhs.Week.Sum().CompareTo(rhs.Week.Sum());
    }

    public static int CompareTo(this TimeDef.Weeks lhs, TimeDef.Weeks rhs)
    {
        var cmp = lhs.WeekStartsWith.CompareTo(rhs.WeekStartsWith);
        if (0 != cmp) return cmp;

        return lhs.Quantity.CompareTo(rhs.Quantity);
    }

    public static int CompareTo(this TimeDef.Year lhs, TimeDef.Year rhs)
    {
        var cmp = lhs.YearsOfPeriod.Count.CompareTo(rhs.YearsOfPeriod.Count);
        if (0 != cmp) return cmp;

        return lhs.YearsOfPeriod.Sum().CompareTo(rhs.YearsOfPeriod.Sum());
    }
}
