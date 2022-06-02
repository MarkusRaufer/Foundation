namespace Foundation.ComponentModel;

public interface ICollectionChanged<T>
{
    Event<Action<CollectionEvent<T>>> CollectionChanged { get; }
}
