using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public class TimeDefRange : TimeDefVisitor
{
    public Opt<TimeDef> Greatest { get; private set; }

    protected void SetGreatest([DisallowNull] TimeDef? td)
    {
        ArgumentNullException.ThrowIfNull(td);

        if (Greatest.IsNone || Greatest.OrThrow().Compare(td) == -1)
            Greatest = Opt.Some(td);
    }

    protected void SetSmallest(TimeDef td)
    {
        if (Smallest.IsNone || Smallest.OrThrow().Compare(td) == 1)
            Smallest = Opt.Some(td);
    }

    public Opt<TimeDef> Smallest { get; private set; }

    protected override void VisitDay(TimeDef.Day td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitDays(TimeDef.Days td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitHour(TimeDef.Hour td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitHours(TimeDef.Hours td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitMinute(TimeDef.Minute td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitMinutes(TimeDef.Minutes td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitMonth(TimeDef.Month td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitMonths(TimeDef.Months td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitTimespan(TimeDef.Timespan td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitWeekday(TimeDef.Weekday td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitWeeks(TimeDef.Weeks td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitYear(TimeDef.Year td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }

    protected override void VisitYears(TimeDef.Years td)
    {
        SetGreatest(td);
        SetSmallest(td);
    }
}

