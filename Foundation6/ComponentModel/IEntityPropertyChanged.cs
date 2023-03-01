namespace Foundation.ComponentModel;

public interface IEntityPropertyChanged<TEventId, TEntityId, TPropertyChanged>
    : IEntityEvent<TEventId, TEntityId>
    where TEntityId : notnull
{
    TPropertyChanged PropertyChanged { get; }
}


public interface IEntityPropertyChanged<TEventId, TEntityId, TObjectType, TPropertyChanged>
    : IEntityChanged<TEventId, TEntityId, TObjectType, TPropertyChanged>
    , ITypedObject<TObjectType>
    where TEntityId : notnull
{
}
