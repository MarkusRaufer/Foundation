namespace Foundation.ComponentModel;

public interface IIdentifiableFactory<TFactoryId>
{
    TFactoryId FactoryId { get; }
}
