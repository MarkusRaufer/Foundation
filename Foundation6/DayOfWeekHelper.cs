namespace Foundation;

using Foundation.Collections.Generic;
using System.Globalization;

public class DayOfWeekHelper
{

    public static IEnumerable<DayOfWeek> CycleDaysOfWeek(DayOfWeek start = DayOfWeek.Sunday)
    {
        return AllDaysOfWeek().Cycle().Skip((int)start);
    }

    /// <summary>
    /// Returns days that can be uses as first day of week.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<DayOfWeek> FirstDaysOfWeek()
    {
        yield return DayOfWeek.Sunday;
        yield return DayOfWeek.Monday;
    }

    public static IEnumerable<DayOfWeek> AllDaysOfWeek(DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
    {
        if(DayOfWeek.Sunday == firstDayOfWeek)
        {
            foreach (DayOfWeek weekDay in Enum.GetValues(typeof(DayOfWeek)))
                yield return weekDay;

            yield break;
        }

        yield return DayOfWeek.Monday;
        yield return DayOfWeek.Tuesday;
        yield return DayOfWeek.Wednesday;
        yield return DayOfWeek.Thursday;
        yield return DayOfWeek.Friday;
        yield return DayOfWeek.Saturday;
        yield return DayOfWeek.Sunday;
    }
    
    public static bool IsValidFirstDayOfWeek(DayOfWeek dayOfWeek)
    {
        return DayOfWeek.Sunday == dayOfWeek || DayOfWeek.Monday == dayOfWeek;
    }

    public static bool IsValidFirstDayOfWeek(DayOfWeek dayOfWeek, CultureInfo cultureInfo)
    {
        return cultureInfo.DateTimeFormat.FirstDayOfWeek == dayOfWeek;
    }
}

