namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;

public class TypedPropertyMap<TObjectType> 
    : PropertyMap<TObjectType, ObjectPropertyValueChanged<TObjectType, object>>
    where TObjectType : notnull
{
    public TypedPropertyMap(TObjectType objectType) : base(objectType)
    {
    }

    public TypedPropertyMap(TObjectType objectType, EquatableSortedDictionary<string, object> dictionary) 
        : base(objectType, dictionary)
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

