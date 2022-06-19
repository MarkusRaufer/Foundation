namespace Foundation;

public static class TimeDefHelper
{
    public static int ChronologicalOrderWeight(TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.Minute or TimeDef.Minutes or TimeDef.Timespan => 0,
            TimeDef.Hour or TimeDef.Hours => 1,
            TimeDef.Day or TimeDef.Days => 2,
            TimeDef.Weekday or TimeDef.WeekOfMonth or TimeDef.Weeks => 3,
            TimeDef.Month or TimeDef.Months => 4,
            TimeDef.Year or TimeDef.Years => 5,
            _ => 6
        };
    }

    public static int Compare(TimeDef lhs, TimeDef rhs)
    {
        var lhsWeight = ValueTimeDefSortOrderWeight(lhs);
        var rhsWeight = ValueTimeDefSortOrderWeight(rhs);

        return lhsWeight.CompareTo(rhsWeight);
    }

    public static bool IsSpanTimeDef(TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.DateSpan or
            TimeDef.DateTimeSpan or
            TimeDef.Timespan => true,
            _ => false
        };
    }

    public static bool IsValueTimeDef(TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.DateSpan or
            TimeDef.DateTimeSpan or
            TimeDef.Day or
            TimeDef.Days or
            TimeDef.Hour or
            TimeDef.Hours or
            TimeDef.Minute or
            TimeDef.Minutes or
            TimeDef.Timespan or
            TimeDef.Weekday or
            TimeDef.WeekOfMonth or
            TimeDef.Weeks or
            TimeDef.Month or
            TimeDef.Months or
            TimeDef.Year or
            TimeDef.Years => true,
            _ => false
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
