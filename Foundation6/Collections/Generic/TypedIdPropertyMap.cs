namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;

public class TypedIdPropertyMap<TId> : TypedIdPropertyMap<string, TId>
    where TId : notnull
{
    public TypedIdPropertyMap(
        string objectType,
        KeyValuePair<string, TId> identifier, 
        char pathSeparator = '/') 
        : base(objectType, identifier, pathSeparator)
    {
    }

    public TypedIdPropertyMap(
        string objectType,
        KeyValuePair<string, TId> identifier, 
        SortedDictionary<string, object> dictionary, 
        char pathSeparator = '/') 
        : base(objectType, identifier, dictionary, pathSeparator)
    {
    }
}

public class TypedIdPropertyMap<TObjectType, TId>   
    : IdPropertyMap<TObjectType, TId, EntityPropertyValueChanged<Guid, TId, TObjectType, object, PropertyValueChanged<object>>>
    where TId : notnull
    where TObjectType : notnull
{
    public TypedIdPropertyMap(
        TObjectType objectType,
        KeyValuePair<string, TId> identifier, 
        char pathSeparator = '/') 
        : base(objectType, identifier, pathSeparator)
    {
    }

    public TypedIdPropertyMap(
        TObjectType objectType,
        KeyValuePair<string, TId> identifier,
        SortedDictionary<string, object> dictionary, 
        char pathSeparator = '/') 
        : base(objectType, identifier, dictionary, pathSeparator)
    {
    }

    public override void HandleEvent(EntityPropertyValueChanged<Guid, TId, TObjectType, object, PropertyValueChanged<object>> @event)
    {
        throw new NotImplementedException();
    }

    protected override EntityPropertyValueChanged<Guid, TId, TObjectType, object, PropertyValueChanged<object>> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
    {
        return new (Guid.NewGuid(), Id, ObjectType, new PropertyValueChanged<object>(propertyName, state, value));
    }
}
