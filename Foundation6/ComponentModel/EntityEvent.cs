namespace Foundation.ComponentModel;

public record EntityEvent<TEventId, TEntityId>(TEventId EventId, TEntityId EntityId) 
    : IEntityEvent<TEventId, TEntityId>
    where TEntityId : notnull;

public record EntityEvent<TEventId, TEntityId, TObjectType>(TEventId EventId, TEntityId EntityId, TObjectType ObjectType)
    : EntityEvent<TEventId, TEntityId>(EventId, EntityId)
    , ITypedObject<TObjectType>
    where TEntityId : notnull;
