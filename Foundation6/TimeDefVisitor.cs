namespace Foundation;

public class TimeDefVisitor
{
    public virtual void Visit(TimeDef timedef)
    {
        switch (timedef)
        {
            case TimeDef.And td: VisitAnd(td); break;
            case TimeDef.DateTimeSpan td: VisitDateTimeSpan(td); break;
            case TimeDef.Day td: VisitDay(td); break;
            case TimeDef.Days td: VisitDays(td); break;
            case TimeDef.Difference td: VisitDifference(td); break;
            case TimeDef.Hour td: VisitHour(td); break;
            case TimeDef.Hours td: VisitHours(td); break;
            case TimeDef.Minute td: VisitMinute(td); break;
            case TimeDef.Minutes td: VisitMinutes(td); break;
            case TimeDef.Month td: VisitMonth(td); break;
            case TimeDef.Months td: VisitMonths(td); break;
            case TimeDef.Not td: VisitNot(td); break;
            case TimeDef.Or td: VisitOr(td); break;
            case TimeDef.Timespan td: VisitTimespan(td); break;
            case TimeDef.Union td: VisitUnion(td); break;
            case TimeDef.Weekday td: VisitWeekday(td); break;
            case TimeDef.WeekOfMonth td: VisitWeekOfMonth(td); break;
            case TimeDef.Weeks td: VisitWeeks(td); break;
            case TimeDef.Year td: VisitYear(td); break;
            case TimeDef.Years td: VisitYears(td); break;
        }
    }

    protected virtual void VisitAnd(TimeDef.And td)
    {
        Visit(td.Lhs);
        Visit(td.Rhs);
    }

    protected virtual void VisitDateTimeSpan(TimeDef.DateTimeSpan td)
    {
    }

    protected virtual void VisitDay(TimeDef.Day td)
    {
    }

    protected virtual void VisitDays(TimeDef.Days td)
    {
    }

    protected virtual void VisitDifference(TimeDef.Difference td)
    {
        Visit(td.Lhs);
        Visit(td.Rhs);
    }

    protected virtual void VisitHour(TimeDef.Hour td)
    {
    }

    protected virtual void VisitHours(TimeDef.Hours td)
    {
    }

    protected virtual void VisitMinute(TimeDef.Minute td)
    {
    }

    protected virtual void VisitMinutes(TimeDef.Minutes td)
    {
    }

    protected virtual void VisitMonth(TimeDef.Month td)
    {
    }

    protected virtual void VisitMonths(TimeDef.Months td)
    {
    }

    protected virtual void VisitNot(TimeDef.Not td)
    {
        Visit(td.TimeDef);
    }

    protected virtual void VisitOr(TimeDef.Or td)
    {
        Visit(td.Lhs);
        Visit(td.Rhs);
    }

    protected virtual void VisitTimespan(TimeDef.Timespan td)
    {
    }

    protected virtual void VisitUnion(TimeDef.Union td)
    {
        Visit(td.Lhs);
        Visit(td.Rhs);
    }

    protected virtual void VisitWeekday(TimeDef.Weekday td)
    {
    }

    protected virtual void VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
    }

    protected virtual void VisitWeeks(TimeDef.Weeks td)
    {
    }

    protected virtual void VisitYear(TimeDef.Year td)
    {
    }

    protected virtual void VisitYears(TimeDef.Years td)
    {
    }
}

