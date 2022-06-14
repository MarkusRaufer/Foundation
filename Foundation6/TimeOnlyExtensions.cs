namespace Foundation;

public static class TimeOnlyExtensions
{
    public static TimeOnly Subtract(this TimeOnly time, TimeSpan span) => new(time.Ticks - span.Ticks);

    public static DateTime ToDateTime(this TimeOnly time)
    {
        return new DateTime(0, 1, 1, time.Hour, time.Minute, time.Second, time.Millisecond);
    }

    public static DateTime ToDateTime(this TimeOnly time, DateTimeKind kind)
    {
        return new DateTime(0, 1, 1, time.Hour, time.Minute, time.Second, time.Millisecond, kind);
    }
}

