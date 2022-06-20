namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;
using System.Diagnostics.CodeAnalysis;

public class TypedIdPropertyMap<TId> : TypedIdPropertyMap<string, TId>
    where TId : notnull
{
    public TypedIdPropertyMap(
        string objectType, 
        KeyValue<string, TId> identifier, 
        char pathSeparator = '/') 
        : base(objectType, identifier, pathSeparator)
    {
    }

    public TypedIdPropertyMap(
        string objectType, 
        KeyValue<string, TId> identifier, 
        SortedDictionary<string, object> dictionary, 
        char pathSeparator = '/') 
        : base(objectType, identifier, dictionary, pathSeparator)
    {
    }
}

public class TypedIdPropertyMap<TObjectType, TId>   
    : IdPropertyMap<TObjectType, TId, EntityChangedEvent<TObjectType, TId>>
    where TId : notnull
    where TObjectType : notnull
{
    public TypedIdPropertyMap(
        TObjectType objectType, 
        KeyValue<string, TId> identifier, 
        char pathSeparator = '/') 
        : base(objectType, identifier, pathSeparator)
    {
    }

    public TypedIdPropertyMap(
        TObjectType objectType, 
        KeyValue<string, TId> identifier,
        SortedDictionary<string, object> dictionary, 
        char pathSeparator = '/') 
        : base(objectType, identifier, dictionary, pathSeparator)
    {
    }

    protected override EntityChangedEvent<TObjectType, TId> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
    {
        return new (ObjectType, Identifier, new PropertyChangedEvent(propertyName, value, state));
    }
}
