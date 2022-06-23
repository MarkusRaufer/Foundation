namespace Foundation;

using Foundation.Collections.Generic;

public abstract record TimeDef
{
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

    public static TimeDef FromDateOnly(DateOnly date)
    {
        var year = new Year(new[] { date.Year });
        var month = new Month(new[] { (Foundation.Month)date.Month });
        var day = new Day(new[] { date.Day });

        return new And(new And(year, month), day);
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

    public static TimeDef FromTimeOnly(TimeOnly time)
    {
        var hour = new Hour(new[] { time.Hour });
        var minute = new Minute(new[] { time.Minute });

        return new And(hour, minute);
    }
}
