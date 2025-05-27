namespace Foundation;

public class TimeDefDateTimeValidator : TimeDefVisitor
{
    private DateTime _dateTime;

    public bool IsValid(TimeDef td, DateTime dateTime)
    {
        _dateTime = dateTime;
        return Visit(td);
    }

    protected override bool VisitDay(TimeDef.Day td)
    {
        return td.DaysOfMonth.Contains(_dateTime.Day);
    }

    protected override bool VisitHour(TimeDef.Hour td)
    {
        return td.HoursOfDay.Contains(_dateTime.Hour);
    }

    protected override bool VisitMinute(TimeDef.Minute td)
    {
        return td.MinutesOfHour.Contains(_dateTime.Minute);
    }

    protected override bool VisitMonth(TimeDef.Month td)
    {
        Month month = (Month)_dateTime.Month;

        return td.MonthsOfYear.Contains(month);
    }

    protected override bool VisitWeekday(TimeDef.Weekday td)
    {
        return td.DaysOfWeek.Contains(_dateTime.DayOfWeek);
    }

    protected override bool VisitYear(TimeDef.Year td)
    {
        return td.YearsOfPeriod.Contains(_dateTime.Year);
    }

    protected override bool VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
        var weeks = _dateTime.WeeksOfMonth(_dateTime.Kind).ToArray();
        foreach (var week in td.Week)
        {
            if (week < 1 || week > weeks.Length) return false;

            var period = weeks[week - 1];
            if (!period.IsWithin(_dateTime)) return false;
        }
        return true; 
    }
}
