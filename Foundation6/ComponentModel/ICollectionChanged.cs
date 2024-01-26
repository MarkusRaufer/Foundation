namespace Foundation.ComponentModel;

public interface ICollectionChanged<T> : ICollectionChanged<T, CollectionEvent<T>>
{
}

public interface ICollectionChanged<T, TEvent>
{
    Event<Action<TEvent>> CollectionChanged { get; }
}