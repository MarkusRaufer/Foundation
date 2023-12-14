namespace Foundation.ComponentModel;

public class NamedIdFactory
    : IIdFactory<NamedId>
    , IIdentifiableFactory<string>
{
    private readonly Func<IComparable> _idFactory;

    public NamedIdFactory(string factoryId, Func<IComparable> idFactory)
    {
        FactoryId = factoryId.ThrowIfNullOrWhiteSpace();
        _idFactory = idFactory.ThrowIfNull();
    }

    public string FactoryId { get; }

    public NamedId NewId()
    {
        var id = _idFactory();
        return new NamedId(FactoryId, id);
    }
}

public class NamedIdFactory<TId>
    : IIdFactory<NamedId<TId>>
    , IIdentifiableFactory<string>
    where TId : struct, IComparable<TId>, IEquatable<TId>
{
    private readonly Func<TId> _idFactory;

    public NamedIdFactory(string factoryId, Func<TId> idFactory)
    {
        FactoryId = factoryId.ThrowIfNullOrWhiteSpace();
        _idFactory = idFactory.ThrowIfNull();
    }

    public string FactoryId { get; }

    public NamedId<TId> NewId()
    {
        var id = _idFactory();
        return new NamedId<TId>(FactoryId, id);
    }
}