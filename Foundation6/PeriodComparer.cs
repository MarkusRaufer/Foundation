namespace Foundation;

public class PeriodComparer : IComparer<Period>
{

    public int Compare(Period x, Period y)
    {
        return PeriodHelper.Compare(x, y, EmptyPeriodIsSmaller);
    }

    public bool EmptyPeriodIsSmaller { get; init; } = true;
}

