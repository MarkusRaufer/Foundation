namespace Foundation;

public static class TimeSpanHelper
{
    /// <summary>
    /// creates a TimeSpan object from a ISO8601 period string.
    /// 
    /// example: 2DT3H4M5S
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static TimeSpan ParseIso8601Duration(string str)
    {
        var separators = new[] { '-', 'P', 'D', 'd', 'T', 'H', 'h', 'M', 'm', 'S', 's', 'F', 'f' };
        var tuples = str.SplitSeq(separators).ToArray();
        var timeSpan = new TimeSpan();
        var value = 0D;
        var subtract = false;
        foreach (var tuple in tuples)
        {
            switch (tuple.Key)
            {
                case '-':
                    subtract = true;
                    break;
                case 'P':
                    if (!string.IsNullOrEmpty(tuple.Value))
                        value = double.Parse(tuple.Value);

                    break;
                case 'D':
                case 'd':
                    timeSpan = subtract
                        ? timeSpan.Subtract(TimeSpan.FromDays(value))
                        : timeSpan.Add(TimeSpan.FromDays(value));

                    break;
                case 'T':
                    value = double.Parse(tuple.Value);
                    break;
                case 'H':
                case 'h':
                    timeSpan = subtract
                        ? timeSpan.Subtract(TimeSpan.FromHours(value))
                        : timeSpan.Add(TimeSpan.FromHours(value));

                    if (!string.IsNullOrEmpty(tuple.Value))
                        value = double.Parse(tuple.Value);

                    break;
                case 'M':
                case 'm':
                    timeSpan = subtract
                        ? timeSpan.Subtract(TimeSpan.FromMinutes(value))
                        : timeSpan.Add(TimeSpan.FromMinutes(value));

                    if (!string.IsNullOrEmpty(tuple.Value))
                        value = double.Parse(tuple.Value);

                    break;
                case 'S':
                case 's':
                    timeSpan = subtract
                        ? timeSpan.Subtract(TimeSpan.FromSeconds(value))
                        : timeSpan.Add(TimeSpan.FromSeconds(value));

                    if (!string.IsNullOrEmpty(tuple.Value))
                        value = double.Parse(tuple.Value);

                    break;
                case 'F':
                case 'f':
                    timeSpan = subtract
                        ? timeSpan.Subtract(TimeSpan.FromMilliseconds(value))
                        : timeSpan.Add(TimeSpan.FromMilliseconds(value));

                    break;
            }
        }
        return timeSpan;
    }
}

