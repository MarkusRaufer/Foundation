namespace Foundation;

public class DateTimeProvider : IDateTimeProvider
{
    private readonly Func<DateTime> _factory;

    public DateTimeProvider(Func<DateTime> factory)
    {
        _factory = factory.ThrowIfNull();
    }

    public DateTime Now => _factory();
}
