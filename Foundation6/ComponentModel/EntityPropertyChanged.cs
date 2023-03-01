namespace Foundation.ComponentModel;

public record EntityPropertyChanged<TEventId, TEntityId, TPropertyChanged>(
    TEventId EventId,
    TEntityId EntityId,
    TPropertyChanged PropertyChanged)
    : EntityEvent<TEventId, TEntityId>(EventId, EntityId)
    , IEntityPropertyChanged<TEventId, TEntityId, TPropertyChanged>
    where TEntityId : notnull
    where TPropertyChanged : IPropertyChanged;

public record EntityPropertyChanged<TEventId, TEntityId, TObjectType, TPropertyChanged>(
    TEventId EventId,
    TEntityId EntityId,
    TObjectType ObjectType,
    TPropertyChanged PropertyChanged)
    : EntityEvent<TEventId, TEntityId>(EventId, EntityId)
    , IEntityPropertyChanged<TEventId, TEntityId, TObjectType, TPropertyChanged>
    where TEntityId : notnull
    where TPropertyChanged : IPropertyChanged;
