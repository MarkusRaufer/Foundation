namespace Foundation;

public static class TimeZoneInfoExt
{
    public static TimeSpan GetUtcOffset(this TimeZoneInfo timeZoneInfo)
    {
        timeZoneInfo.ThrowIfNull(nameof(timeZoneInfo));
        var adjustmentRule = timeZoneInfo.GetAdjustmentRules().FirstOrDefault();
        return null != adjustmentRule
            ? adjustmentRule.DaylightDelta + timeZoneInfo.BaseUtcOffset
            : timeZoneInfo.BaseUtcOffset;
    }
}

