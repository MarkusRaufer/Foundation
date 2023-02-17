using Foundation.Collections.Generic;
using Microsoft.VisualBasic;

namespace Foundation;

public static class TimeDefExtensions
{
    public static IEnumerable<TimeDef> BothSides(this TimeDef.And and)
    {
        yield return and.Lhs;
        yield return and.Rhs;
    }

    public static IEnumerable<TimeDef> BothSides(this TimeDef.Difference difference)
    {
        yield return difference.Lhs;
        yield return difference.Rhs;
    }

    public static IEnumerable<TimeDef> BothSides(this TimeDef.Or or)
    {
        yield return or.Lhs;
        yield return or.Rhs;
    }

    public static IEnumerable<TimeDef> BothSides(this TimeDef.Union union)
    {
        yield return union.Lhs;
        yield return union.Rhs;
    }

    public static TimeDef Chain(this IEnumerable<TimeDef> timeDefs, Func<TimeDef, TimeDef, TimeDef> binaryOperationFactory)
    {
        binaryOperationFactory.ThrowIfNull();

        var it = timeDefs.GetEnumerator();
        if (!it.MoveNext()) throw new ArgumentOutOfRangeException(nameof(timeDefs), "must not be empty");

        var lhs = it.Current;
        
        while (it.MoveNext())
        {
            lhs = binaryOperationFactory(lhs, it.Current);
        }

        return lhs;
    }

    public static TimeDef ChainByAnd(this IEnumerable<TimeDef> timeDefs)
    {
        return Chain(timeDefs, (l, r) => new TimeDef.And(l, r));
    }

    public static TimeDef ChainByOr(this IEnumerable<TimeDef> timeDefs)
    {
        return Chain(timeDefs, (l, r) => new TimeDef.Or(l, r));
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
            TimeDef.Day l => rhs is TimeDef.Day r && equals(l.DaysOfMonth, r.DaysOfMonth),
            TimeDef.Days l => rhs is TimeDef.Days r && l.Quantity == r.Quantity,
            TimeDef.Difference l => rhs is TimeDef.Difference r
                                    && Equals(l.Lhs, r.Lhs)
                                    && Equals(l.Rhs, r.Rhs),
            TimeDef.Hour l => rhs is TimeDef.Hour r && equals(l.HoursOfDay, r.HoursOfDay),
            TimeDef.Hours l => rhs is TimeDef.Hours r && l.Quantity == r.Quantity,
            TimeDef.Minute l => rhs is TimeDef.Minute r && equals(l.MinutesOfHour, r.MinutesOfHour),
            TimeDef.Minutes l => rhs is TimeDef.Minutes r && l.Quantity == r.Quantity,
            TimeDef.Month l => rhs is TimeDef.Month r && equals(l.MonthsOfYear, r.MonthsOfYear),
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
            TimeDef.Weekday l => rhs is TimeDef.Weekday r && equals(l.DaysOfWeek, r.DaysOfWeek),
            TimeDef.WeekOfMonth l => rhs is TimeDef.WeekOfMonth r
                                     && equals(l.Week, r.Week)
                                     && l.WeekStartsWith == r.WeekStartsWith,
            TimeDef.Weeks l => rhs is TimeDef.Weeks r
                               && l.Quantity == r.Quantity
                               && l.WeekStartsWith == r.WeekStartsWith,
            TimeDef.Year l => rhs is TimeDef.Year r && equals(l.YearsOfPeriod, r.YearsOfPeriod),
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

