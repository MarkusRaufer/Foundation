namespace Foundation;

using System.Globalization;

public class DateTimeHelper
{
    public static readonly string[] DateTimeStringFormats =
    { 
            // Basic formats
            "yyyyMMddTHHmmsszzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmssZ",
            // Extended formats
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssZ",
            // All of the above with reduced accuracy
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmZ",
            "yyyy-MM-ddTHH:mmzzz",
            "yyyy-MM-ddTHH:mmzz",
            "yyyy-MM-ddTHH:mmZ",
            // Accuracy reduced to hours
            "yyyyMMddTHHzzz",
            "yyyyMMddTHHzz",
            "yyyyMMddTHHZ",
            "yyyy-MM-ddTHHzzz",
            "yyyy-MM-ddTHHzz",
            "yyyy-MM-ddTHHZ",
            // Accuracy reduced to date
            "yyyyMMdd",
            "yyyy-MM-dd"
        };

    public static DateTime CreateTime(int hour, int minute, int second)
    {
        return default(DateTime).AddHours(hour).AddMinutes(minute).AddSeconds(second);
    }

    public static int GetDaysOfYear(int year)
    {
        var dec31 = new DateTime(year, 12, 31);
        return dec31.DayOfYear;
    }


    public static DateTime Max(DateTime lhs, DateTime rhs)
    {
        return (lhs > rhs) ? lhs : rhs;
    }

    public static DateTimeOffset Max(DateTimeOffset lhs, DateTimeOffset rhs)
    {
        return (lhs > rhs) ? lhs : rhs;
    }

    public static DateTime Min(DateTime lhs, DateTime rhs)
    {
        return (lhs < rhs) ? lhs : rhs;
    }

    public static DateTimeOffset Min(DateTimeOffset lhs, DateTimeOffset rhs)
    {
        return (lhs < rhs) ? lhs : rhs;
    }

    /// <summary>
    /// Returns the current time dependend on kind.
    /// </summary>
    /// <param name="kind"></param>
    /// <returns>If kind is <see cref="DateTimeKind.Unspecified"/> it returns <see cref="DateTime.UtcNow">.</returns>
    public static DateTime Now(DateTimeKind kind)
    {
        return kind switch
        {
            DateTimeKind.Local => DateTime.Now,
            _ => DateTime.UtcNow
        };
    }

    public static bool TryParseFromIso8601(string str, out DateTime dt)
    {
        return DateTime.TryParseExact(
            str,
            DateTimeStringFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out dt);
    }

}

