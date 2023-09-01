namespace Foundation.ComponentModel;

public interface IEntityEvent<TEventId, TEntityId> : IEvent<TEventId>
{
    TEntityId EntityId { get; }
}

public interface IEntityEvent<TEventId, TEntityId, TObjectType>
    : IEntityEvent<TEventId, TEntityId>
    , ITypedObject<TObjectType>
{
}