using static Foundation.TimeDef;

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
            Timespan => 4,
            Day => 5,
            Days => 6,
            Weekday => 7,
            WeekOfMonth => 8,
            Weeks => 9,
            TimeDef.Month => 10,
            Months => 11,
            Year => 12,
            Years => 13,
            Difference => 14,
            Union => 15,
            Or => 16,
            And => 17,
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
            DateSpan td => td.To.Subtract(td.From),
            DateTimeSpan td => td.To.Subtract(td.From),
            Timespan td => td.To.Subtract(td.From),
            _ => TimeSpan.Zero
        };
    }

    public static object GetValueOfValueTimeDef(TimeDef timedef)
    {
        return timedef switch
        {
            DateSpan td => td.To.Subtract(td.From),
            DateTimeSpan td => td.To.Subtract(td.From),
            QuantityTimeDef td => td.Quantity,
            Timespan td => td.To.Subtract(td.From),
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

    public static Period ToPeriod(TimeDef.DateSpan dateSpan, DateTimeKind kind)
    {
        return Period.New(dateSpan.From, dateSpan.To, kind);
    }

    public static Period ToPeriod(TimeDef.DateTimeSpan dateTimeSpan, DateTimeKind kind)
    {
        return Period.New(dateTimeSpan.From, dateTimeSpan.To);
    }

    public static Period ToPeriod(TimeDef.Timespan timespan, DateTimeKind kind)
    {
        return Period.New(timespan.From, timespan.To, kind);
    }

    public static int ValueTimeDefSortOrderWeight(TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.DateSpan td => valueOfDateSpan(td.From, td.To),
            TimeDef.DateTimeSpan td => valueOfDateTimeSpan(td.From, td.To),
            TimeDef.Day => 4,
            TimeDef.Days => 4,
            TimeDef.Hour => 5,
            TimeDef.Hours => 5,
            TimeDef.Minute => 6,
            TimeDef.Minutes => 6,
            TimeDef.Timespan td => valueOfTimespan(td.From, td.To),
            TimeDef.Weekday => 4,
            TimeDef.WeekOfMonth => 3,
            TimeDef.Weeks => 3,
            TimeDef.Month => 2,
            TimeDef.Months => 2,
            TimeDef.Year => 1,
            TimeDef.Years => 1,
            _ => 10,
        };

        int valueOfDateSpan(DateOnly from, DateOnly to)
        {
            if (from.Year != to.Year) return 1;
            if(from.Month != to.Month) return 2;
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
    }
}
