namespace Foundation;

public static class TimeDefExtensions
{
    public static int ChronologicalOrderWeight(this TimeDef timedef)
    {
        return timedef switch
        {
            TimeDef.Minute or TimeDef.Minutes or TimeDef.Timespan => 0,
            TimeDef.Hour or TimeDef.Hours => 1,
            TimeDef.Day or TimeDef.Days => 2,
            TimeDef.Weekday or TimeDef.WeekOfMonth or TimeDef.Weeks => 3,
            TimeDef.Month or TimeDef.Months => 4,
            TimeDef.Year or TimeDef.Years => 5,
            _ => 6
        };
    }

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
            TimeDef.Day l => rhs is TimeDef.Day r && equals(l.Values, r.Values),
            TimeDef.Days l => rhs is TimeDef.Days r && l.Value == r.Value,
            TimeDef.Difference l => rhs is TimeDef.Difference r
                                    && Equals(l.Lhs, r.Lhs)
                                    && Equals(l.Rhs, r.Rhs),
            TimeDef.Hour l => rhs is TimeDef.Hour r && equals(l.Values, r.Values),
            TimeDef.Hours l => rhs is TimeDef.Hours r && l.Value == r.Value,
            TimeDef.Minute l => rhs is TimeDef.Minute r && equals(l.Values, r.Values),
            TimeDef.Minutes l => rhs is TimeDef.Minutes r && l.Value == r.Value,
            TimeDef.Month l => rhs is TimeDef.Month r && equals(l.Values, r.Values),
            TimeDef.Months l => rhs is TimeDef.Months r && l.Value == r.Value,
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
            TimeDef.Weekday l => rhs is TimeDef.Weekday r && equals(l.Values, r.Values),
            TimeDef.WeekOfMonth l => rhs is TimeDef.WeekOfMonth r
                                     && equals(l.Values, r.Values)
                                     && l.WeekStartsWith == r.WeekStartsWith,
            TimeDef.Weeks l => rhs is TimeDef.Weeks r
                               && l.Value == r.Value
                               && l.WeekStartsWith == r.WeekStartsWith,
            TimeDef.Year l => rhs is TimeDef.Year r && equals(l.Values, r.Values),
            TimeDef.Years l => rhs is TimeDef.Years r && l.Value == r.Value,
            _ => throw new NotImplementedException($"{lhs}")
        };
    }

    public static int Compare(this TimeDef lhs, TimeDef rhs)
    {
        var leftWeight = ChronologicalOrderWeight(lhs);
        var rightWeight = ChronologicalOrderWeight(rhs);
        if (leftWeight < rightWeight) return -1;
        if (leftWeight > rightWeight) return 1;
        return 0;
    }
}

