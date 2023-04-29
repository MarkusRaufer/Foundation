namespace Foundation.ComponentModel;

public record EntityChanged<TEventId, TEntityId, TChangedState>(
    TEventId EventId,
    TEntityId EntityId,
    TChangedState ChangedState)
    : EntityEvent<TEventId, TEntityId>(EventId, EntityId)
    , IEntityChanged<TEventId, TEntityId, TChangedState>
    where TEntityId : notnull;

public record EntityChanged<TEventId, TObjectType, TEntityId, TChangedState>(
    TEventId EventId,
    TObjectType ObjectType,
    TEntityId EntityId,
    TChangedState ChangedState)
    : EntityChanged<TEventId, TEntityId, TChangedState>(EventId, EntityId, ChangedState)
    , IEntityChanged<TEventId, TObjectType, TEntityId, TChangedState>
    where TEntityId : notnull;
