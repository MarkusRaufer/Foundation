namespace Foundation;

using Foundation.Collections.Generic;

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
            TimeDef.Timespan timespan => Timespan(timespan),
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
        return Day(day.Values.ToArray());
    }

    public static IPeriodGenerator Days(int days)
    {
        if (days < 0) throw new ArgumentOutOfRangeException(nameof(days));

        return new PeriodGenerator(period =>
        {
            return period.Days()
                         .CycleEnumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.Item1 % days))
                         .Select(tuple => tuple.Item2);
        });
    }

    public static IPeriodGenerator Days(TimeDef.Days days)
    {
        return Days(days.Value);
    }

    public static IPeriodGenerator Hour(params int[] hour)
    {
        return new PeriodGenerator(period => period.Hours().Where(p => hour.Contains(p.Start.Hour)));
    }

    public static IPeriodGenerator Hour(TimeDef.Hour hour)
    {
        return Hour(hour.Values.ToArray());
    }

    public static IPeriodGenerator Hours(int hours)
    {
        if (hours < 0) throw new ArgumentOutOfRangeException(nameof(hours));

        return new PeriodGenerator(period =>
        {
            return period.Hours()
                         .CycleEnumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.Item1 % hours))
                         .Select(tuple => tuple.Item2);
        });
    }

    public static IPeriodGenerator Hours(TimeDef.Hours hours)
    {
        return Hours(hours.Value);
    }

    public static IPeriodGenerator Minute(params int[] minute)
    {
        return new PeriodGenerator(period => period.Minutes().Where(p => minute.Contains(p.Start.Minute)));
    }

    public static IPeriodGenerator Minute(TimeDef.Minute minute)
    {
        return Minute(minute.Values.ToArray());
    }

    public static IPeriodGenerator Minutes(int minutes)
    {
        if (minutes < 0) throw new ArgumentOutOfRangeException(nameof(minutes));

        return new PeriodGenerator(period =>
        {
            return period.Minutes()
                         .CycleEnumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.Item1 % minutes))
                         .Select(tuple => tuple.Item2);
        });
    }

    public static IPeriodGenerator Minutes(TimeDef.Minutes minutes)
    {
        return Minutes(minutes.Value);
    }

    public static IPeriodGenerator Month(params Month[] month)
    {
        var m = month.Select(x => (int)x).ToList();
        return new PeriodGenerator(period => period.Months().Where(p => m.Contains(p.Start.Month)));
    }

    public static IPeriodGenerator Month(TimeDef.Month month)
    {
        return Month(month.Values.ToArray());
    }

    public static IPeriodGenerator Months(int months)
    {
        if (months < 0) throw new ArgumentOutOfRangeException(nameof(months));

        return new PeriodGenerator(period =>
        {
            return period.Months()
                         .CycleEnumerate(0, int.MaxValue, count => count + 1)
                         .Where(tuple => 0 == (tuple.Item1 % months))
                         .Select(tuple => tuple.Item2);
        });
    }

    public static IPeriodGenerator Months(TimeDef.Months months)
    {
        return Months(months.Value);
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
            var allPeriods = PeriodGenerator.GeneratePeriodsFromSmallestTimeDef(td, period);
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

    public static IPeriodGenerator Timespan(TimeDef.Timespan time)
    {
        var from = time.From.ToTimeSpan();
        var to = time.To.ToTimeSpan();

        return new PeriodGenerator(period => period.Hours().Where(p =>
        {
            return from >= p.Start.TimeOfDay && to < p.End.TimeOfDay;
        }));
    }

    public static IPeriodGenerator Week(DayOfWeek start, params int[] week)
    {
        return new PeriodGenerator(period =>
        {
            return period.Months()
                         .SelectMany(month =>
                         {
                             var max = month.Weeks(start).Count();
                             return month.Weeks(start)
                                     .CycleEnumerate(1, max)
                                     .Where(tuple => week.Contains(tuple.Item1))
                                     .Select(t => t.Item2);
                         });
        });
    }

    public static IPeriodGenerator Week(TimeDef.WeekOfMonth weekOfMonth)
    {
        return Week(weekOfMonth.WeekStartsWith, weekOfMonth.Values.ToArray());
    }

    public static IPeriodGenerator Weekday(params DayOfWeek[] weekday)
    {
        return new PeriodGenerator(period => period.Days().Where(d => weekday.Contains(d.Start.DayOfWeek)));
    }

    public static IPeriodGenerator Weekday(TimeDef.Weekday weekday)
    {
        return Weekday(weekday.Values.ToArray());
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
                                         .CycleEnumerate(0, int.MaxValue, count => count + 1);
                         })
                         .Where(tuple => 0 == (tuple.Item1 % weeks))
                         .Select(t => t.Item2); ;
        });
    }

    public static IPeriodGenerator Weeks(TimeDef.Weeks weeks)
    {
        return Weeks(weeks.Value, weeks.WeekStartsWith);
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
        return Year(year.Values.ToArray());
    }

    public static IPeriodGenerator Years(int years)
    {
        return new PeriodGenerator(period =>
        {
            return period.Years()
                         .CycleEnumerate(0, int.MaxValue)
                         .Where(tuple => 0 == (tuple.Item1 % years))
                         .Select(tuple => tuple.Item2);
        });
    }

    public static IPeriodGenerator Years(TimeDef.Years years)
    {
        return Years(years.Value);
    }
}
