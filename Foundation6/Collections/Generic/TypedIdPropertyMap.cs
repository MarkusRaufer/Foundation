namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;

public class TypedIdPropertyMap<TId> : TypedIdPropertyMap<string, TId>
    where TId : notnull
{
    public TypedIdPropertyMap(string objectType, TId id) 
        : base(objectType, id)
    {
    }

    public TypedIdPropertyMap(string objectType, TId id,  EquatableSortedDictionary<string, object> dictionary) 
        : base(objectType, id, dictionary)
    {
    }
}

public class TypedIdPropertyMap<TObjectType, TId>   
    : IdPropertyMap<TObjectType, TId, EntityPropertyValueChanged<Guid, TId, TObjectType, object, PropertyValueChanged<object>>>
    where TId : notnull
    where TObjectType : notnull
{
    public TypedIdPropertyMap(TObjectType objectType, TId id)
        : base(objectType, id)
    {
    }

    public TypedIdPropertyMap(TObjectType objectType, TId id, EquatableSortedDictionary<string, object> dictionary) 
        : base(objectType, id, dictionary)
    {
    }

    public override void HandleEvent(EntityPropertyValueChanged<Guid, TId, TObjectType, object, PropertyValueChanged<object>> @event)
    {
        if (null == @event) return;
        if (!ObjectType.Equals(@event.ObjectType)) return;

        switch(@event.PropertyChanged.ChangedState)
        {
            case PropertyChangedState.Added: Add(@event.PropertyChanged.PropertyName, @event.PropertyChanged.Value); break;
            case PropertyChangedState.Removed: Remove(@event.PropertyChanged.PropertyName); break;
            case PropertyChangedState.Replaced: this[@event.PropertyChanged.PropertyName] = @event.PropertyChanged.Value!; break;
        }
    }

    protected override EntityPropertyValueChanged<Guid, TId, TObjectType, object, PropertyValueChanged<object>> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
    {
        return new (Guid.NewGuid(), Id, ObjectType, new PropertyValueChanged<object>(propertyName, state, value));
    }
}
