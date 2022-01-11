namespace Foundation;

using Foundation.Collections.Generic;

public static class DayOfWeekExtensions
{
    public static DayOfWeek GetWeekDay(this DayOfWeek dayOfWeek, int days)
    {
        var weekDays = DayOfWeekHelper.GetAllDaysOfWeek().ToList();
        var it = RingEnumerable.Create(weekDays, true);
        var i = 0;
        var dayFound = false;
        foreach (DayOfWeek d in it)
        {
            if (d == dayOfWeek) dayFound = true;
            if (!dayFound) continue;

            if (++i == days) return d;
        }

        throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
    }

    public static DayOfWeek LastDayOfWeek(this DayOfWeek start)
    {
        if (DayOfWeek.Monday == start)
            return DayOfWeek.Sunday;

        if (DayOfWeek.Sunday == start)
            return DayOfWeek.Saturday;

        throw new ArgumentOutOfRangeException("begin can be sunday or monday");
    }
}

