namespace Foundation;

public class IntegerIdentifierFactory : IIdentifierFactory, IResetable
{
    private int _current;
    private int _min;

    public IntegerIdentifierFactory(int min = 0)
    {
        _min = min;
        _current = min;
    }

    public Identifier CreateEmptyIdentifier()
    {
        return Identifier.New(_min);
    }

    public Identifier CreateIdentifier()
    {
        return Identifier.New(++_current);
    }

    public Identifier CreateIdentifier(object value)
    {
        if (null == value) throw new ArgumentNullException("value");
        if (!(value is int)) throw new ArgumentOutOfRangeException("value is not an int");
        var id = (int)value;
        return Identifier.New(id);
    }

    public Identifier CreateIdentifierFromString(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("value");
        var id = int.Parse(value);
        return Identifier.New(id);
    }

    public void Reset()
    {
        _current = _min;
    }
}

