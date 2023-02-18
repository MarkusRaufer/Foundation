using static Foundation.TimeDef;

namespace Foundation;

public class PeriodGenerator : IPeriodGenerator
{
    private readonly Func<Period, IEnumerable<Period>> _generator;

    public PeriodGenerator(Func<Period, IEnumerable<Period>> generator)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
    }

    public IEnumerable<Period> GeneratePeriods(Period period)
    {
        return _generator(period);
    }

    public static IEnumerable<Period> GeneratePeriods(TimeDef td, Period period)
    {
        var range = new TimeDefRange();
        range.Visit(td);

        if (range.Smallest.IsNone) return Enumerable.Empty<Period>();

        return range.Smallest.OrThrow() switch
        {
            Day _ => period.Days(),
            Days _ => period.Days(),
            Hour _ => period.Hours(),
            Hours _ => period.Hours(),
            Minute _ => period.Minutes(),
            Minutes _ => period.Minutes(),
            TimeDef.Month _ => period.Months(),
            Months _ => period.Months(),
            Timespan _ => period.Minutes(),
            Weekday _ => period.Days(),
            WeekOfMonth _ => period.Weeks(),
            Weeks _ => period.Weeks(),
            Year _ => period.Years(),
            Years _ => period.Years(),
            _ => Enumerable.Empty<Period>()
        };
    }
}

public class PeriodGenerator2
{
    public IEnumerable<Period> GeneratePeriods<T>(SpanTimeDef<T> span, Period? limit = null)
    {
        if(span is SpanTimeDef<DateOnly> dateSpan)
        {
            var period = Period.New(dateSpan.From, dateSpan.To);
            return period.Days();
        }

        if(span is SpanTimeDef<DateTime> dateTimeSpan)
        {
            var period = Period.New(dateTimeSpan.From, dateTimeSpan.To);
            return period.Days();
        }

        if (span is SpanTimeDef<TimeOnly> timeSpan)
        {
            if(null == limit) return Enumerable.Empty<Period>();


            var period = Period.New(timeSpan.From, timeSpan.To);

            var range = new TimeDefRange();
            range.Visit(timeSpan);

            if (!range.Smallest.TryGet(out var smallest)) return Enumerable.Empty<Period>();

            return PeriodHelper.Chop(limit.Value, smallest);
        }

        return Enumerable.Empty<Period>();
    }
}