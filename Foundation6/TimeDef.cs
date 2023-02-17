namespace Foundation;

using Foundation.Collections.Generic;
using System.Diagnostics;

public abstract record TimeDef
{
    #region time definitions
    public sealed record And(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public sealed record DateSpan(DateOnly From, DateOnly To) : TimeDef;
    public sealed record DateTimeSpan(DateTime From, DateTime To) : TimeDef;
    public sealed record Day(NonEmptyUniqueValues<int> DaysOfMonth) : TimeDef;
    public sealed record Days(int Quantity) : TimeDef;
    public sealed record Difference(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public sealed record Hour(NonEmptyUniqueValues<int> HoursOfDay) : TimeDef;
    public sealed record Hours(int Quantity) : TimeDef;
    public sealed record Minute(NonEmptyUniqueValues<int> MinutesOfHour) : TimeDef;
    public sealed record Minutes(int Quantity) : TimeDef;
    public sealed record Month(NonEmptyUniqueValues<Foundation.Month> MonthsOfYear) : TimeDef;
    public sealed record Months(int Quantity) : TimeDef;
    public sealed record Not(TimeDef TimeDef) : TimeDef;
    public sealed record Or(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public sealed record Timespan(TimeOnly From, TimeOnly To) : TimeDef;
    public sealed record Union(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public sealed record Weekday(NonEmptyUniqueValues<DayOfWeek> DaysOfWeek) : TimeDef;
    public sealed record WeekOfMonth(DayOfWeek WeekStartsWith, NonEmptyUniqueValues<int> Week) : TimeDef;
    public sealed record Weeks(int Quantity, DayOfWeek WeekStartsWith) : TimeDef;
    public sealed record Year(NonEmptyUniqueValues<int> YearsOfPeriod) : TimeDef;
    public sealed record Years(int Quantity) : TimeDef;
    #endregion time defintions

    #region factory methods

    /// <summary>
    /// Chains timeDefs by And.
    /// </summary>
    /// <param name="timeDefs"></param>
    /// <returns></returns>
    public static TimeDef ChainByAnd(params TimeDef[] timeDefs) => timeDefs.ChainByAnd();

    /// <summary>
    /// Chains timeDefs by Or.
    /// </summary>
    /// <param name="timeDefs"></param>
    /// <returns></returns>
    public static TimeDef ChainByOr(params TimeDef[] timeDefs) => timeDefs.ChainByOr();

    public static TimeDef FromDate(int year, int month, int day)
    {
        return FromDate(year, (Foundation.Month)month, day);
    }

    public static TimeDef FromDate(int year, Foundation.Month month, int day)
    {
        var dtYear = FromYear(year);
        var dtMonth = FromMonth(month);
        var dtDay = FromDay(day);

        return ChainByAnd(dtYear, dtMonth, dtDay);
    }

    public static TimeDef FromDateOnly(DateOnly date)
    {
        return FromDate(date.Year, date.Month, date.Day);
    }

    public static TimeDef FromDateTime(DateTime dateTime)
    {
        var year = new Year(new[] { dateTime.Year });
        var month = new Month(new[] { (Foundation.Month)dateTime.Month });
        var day = new Day(new[] { dateTime.Day });
        var hour = new Hour(new[] { dateTime.Hour });
        var minute = new Minute(new[] { dateTime.Minute });
        
        return ChainByAnd(year, month, day, hour, minute);
    }

    public static TimeDef FromDay(params int[] day)
    {
        if (day.Any(d => d is < 1 or > 31)) throw new ArgumentOutOfRangeException(nameof(day), "must be between [1..31]");

        return new Day(day);
    }

    public static TimeDef FromHour(params int[] hour)
    {
        if (hour.Any(h => h is < 0 or > 23)) throw new ArgumentOutOfRangeException(nameof(hour), "must be between [0..23]");

        return new Hour(hour);
    }

    public static TimeDef FromMinute(params int[] minute)
    {
        if (minute.Any(m => m is < 0 or > 59)) throw new ArgumentOutOfRangeException(nameof(minute), "must be between [0..59]");

        return new Minute(minute);
    }

    public static TimeDef FromMonth(params Foundation.Month[] month)
    {
        return new Month(month);
    }

    public static TimeDef FromTime(int hour, int minute)
    {
        var tdHour = FromHour(hour);
        var tdMinute = FromMinute(minute);

        return new And(tdHour, tdMinute);
    }

    public static TimeDef FromTimeOnly(TimeOnly time)
    {
        var hour = new Hour(new[] { time.Hour });
        var minute = new Minute(new[] { time.Minute });

        return new And(hour, minute);
    }

    public static TimeDef FromWeekday(params DayOfWeek[] dayOfWeek)
    {
        return new Weekday(dayOfWeek);
    }

    public static TimeDef FromWeekOfMonth(DayOfWeek weekStartsWith, params int[] week)
    {
        return new WeekOfMonth(weekStartsWith, week);
    }

    public static TimeDef FromYear(params int[] year)
    {
        return new Year(year);
    }

    # endregion factory methods
}
