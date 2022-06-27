namespace Foundation;

public class IdFactory : IIdFactory<Id>, IResetable
{
    private Id _current;
    private readonly Func<Id, Id> _factory;
    private readonly Id _seed;
    private bool _passed;

    public IdFactory(Id seed, Func<Id, Id> factory)
    {
        _seed = seed;
        _factory = factory.ThrowIfNull();

        _current = seed;
        _passed = false;
    }

    public Id CreateId()
    {
        if(!_passed)
        {
            _passed = true;
            return _current;
        }

        return _factory(_current);
    }

    public void Reset()
    {
        _current = _seed;
        _passed = false;
    }
}

