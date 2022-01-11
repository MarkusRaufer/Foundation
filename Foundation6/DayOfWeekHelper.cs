namespace Foundation;

using System.Globalization;

public class DayOfWeekHelper
{
    public static IEnumerable<DayOfWeek> GetFirstDaysOfWeek()
    {
        yield return DayOfWeek.Sunday;
        yield return DayOfWeek.Monday;
    }

    public static IEnumerable<DayOfWeek> GetAllDaysOfWeek()
    {
        foreach (DayOfWeek d in Enum.GetValues(typeof(DayOfWeek)))
            yield return d;
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

