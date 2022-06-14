namespace Foundation
{
    public static class DateOnlyExtensions
    {
        public static DateTime ToDateTime(this DateOnly dateOnly)
        {
            return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
        }

        public static DateTime ToDateTime(this DateOnly dateOnly, DateTimeKind kind)
        {
            return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, 0, 0, 0, kind);
        }

        public static long ToTicks(this DateOnly dateOnly) => dateOnly.ToDateTime().Ticks;

        public static long ToTicks(this DateOnly dateOnly, DateTimeKind kind) => dateOnly.ToDateTime(kind).Ticks;
    }
}
