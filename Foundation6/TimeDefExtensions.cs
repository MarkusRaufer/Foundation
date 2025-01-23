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
using Foundation.Collections.Generic;

namespace Foundation;

public static class TimeDefExtensions
{
    public static IEnumerable<TimeDef> BothSides(this TimeDef.And and)
    {
        yield return and.Lhs;
        yield return and.Rhs;
    }

    public static IEnumerable<TimeDef> BothSides(this TimeDef.Difference difference)
    {
        yield return difference.Lhs;
        yield return difference.Rhs;
    }

    public static IEnumerable<TimeDef> BothSides(this TimeDef.Or or)
    {
        yield return or.Lhs;
        yield return or.Rhs;
    }

    public static IEnumerable<TimeDef> BothSides(this TimeDef.Union union)
    {
        yield return union.Lhs;
        yield return union.Rhs;
    }

    /// <summary>
    /// Chains timeDefs by a binary TimeDef.
    /// </summary>
    /// <param name="timeDefs"></param>
    /// <param name="binaryOperationFactory"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TimeDef Chain(this IEnumerable<TimeDef> timeDefs, Func<TimeDef, TimeDef, TimeDef> binaryOperationFactory)
    {
        binaryOperationFactory.ThrowIfNull();

        var it = timeDefs.GetEnumerator();
        if (!it.MoveNext()) throw new ArgumentOutOfRangeException(nameof(timeDefs), "must not be empty");

        var lhs = it.Current;
        
        while (it.MoveNext())
        {
            lhs = binaryOperationFactory(lhs, it.Current);
        }

        return lhs;
    }

    /// <summary>
    /// Chains timeDefs by And.
    /// </summary>
    /// <param name="timeDefs"></param>
    /// <returns></returns>
    public static TimeDef ChainByAnd(this IEnumerable<TimeDef> timeDefs)
    {
        return Chain(timeDefs, (l, r) => new TimeDef.And(l, r));
    }

    /// <summary>
    /// Chains timeDefs by Or.
    /// </summary>
    /// <param name="timeDefs"></param>
    /// <returns></returns>
    public static TimeDef ChainByOr(this IEnumerable<TimeDef> timeDefs)
    {
        return Chain(timeDefs, (l, r) => new TimeDef.Or(l, r));
    }

    public static bool Equals(this TimeDef lhs, TimeDef rhs)
    {
        return lhs switch
        {
            TimeDef.And l => rhs is TimeDef.And r && Equals(l.Lhs, r.Lhs) && Equals(l.Rhs, r.Rhs),
            TimeDef.DateSpan l => rhs is TimeDef.DateSpan r && l.Equals(r),
            TimeDef.DateTimeSpan l => rhs is TimeDef.DateTimeSpan r && l.Equals(r),
            TimeDef.Day l => rhs is TimeDef.Day r && l.Equals(r),
            TimeDef.Days l => rhs is TimeDef.Days r && l.Equals(r),
            TimeDef.Difference l => rhs is TimeDef.Difference r && Equals(l.Lhs, r.Lhs) && Equals(l.Rhs, r.Rhs),
            TimeDef.Hour l => rhs is TimeDef.Hour r && l.Equals(r),
            TimeDef.Hours l => rhs is TimeDef.Hours r && l.Equals(r),
            TimeDef.Minute l => rhs is TimeDef.Minute r && l.Equals(r),
            TimeDef.Minutes l => rhs is TimeDef.Minutes r && l.Equals(r),
            TimeDef.Month l => rhs is TimeDef.Month r && l.Equals(r),
            TimeDef.Months l => rhs is TimeDef.Months r && l.Equals(r),
            TimeDef.Not l => rhs is TimeDef.Not r && Equals(l.TimeDef, r.TimeDef),
            TimeDef.Or l => rhs is TimeDef.Or r && Equals(l.Lhs, r.Lhs) && Equals(l.Rhs, r.Rhs),
            TimeDef.Timespan l => rhs is TimeDef.Timespan r && l.Equals(r),
            TimeDef.Union l => rhs is TimeDef.Union r && Equals(l.Lhs, r.Lhs) && Equals(l.Rhs, r.Rhs),
            TimeDef.Weekday l => rhs is TimeDef.Weekday r && l.Equals(r),
            TimeDef.WeekOfMonth l => rhs is TimeDef.WeekOfMonth r && l.Equals(r),
            TimeDef.Weeks l => rhs is TimeDef.Weeks r && l.Equals(r),
            TimeDef.Year l => rhs is TimeDef.Year r && l.Equals(r),
            TimeDef.Years l => rhs is TimeDef.Years r && l.Equals(r),
            _ => throw new NotImplementedException($"{lhs}")
        };
    }

    public static bool IsBinaryTimeDef(this TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.And or
            TimeDef.Difference or
            TimeDef.Or or
            TimeDef.Union => true,
            _ => false
        };
    }

    public static bool IsQuantityTimeDef(this TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.Days or
            TimeDef.Hours or
            TimeDef.Minutes or
            TimeDef.Weeks or
            TimeDef.Months or
            TimeDef.Years => true,
            _ => false
        };
    }

    public static bool IsSpanTimeDef(this TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.DateSpan or
            TimeDef.Timespan => true,
            TimeDef.DateTimeSpan or
            _ => false
        };
    }

    public static bool IsValueCollectionTimeDef(this TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.Minute or
            TimeDef.Hour or
            TimeDef.Day or
            TimeDef.Weekday or
            TimeDef.WeekOfMonth or
            TimeDef.Month or
            TimeDef.Year => true,
            _ => false
        };
    }

    public static bool IsValueTimeDef(this TimeDef td)
    {
        return td switch
        {
            TimeDef.Minutes or
            TimeDef.Hours or
            TimeDef.Days or
            TimeDef.Weeks or
            TimeDef.Months or
            TimeDef.Years => true,
            _ => false
        };
    }

    public static TimeSpan ToTimeSpan(this TimeDef.Minutes minutes) => TimeSpan.FromMinutes(minutes.Quantity);
    public static TimeSpan ToTimeSpan(this TimeDef.Hours hours) => TimeSpan.FromHours(hours.Quantity);
    public static TimeSpan ToTimeSpan(this TimeDef.Days days) => TimeSpan.FromDays(days.Quantity);
    public static TimeSpan ToTimeSpan(this TimeDef.Weeks weeks) => TimeSpan.FromDays(weeks.Quantity);
    public static TimeSpan ToTimeSpan(this TimeDef.DateSpan dateSpan) => dateSpan.To.Subtract(dateSpan.From);
    public static TimeSpan ToTimeSpan(this TimeDef.Timespan dateSpan) => dateSpan.To.Subtract(dateSpan.From);
}

