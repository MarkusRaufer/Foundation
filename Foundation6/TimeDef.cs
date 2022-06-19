namespace Foundation;

using Foundation.ComponentModel;

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
}
