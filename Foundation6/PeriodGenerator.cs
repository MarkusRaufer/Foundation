namespace Foundation;

public class PeriodGenerator : IPeriodGenerator
{
    private Func<Period, IEnumerable<Period>> _generator;

    public PeriodGenerator(Func<Period, IEnumerable<Period>> generator)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
    }

    public IEnumerable<Period> GeneratePeriods(Period period)
    {
        return _generator(period);
    }

    public static IEnumerable<Period> GeneratePeriodsFromSmallestTimeDef(TimeDef td, Period period)
    {
        var range = new TimeDefRange();
        range.Visit(td);

        if (range.Smallest.IsNone) return Enumerable.Empty<Period>();

        return range.Smallest.Value switch
        {
            TimeDef.Day _ => period.Days(),
            TimeDef.Days _ => period.Days(),
            TimeDef.Hour _ => period.Hours(),
            TimeDef.Hours _ => period.Hours(),
            TimeDef.Minute _ => period.Minutes(),
            TimeDef.Minutes _ => period.Minutes(),
            TimeDef.Month _ => period.Months(),
            TimeDef.Months _ => period.Months(),
            TimeDef.Timespan _ => period.Minutes(),
            TimeDef.Weekday _ => period.Days(),
            TimeDef.WeekOfMonth _ => period.Weeks(),
            TimeDef.Weeks _ => period.Weeks(),
            TimeDef.Year _ => period.Years(),
            TimeDef.Years _ => period.Years(),
            _ => Enumerable.Empty<Period>()
        };
    }
}

