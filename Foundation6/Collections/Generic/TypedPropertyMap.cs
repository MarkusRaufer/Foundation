namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class TypedPropertyMap<TObjectType> 
    : PropertyMap<TObjectType, PropertyChangedEvent<TObjectType>>
    where TObjectType : notnull
{
    public TypedPropertyMap(
        [DisallowNull] TObjectType objectType, 
        char pathSeparator = '/') 
        : base(objectType, pathSeparator)
    {
    }

    public TypedPropertyMap(
        [DisallowNull] TObjectType objectType,
        [DisallowNull] SortedDictionary<string, object> dictionary, 
        char pathSeparator = '/') 
        : base(objectType, dictionary, pathSeparator)
    {
    }

    public override void HandleEvent(PropertyChangedEvent<TObjectType> propertyChanged)
    {
        if (null == propertyChanged) return;

        if (!ObjectType.Equals(propertyChanged.ObjectType)) return;

        switch (propertyChanged.ChangedState)
        {
            case PropertyChangedState.Added: Add(propertyChanged.Name, propertyChanged.Value); break;
            case PropertyChangedState.Removed: Remove(propertyChanged.Name); break;
            case PropertyChangedState.Replaced: this[propertyChanged.Name] = propertyChanged.Value!; break;
        };
    }

    protected override PropertyChangedEvent<TObjectType> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
    {
        return new PropertyChangedEvent<TObjectType>(ObjectType, propertyName, value, state);
    }
}

