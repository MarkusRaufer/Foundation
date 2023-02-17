namespace Foundation;

public static class TimeOnlyExtensions
{
    public static TimeSpan Subtract(this TimeOnly time, TimeOnly subtract) => TimeSpan.FromTicks(time.Ticks - subtract.Ticks);
    
    public static TimeOnly Subtract(this TimeOnly time, TimeSpan span) => new(time.Ticks - span.Ticks);

    public static DateTime ToDateTime(this TimeOnly time)
    {
        var year = DateOnly.MinValue.Year;
        return new DateTime(year, 1, 1, time.Hour, time.Minute, time.Second, time.Millisecond);
    }

    public static DateTime ToDateTime(this TimeOnly time, DateTimeKind kind)
    {
        var year = DateOnly.MinValue.Year;
        return new DateTime(year, 1, 1, time.Hour, time.Minute, time.Second, time.Millisecond, kind);
    }
}

