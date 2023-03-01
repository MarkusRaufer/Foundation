namespace Foundation.ComponentModel;

public record EntityPropertyValueChanged<TEventId, TEntityId, TValue, TPropertyChanged>(
    TEventId EventId,
    TEntityId EntityId,
    TPropertyChanged PropertyChanged)
    : EntityEvent<TEventId, TEntityId>(EventId, EntityId)
    , IEntityPropertyChanged<TEventId, TEntityId, TPropertyChanged>
    where TEntityId : notnull
    where TPropertyChanged : IPropertyValueChanged<TValue>;

public record EntityPropertyValueChanged<TEventId, TEntityId, TObjectType, TValue, TPropertyChanged>(
    TEventId EventId,
    TEntityId EntityId,
    TObjectType ObjectType,
    TPropertyChanged PropertyChanged)
    : EntityEvent<TEventId, TEntityId, TObjectType>(EventId, EntityId, ObjectType)
    , IEntityPropertyChanged<TEventId, TEntityId, TObjectType, TPropertyChanged>
    where TEntityId : notnull
    where TPropertyChanged : IPropertyValueChanged<TValue>;