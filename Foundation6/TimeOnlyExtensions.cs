namespace Foundation;

public static class TimeOnlyExtensions
{
    public static TimeOnly Subtract(this TimeOnly time, TimeSpan span) => new(time.Ticks - span.Ticks);
}

