namespace Foundation;

using Foundation.Collections.Generic;

public static class DayOfWeekExtensions
{
    public static DayOfWeek GetWeekDay(this DayOfWeek dayOfWeek, int days)
    {
        if(0 > days) throw new ArgumentOutOfRangeException(nameof(days));

        var i = 0;

        foreach (DayOfWeek d in DayOfWeekHelper.AllDaysOfWeek().Cycle())
        {
            i++;
            if (i == days) return d;
        }

        throw new NotFiniteNumberException();
    }

    public static DayOfWeek LastDayOfWeek(this DayOfWeek start)
    {
        if (DayOfWeek.Monday == start)
            return DayOfWeek.Sunday;

        if (DayOfWeek.Sunday == start)
            return DayOfWeek.Saturday;

        throw new ArgumentException($"start can be {DayOfWeek.Sunday} or {DayOfWeek.Monday}");
    }
}

