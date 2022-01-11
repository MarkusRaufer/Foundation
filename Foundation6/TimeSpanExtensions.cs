namespace Foundation;

using System.Text;

public static class TimeSpanExtensions
{
    public static int GetDays(this TimeSpan duration)
    {
        return (int)Math.Ceiling(duration.TotalDays);
    }

    public static string ToIso8601PeriodString(this TimeSpan duration)
    {
        var sb = new StringBuilder();
        if (TimeSpan.Zero > duration)
            sb.Append('-');

        sb.Append('P');
        if (0 < duration.Days)
        {
            sb.Append(duration.Days);
            sb.Append('D');
        }

        var diff = duration - TimeSpan.FromDays(duration.Days);
        if (TimeSpan.Zero < diff)
        {
            sb.Append('T');
        }
        else
        {
            return sb.ToString();
        }


        if (0 < duration.Hours)
        {
            sb.Append(duration.Hours);
            sb.Append('H');
        }
        if (0 < duration.Minutes)
        {
            sb.Append(duration.Minutes);
            sb.Append('M');
        }
        if (0 < duration.Seconds)
        {
            sb.Append(duration.Seconds);
            sb.Append('S');
        }
        if (0 < duration.Milliseconds)
        {
            sb.Append(duration.Milliseconds);
            sb.Append('F');
        }

        return sb.ToString();
    }

    public static IEnumerable<string> ToIso8601PeriodStringParts(this TimeSpan duration)
    {
        var sb = new StringBuilder();
        if (TimeSpan.Zero > duration)
            yield return "-";

        yield return "P";
        if (0 < duration.Days)
            yield return $"{duration.Days}D";

        var diff = duration - TimeSpan.FromDays(duration.Days);
        if (TimeSpan.Zero < diff)
            yield return "T";
        else
            yield break;

        if (0 < duration.Hours)
            yield return $"{duration.Hours}H";

        if (0 < duration.Minutes)
            yield return $"{duration.Minutes}M";

        if (0 < duration.Seconds)
            yield return $"{duration.Seconds}S";

        if (0 < duration.Milliseconds)
            yield return $"{duration.Milliseconds}F";
    }
}
