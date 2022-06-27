namespace Foundation;

public class GuidNamedIdFactory : IIdFactory<NamedId>
{
    private readonly string _name;
    public GuidNamedIdFactory(string name)
    {
        _name = name.ThrowIfNullOrEmpty();
    }

    public NamedId CreateId()
    {
        return NamedId.New(_name, (IComparable)Guid.NewGuid());
    }
}
