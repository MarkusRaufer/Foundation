namespace Foundation;

using Foundation.Collections.Generic;

public abstract record TimeDef
{
    #region time definitions
    public record And(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public record DateSpan(DateOnly From, DateOnly To) : TimeDef;
    public record DateTimeSpan(DateTime From, DateTime To) : TimeDef;
    public record Day(EquatableArray<int> DayOfMonth) : TimeDef;
    public record Days(int Quantity) : TimeDef;
    public record Difference(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public record Hour(EquatableArray<int> HourOfDay) : TimeDef;
    public record Hours(int Quantity) : TimeDef;
    public record Minute(EquatableArray<int> MinuteOfHour) : TimeDef;
    public record Minutes(int Quantity) : TimeDef;
    public record Month(EquatableArray<Foundation.Month> MonthOfYear) : TimeDef;
    public record Months(int Quantity) : TimeDef;
    public record Not(TimeDef TimeDef) : TimeDef;
    public record Or(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public record Timespan(TimeOnly From, TimeOnly To) : TimeDef;
    public record Union(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public record Weekday(EquatableArray<DayOfWeek> DayOfWeek) : TimeDef;
    public record WeekOfMonth(DayOfWeek WeekStartsWith, EquatableArray<int> Week) : TimeDef;
    public record Weeks(int Quantity, DayOfWeek WeekStartsWith) : TimeDef;
    public record Year(EquatableArray<int> YearOfDate) : TimeDef;
    public record Years(int Quantity) : TimeDef;
    #endregion time defintions

    #region factory methods

    public static TimeDef FromDate(int year, int month, int day)
    {
        return FromDate(year, (Foundation.Month)month, day);
    }

    public static TimeDef FromDate(int year, Foundation.Month month, int day)
    {
        var dtYear = FromYear(year);
        var dtMonth = FromMonth(month);
        var dtDay = FromDay(day);

        return new And(new And(dtYear, dtMonth), dtDay);
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

        return new And(new And(new And(new And(year, month), day), hour), minute);
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
