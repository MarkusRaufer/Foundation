namespace Foundation;

public interface IIdentifiableFactory<TFactoryId>
{
    TFactoryId FactoryId { get; }
}
