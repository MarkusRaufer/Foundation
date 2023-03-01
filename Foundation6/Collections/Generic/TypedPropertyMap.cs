namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;
using System.Collections.Generic;

public class TypedPropertyMap<TObjectType> 
    : PropertyMap<TObjectType, ObjectPropertyValueChanged<TObjectType, object>>
    where TObjectType : notnull
{
    public TypedPropertyMap(
        TObjectType objectType, 
        char pathSeparator = '/') 
        : base(objectType, pathSeparator)
    {
    }

    public TypedPropertyMap(
        TObjectType objectType,
        SortedDictionary<string, object> dictionary, 
        char pathSeparator = '/') 
        : base(objectType, dictionary, pathSeparator)
    {
    }

    public override void HandleEvent(ObjectPropertyValueChanged<TObjectType, object> propertyChanged)
    {
        if (null == propertyChanged) return;

        if (!ObjectType.Equals(propertyChanged.ObjectType)) return;

        switch (propertyChanged.ChangedState)
        {
            case PropertyChangedState.Added: Add(propertyChanged.PropertyName, propertyChanged.Value); break;
            case PropertyChangedState.Removed: Remove(propertyChanged.PropertyName); break;
            case PropertyChangedState.Replaced: this[propertyChanged.PropertyName] = propertyChanged.Value!; break;
        };
    }

    protected override ObjectPropertyValueChanged<TObjectType, object> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
    {
        return new ObjectPropertyValueChanged<TObjectType, object>(ObjectType, propertyName, value, state);
    }
}

