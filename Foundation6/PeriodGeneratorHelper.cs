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
using System.Linq;

public class PeriodGeneratorHelper
{
    public static IPeriodGenerator And(IPeriodGenerator lhs, IPeriodGenerator rhs)
    {
        return new PeriodGenerator(period =>
        {
            var lhsGenerated = lhs.GeneratePeriods(period);
            var rhsGenerated = rhs.GeneratePeriods(period);
            return PeriodHelper.IntersectGroup(lhsGenerated, rhsGenerated);
        });
    }

    public static IPeriodGenerator And(TimeDef.And and)
    {
        return And(CreateGenerator(and.Lhs), CreateGenerator(and.Rhs));
    }

    public static IPeriodGenerator CreateGenerator(TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.And and => And(and),
            TimeDef.Day day => Day(day),
            TimeDef.Days days => Days(days),
            TimeDef.Hour hour => Hour(hour),
            TimeDef.Hours hours => Hours(hours),
            TimeDef.Minute minute => Minute(minute),
            TimeDef.Minutes minutes => Minutes(minutes),
            TimeDef.Month month => Month(month),
            TimeDef.Months months => Months(months),
            TimeDef.Not @not => Not(@not),
            TimeDef.Or @or => Or(@or),
#if NET6_0_OR_GREATER
            TimeDef.Timespan timespan => Timespan(timespan),
#endif
            TimeDef.Weekday weekday => Weekday(weekday),
            TimeDef.WeekOfMonth weekOfMonth => Week(weekOfMonth),
            TimeDef.Weeks weeks => Weeks(weeks),
            TimeDef.Year year => Year(year),
            TimeDef.Years years => Years(years),
            _ => Timespan(TimeSpan.Zero)
        };
    }

    public static IPeriodGenerator Day(params int[] day)
    {
        return new PeriodGenerator(period => period.Days().Where(p => day.Contains(p.Start.Day)));
    }

    public static IPeriodGenerator Day(TimeDef.Day day)
    {
        return Day(day.DaysOfMonth.ToArray());
    }

    public static IPeriodGenerator Days(int days)
    {
        if (days < 0) throw new ArgumentOutOfRangeException(nameof(days));

        var count = 0;

        return new PeriodGenerator(period =>
        {
            return period.Days()
                         .Cycle()
                         .Enumerate(0, int.MaxValue, x => count + 1)
                         .Where(tuple => 0 == (tuple.counter % days))
                         .Select(tuple => tuple.item);
        });
    }

    public static IPeriodGenerator Days(TimeDef.Days days)
    {
        return Days(days.Quantity);
    }

    public static IPeriodGenerator Hour(params int[] hour)
    {
        return new PeriodGenerator(period => period.Hours().Where(p => hour.Contains(p.Start.Hour)));
    }

    public static IPeriodGenerator Hour(TimeDef.Hour hour)
    {
        return Hour(hour.HoursOfDay.ToArray());
    }

    public static IPeriodGenerator Hours(int hours)
    {
        if (hours < 0) throw new ArgumentOutOfRangeException(nameof(hours));

        return new PeriodGenerator(period =>
        {
            return period.Hours()
                         .Cycle()
                         .Enumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.counter % hours))
                         .Select(tuple => tuple.item);
        });
    }

    public static IPeriodGenerator Hours(TimeDef.Hours hours)
    {
        return Hours(hours.Quantity);
    }

    public static IPeriodGenerator Minute(params int[] minute)
    {
        return new PeriodGenerator(period => period.Minutes().Where(p => minute.Contains(p.Start.Minute)));
    }

    public static IPeriodGenerator Minute(TimeDef.Minute minute)
    {
        return Minute(minute.MinutesOfHour.ToArray());
    }

    public static IPeriodGenerator Minutes(int minutes)
    {
        if (minutes < 0) throw new ArgumentOutOfRangeException(nameof(minutes));

        return new PeriodGenerator(period =>
        {
            return period.Minutes()
                         .Cycle()
                         .Enumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.counter % minutes))
                         .Select(tuple => tuple.item);
        });
    }

    public static IPeriodGenerator Minutes(TimeDef.Minutes minutes)
    {
        return Minutes(minutes.Quantity);
    }

    public static IPeriodGenerator Month(params Month[] month)
    {
        var m = month.Select(x => (int)x).ToList();
        return new PeriodGenerator(period => period.Months().Where(p => m.Contains(p.Start.Month)));
    }

    public static IPeriodGenerator Month(TimeDef.Month month)
    {
        return Month(month.MonthsOfYear.ToArray());
    }

    public static IPeriodGenerator Months(int months)
    {
        if (months < 0) throw new ArgumentOutOfRangeException(nameof(months));

        return new PeriodGenerator(period =>
        {
            return period.Months()
                         .Cycle()
                         .Enumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.counter % months))
                         .Select(tuple => tuple.item);
        });
    }

    public static IPeriodGenerator Months(TimeDef.Months months)
    {
        return Months(months.Quantity);
    }

    public static IPeriodGenerator Not(IPeriodGenerator all, IPeriodGenerator not)
    {
        return new PeriodGenerator(period =>
        {
            var allPeriods = all.GeneratePeriods(period);
            var notPeriods = not.GeneratePeriods(period);
            return allPeriods.Except(notPeriods);
        });
    }

    public static IPeriodGenerator Not(TimeDef td)
    {
        return new PeriodGenerator(period =>
        {
            var allPeriods = PeriodGenerator.GeneratePeriods(td, period);
            var notPeriods = CreateGenerator(td).GeneratePeriods(period);
            return allPeriods.Except(notPeriods);
        });
    }

    public static IPeriodGenerator Or(IPeriodGenerator lhs, IPeriodGenerator rhs)
    {
        return new PeriodGenerator(period =>
        {
            var lhsGenerated = lhs.GeneratePeriods(period);
            var rhsGenerated = rhs.GeneratePeriods(period);
            return PeriodHelper.Union(lhsGenerated.Concat(rhsGenerated));
        });
    }

    public static IPeriodGenerator Or(TimeDef.Or or)
    {
        return Or(CreateGenerator(or.Lhs), CreateGenerator(or.Rhs));
    }

    public static IPeriodGenerator Timespan(DateTime dateTime)
    {
        return Timespan(dateTime.TimeOfDay);
    }

    public static IPeriodGenerator Timespan(TimeSpan time)
    {
        return new PeriodGenerator(period => period.Hours().Where(p =>
        {
            return time >= p.Start.TimeOfDay && time < p.End.TimeOfDay;
        }));
    }


#if NET6_0_OR_GREATER
    public static IPeriodGenerator Timespan(TimeDef.Timespan time)
    {
        var from = time.From.ToTimeSpan();
        var to = time.To.ToTimeSpan();

        return new PeriodGenerator(period => period.Hours().Where(p =>
        {
            return from >= p.Start.TimeOfDay && to < p.End.TimeOfDay;
        }));
    }
#endif

    public static IPeriodGenerator Week(DayOfWeek start, params int[] week)
    {
        return new PeriodGenerator(period =>
        {
            return period.Months()
                         .SelectMany(month =>
                         {
                             var max = month.Weeks(start).Count();
                             return month.Weeks(start)
                                     .Cycle()
                                     .EnumerateRange(1, max)
                                     .Where(tuple => week.Contains(tuple.counter))
                                     .Select(tuple => tuple.item);
                         });
        });
    }

    public static IPeriodGenerator Week(TimeDef.WeekOfMonth weekOfMonth)
    {
        return Week(weekOfMonth.WeekStartsWith, weekOfMonth.Week.ToArray());
    }

    public static IPeriodGenerator Weekday(params DayOfWeek[] weekday)
    {
        return new PeriodGenerator(period => period.Days().Where(d => weekday.Contains(d.Start.DayOfWeek)));
    }

    public static IPeriodGenerator Weekday(TimeDef.Weekday weekday)
    {
        return Weekday(weekday.DaysOfWeek.ToArray());
    }

    public static IPeriodGenerator Weeks(int weeks, DayOfWeek start)
    {
        if (weeks < 0) throw new ArgumentOutOfRangeException(nameof(weeks));
        return new PeriodGenerator(period =>
        {
            return period.Months()
                         .SelectMany(month =>
                         {
                             return month.Weeks(start)
                                         .Cycle()
                                         .Enumerate(0, int.MaxValue, count => count + 1);
                         })
                         .Where(tuple => 0 == (tuple.counter % weeks))
                         .Select(t => t.item);
        });
    }

    public static IPeriodGenerator Weeks(TimeDef.Weeks weeks)
    {
        return Weeks(weeks.Quantity, weeks.WeekStartsWith);
    }

    public static IPeriodGenerator Year(params int[] year)
    {
        return new PeriodGenerator(period =>
        {
            return period.Years().Where(p => year.Contains(p.Start.Year));
        });
    }

    public static IPeriodGenerator Year(TimeDef.Year year)
    {
        return Year(year.YearsOfPeriod.ToArray());
    }

    public static IPeriodGenerator Years(int years)
    {
        return new PeriodGenerator(period =>
        {
            return period.Years()
                         .Cycle()
                         .EnumerateRange(0, int.MaxValue)
                         .Where(tuple => 0 == (tuple.counter % years))
                         .Select(tuple => tuple.item);
        });
    }

    public static IPeriodGenerator Years(TimeDef.Years years)
    {
        return Years(years.Quantity);
    }
}
