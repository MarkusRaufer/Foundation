namespace Foundation.ComponentModel;

public class GuidNamedIdFactory
    : IIdFactory<NamedId>
    , IIdentifiableFactory<string>
{
    public GuidNamedIdFactory(string name)
    {
        FactoryId = name.ThrowIfNullOrEmpty();
    }

    public string FactoryId { get; }

    public NamedId NewId()
    {
        return NamedId.New(FactoryId, (IComparable)Guid.NewGuid());
    }
}
