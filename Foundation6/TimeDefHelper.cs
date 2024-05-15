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
﻿using static Foundation.TimeDef;

namespace Foundation;

public static class TimeDefHelper
{
    public static int ChronologicalOrderWeight(TimeDef timedef)
    {
        return timedef switch
        {
            Minute => 0,
            Minutes => 1,
            Hour => 2,
            Hours => 3,
#if NET6_0_OR_GREATER
            Timespan => 4,
#endif
            Day => 5,
            Days => 6,
            Weekday => 7,
            WeekOfMonth => 8,
            Weeks => 9,
            TimeDef.Month => 10,
            Months => 11,
#if NET6_0_OR_GREATER
            DateSpan => 12,
#endif
            Year => 13,
            Years => 14,
            Difference => 15,
            Union => 16,
            Or => 17,
            And => 18,
            _ => throw new NotImplementedException($"{timedef}")
        };
    }

    //public static int Compare(TimeDef lhs, TimeDef rhs)
    //{
    //    var lhsWeight = ValueTimeDefSortOrderWeight(lhs);
    //    var rhsWeight = ValueTimeDefSortOrderWeight(rhs);

    //    return lhsWeight.CompareTo(rhsWeight);
    //}

    public static TimeSpan GetValueOfSpanTimeDef(TimeDef timedef)
    {
        return timedef switch
        {
#if NET6_0_OR_GREATER
            DateSpan td => td.To.Subtract(td.From),
            Timespan td => td.To.Subtract(td.From),
#endif
            DateTimeSpan td => td.To.Subtract(td.From),
            _ => TimeSpan.Zero
        };
    }

    public static object GetValueOfValueTimeDef(TimeDef timedef)
    {
        return timedef switch
        {
#if NET6_0_OR_GREATER
            DateSpan td => td.To.Subtract(td.From),
            Timespan td => td.To.Subtract(td.From),
#endif
            DateTimeSpan td => td.To.Subtract(td.From),
            QuantityTimeDef td => td.Quantity,
            _ => 0
        };
    }

    public static IEnumerable<int> GetValuesOfValueCollectionTimeDef(TimeDef timedef)
    {
        return timedef switch
        {
            Day td => td.DaysOfMonth,
            Hour td => td.HoursOfDay,
            Minute td => td.MinutesOfHour,
            TimeDef.Month td => td.MonthsOfYear.Cast<int>(),
            Weekday td => td.DaysOfWeek.Cast<int>(),
            WeekOfMonth td => td.Week.Append((int)td.WeekStartsWith),
            Year td => td.YearsOfPeriod,
            _ => Enumerable.Empty<int>()
        };
    }

#if NET6_0_OR_GREATER
    public static Period ToPeriod(TimeDef.DateSpan dateSpan, DateTimeKind kind)
    {
        return Period.New(dateSpan.From, dateSpan.To, kind);
    }
#endif

    public static Period ToPeriod(TimeDef.DateTimeSpan dateTimeSpan, DateTimeKind kind)
    {
        return Period.New(dateTimeSpan.From, dateTimeSpan.To);
    }

#if NET6_0_OR_GREATER
    public static Period ToPeriod(TimeDef.Timespan timespan, DateTimeKind kind)
    {
        return Period.New(timespan.From, timespan.To, kind);
    }
#endif

    public static int ValueTimeDefSortOrderWeight(TimeDef timedef)
    {
        return timedef switch
        {
#if NET6_0_OR_GREATER
            DateSpan td => valueOfDateSpan(td.From, td.To),
            Timespan td => valueOfTimespan(td.From, td.To),
#endif
            DateTimeSpan td => valueOfDateTimeSpan(td.From, td.To),
            Day => 4,
            Days => 4,
            Hour => 5,
            Hours => 5,
            Minute => 6,
            Minutes => 6,
            Weekday => 4,
            WeekOfMonth => 3,
            Weeks => 3,
            TimeDef.Month => 2,
            Months => 2,
            Year => 1,
            Years => 1,
            _ => 10,
        };

#if NETSTANDARD2_0
        int valueOfDateSpan(DateTime from, DateTime to)
        {
            if (from.Year != to.Year) return 1;
            if (from.Month != to.Month) return 2;
            return 4;
        }

         int valueOfDateTimeSpan(DateTime from, DateTime to)
        {
            var weight = valueOfDateSpan(from, to);
            if (weight > 2) return weight;

            return valueOfTimespan(from, to);
        }

        int valueOfTimespan(DateTime from, DateTime to)
        {
            if (from.Hour != to.Hour) return 6;
            if (from.Minute != to.Minute) return 5;

            return 7;
        }
#else
        int valueOfDateSpan(DateOnly from, DateOnly to)
        {
            if (from.Year != to.Year) return 1;
            if (from.Month != to.Month) return 2;
            return 4;
        }

        int valueOfDateTimeSpan(DateTime from, DateTime to)
        {
            var weight = valueOfDateSpan(from.ToDateOnly(), to.ToDateOnly());
            if (weight > 2) return weight;

            return valueOfTimespan(from.ToTimeOnly(), to.ToTimeOnly());
        }

        int valueOfTimespan(TimeOnly from, TimeOnly to)
        {
            if (from.Hour != to.Hour) return 6;
            if (from.Minute != to.Minute) return 5;

            return 7;
        }
#endif
    }
}
