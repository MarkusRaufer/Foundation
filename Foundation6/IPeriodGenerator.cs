namespace Foundation;

public interface IPeriodGenerator
{
    IEnumerable<Period> GeneratePeriods(Period period);
}

