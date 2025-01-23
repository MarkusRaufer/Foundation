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
using static Foundation.TimeDef;

public class PeriodHelper
{
    public static IEnumerable<Period> Chop(Period period, TimeDef td)
    {
        return td switch
        {
            DateSpan _ => period.Days(),
            Timespan _ => period.Minutes(),
            Day _ => period.Days(),
            Days _ => period.Days(),
            Hour _ => period.Hours(),
            Hours _ => period.Hours(),
            Minute _ => period.Minutes(),
            Minutes _ => period.Minutes(),
            TimeDef.Month _ => period.Months(),
            Months _ => period.Months(),
            Weekday _ => period.Days(),
            WeekOfMonth _ => period.Weeks(),
            Weeks _ => period.Weeks(),
            Year _ => period.Years(),
            Years _ => period.Years(),
            _ => Enumerable.Empty<Period>()
        };
    }

    public static int Compare(Period lhs, Period rhs, bool emptyIsSmaller = true)
    {
        if (lhs < rhs)
        {
            if (lhs.IsEmpty && !emptyIsSmaller) return 1;
            return -1;
        }
        if (lhs > rhs)
        {
            if (rhs.IsEmpty && !emptyIsSmaller) return -1;
            return 1;
        }
        return 0;
    }

    public static IEnumerable<Period> Intersect(IEnumerable<Period> lhs, IEnumerable<Period> rhs)
    {
        return IntersectGroup(lhs, rhs);
    }

    public static IEnumerable<Period> IntersectGroup(IEnumerable<Period> lhs, IEnumerable<Period> rhs, int rhsCount = 0)
    {
        var sortedRhs = (0 == rhsCount)
            ? new List<Period>()
            : new List<Period>(rhsCount);

        rhs.ForEach(sortedRhs.Add);
        sortedRhs.Sort(new PeriodComparer());

        var intersected = new List<Period>();
        foreach (var l in lhs)
        {
            var periods = new List<Period>();
            foreach (var r in sortedRhs)
            {
                if (r.Start > l.End) break;

                var result = l.Intersect(r.Start, r.End);
                if (result.TryGet(out Period value)) periods.Add(value);
            }
            if (0 == periods.Count) continue;
            periods.ForEach(intersected.Add);
        }
        return intersected;
    }

    public static IEnumerable<Period> Merge(params Period[] periods)
    {
        if (periods.Length == 0) yield break;
        if (periods.Length == 1)
        {
            yield return periods[0];
            yield break;
        }
        else if (periods.Length == 2)
        {
            var first = periods.First();
            var second = periods.Last();
            if (first.IsOverlapping(second))
                yield return first.Union(second).OrThrow();
            else
            {
                yield return first;
                yield return second;
            }
            yield break;
        }
        var sortedPeriods = periods.OrderBy(p => p.Start);
        var mergedPeriods = new LinkedList<Period>();
        foreach (var period in sortedPeriods)
        {
            if (mergedPeriods.Count == 0)
            {
                mergedPeriods.AddLast(period);
                continue;
            }

            var prevPeriod = mergedPeriods.Last();
            if (prevPeriod.IsOverlapping(period))
            {
                var merged = prevPeriod.Union(period).OrThrow();
                mergedPeriods.RemoveLast();
                mergedPeriods.AddLast(merged);
            }
            else
                mergedPeriods.AddLast(period);
        }

        foreach (var period in mergedPeriods)
            yield return period;
    }

    public static IEnumerable<Period> Union(IEnumerable<Period> periods)
    {
        var united = new List<Period>();
        periods.ForEach(united.Add);
        united.Sort(new PeriodComparer());

        var i = 0;
        var last = 1;
        while (last < united.Count() - 1)
        {
            var first = united[i];
            var second = united[last];

            if (first.IsOverlapping(second.Start, second.End))
            {
                if (second.End > first.End)
                {
                    united[i] = Period.New(first.Start, second.End);
                }
                united[last] = Period.Empty;
            }
            else
            {
                i = i + 1;
            }
            last = last + 1;
        }
        // TODO: Simply cut out trailing empty periods.
        return united;
    }
}

