namespace Foundation;

using System.Text;

public static class TimeSpanExtensions
{
    public static int GetDays(this TimeSpan duration)
    {
        return (int)Math.Ceiling(duration.TotalDays);
    }

    public static string ToIso8601Period(this TimeSpan duration)
    {
        var sb = new StringBuilder();
        if (TimeSpan.Zero > duration)
            sb.Append('-');

        sb.Append('P');
        if (0 != duration.Days)
        {
            var days = Math.Abs(duration.Days);
            sb.Append(days);
            sb.Append('D');
        }

        var diff = duration - TimeSpan.FromDays(duration.Days);
        if (TimeSpan.Zero != diff)
        {
            sb.Append('T');
        }
        else
        {
            return sb.ToString();
        }

        if (0 != duration.Hours)
        {
            var hours = Math.Abs(duration.Hours);
            sb.Append(hours);
            sb.Append('H');
        }
        if (0 != duration.Minutes)
        {
            var minutes = Math.Abs(duration.Minutes);
            sb.Append(minutes);
            sb.Append('M');
        }
        if (0 != duration.Seconds)
        {
            var seconds = Math.Abs(duration.Seconds);
            sb.Append(seconds);
            sb.Append('S');
        }
        if (0 != duration.Milliseconds)
        {
            var milliseconds = Math.Abs(duration.Milliseconds);
            sb.Append(milliseconds);
            sb.Append('F');
        }

        return sb.ToString();
    }

    public static IEnumerable<string> ToIso8601PeriodParts(this TimeSpan duration)
    {
        var sb = new StringBuilder();
        if (TimeSpan.Zero > duration)
            yield return "-";

        yield return "P";
        if (0 != duration.Days)
            yield return $"{Math.Abs(duration.Days)}D";

        var diff = duration - TimeSpan.FromDays(duration.Days);
        if (TimeSpan.Zero != diff)
            yield return "T";
        else
            yield break;

        if (0 != duration.Hours)
            yield return $"{Math.Abs(duration.Hours)}H";

        if (0 != duration.Minutes)
            yield return $"{Math.Abs(duration.Minutes)}M";

        if (0 != duration.Seconds)
            yield return $"{Math.Abs(duration.Seconds)}S";

        if (0 != duration.Milliseconds)
            yield return $"{Math.Abs(duration.Milliseconds)}F";
    }
}
