namespace Foundation;

public static class TimeDefExtensions
{
    public static bool Equals(this TimeDef lhs, TimeDef rhs)
    {
        static bool equals<T>(IEnumerable<T> lhs, IEnumerable<T> rhs)
        {
            return lhs.OrderBy(x => x).SequenceEqual(rhs.OrderBy(x => x));
        }

        return lhs switch
        {
            TimeDef.And l => rhs is TimeDef.And r
                             && Equals(l.Lhs, r.Lhs)
                             && Equals(l.Rhs, r.Rhs),
            TimeDef.DateTimeSpan l => rhs is TimeDef.DateTimeSpan r
                                      && Equals(l.From, r.From)
                                      && Equals(l.To, r.To),
            TimeDef.Day l => rhs is TimeDef.Day r && equals(l.DayOfMonth, r.DayOfMonth),
            TimeDef.Days l => rhs is TimeDef.Days r && l.Quantity == r.Quantity,
            TimeDef.Difference l => rhs is TimeDef.Difference r
                                    && Equals(l.Lhs, r.Lhs)
                                    && Equals(l.Rhs, r.Rhs),
            TimeDef.Hour l => rhs is TimeDef.Hour r && equals(l.HourOfDay, r.HourOfDay),
            TimeDef.Hours l => rhs is TimeDef.Hours r && l.Quantity == r.Quantity,
            TimeDef.Minute l => rhs is TimeDef.Minute r && equals(l.MinuteOfHour, r.MinuteOfHour),
            TimeDef.Minutes l => rhs is TimeDef.Minutes r && l.Quantity == r.Quantity,
            TimeDef.Month l => rhs is TimeDef.Month r && equals(l.MonthOfYear, r.MonthOfYear),
            TimeDef.Months l => rhs is TimeDef.Months r && l.Quantity == r.Quantity,
            TimeDef.Not l => rhs is TimeDef.Not r && Equals(l.TimeDef, r.TimeDef),
            TimeDef.Or l => rhs is TimeDef.Or r
                            && Equals(l.Lhs, r.Lhs)
                            && Equals(l.Rhs, r.Rhs),
            TimeDef.Timespan l => rhs is TimeDef.Timespan r
                                  && Equals(l.From, r.From)
                                  && Equals(l.To, r.To),
            TimeDef.Union l => rhs is TimeDef.Union r
                               && Equals(l.Lhs, r.Lhs)
                               && Equals(l.Rhs, r.Rhs),
            TimeDef.Weekday l => rhs is TimeDef.Weekday r && equals(l.DayOfWeek, r.DayOfWeek),
            TimeDef.WeekOfMonth l => rhs is TimeDef.WeekOfMonth r
                                     && equals(l.Week, r.Week)
                                     && l.WeekStartsWith == r.WeekStartsWith,
            TimeDef.Weeks l => rhs is TimeDef.Weeks r
                               && l.Quantity == r.Quantity
                               && l.WeekStartsWith == r.WeekStartsWith,
            TimeDef.Year l => rhs is TimeDef.Year r && equals(l.YearOfDate, r.YearOfDate),
            TimeDef.Years l => rhs is TimeDef.Years r && l.Quantity == r.Quantity,
            _ => throw new NotImplementedException($"{lhs}")
        };
    }

    public static int Compare(this TimeDef lhs, TimeDef rhs)
    {
        var leftWeight = TimeDefHelper.ChronologicalOrderWeight(lhs);
        var rightWeight = TimeDefHelper.ChronologicalOrderWeight(rhs);
        if (leftWeight < rightWeight) return -1;
        if (leftWeight > rightWeight) return 1;
        return 0;
    }
}

