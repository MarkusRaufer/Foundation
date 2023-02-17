using System.Runtime.CompilerServices;

namespace Foundation
{
    public static class DateOnlyExtensions
    {
        public static DateOnly Add(this DateOnly date, TimeSpan span)
        {
            return DateOnly.FromDateTime(date.ToDateTime().Add(span));
        }

        public static TimeSpan Subtract(this DateOnly date, DateOnly subtract)
            => TimeSpan.FromDays(date.DayNumber - subtract.DayNumber);

        public static DateOnly Subtract(this DateOnly date, TimeSpan span)
            => DateOnly.FromDateTime(date.ToDateTime().Subtract(span));

        public static DateTime ToDateTime(this DateOnly date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime ToDateTime(this DateOnly date, DateTimeKind kind)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, kind);
        }

        public static long ToTicks(this DateOnly date) => date.ToDateTime().Ticks;

        public static long ToTicks(this DateOnly date, DateTimeKind kind) => date.ToDateTime(kind).Ticks;
    }
}
