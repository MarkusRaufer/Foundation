namespace Foundation;

public static class TimeOnlyExtensions
{
    public static TimeOnly Subtract(this TimeOnly time, TimeSpan span) => new(time.Ticks - span.Ticks);

    public static TimeSpan ToTimeSpan(this TimeOnly time)
    {
        return new TimeSpan(0, time.Hour, time.Minute, time.Second, time.Millisecond);
    }
}

