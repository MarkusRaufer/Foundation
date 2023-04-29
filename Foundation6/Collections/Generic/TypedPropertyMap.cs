namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;

public class TypedPropertyMap 
    : TypedPropertyMap<string, ObjectPropertyValueChanged>
{
    public TypedPropertyMap(string objectType) : base(objectType)
    {
    }

    public TypedPropertyMap(string objectType, EquatableSortedDictionary<string, object> dictionary) 
        : base(objectType, dictionary)
    {
    }

    protected override ObjectPropertyValueChanged CreateChangedEvent(string propertyName, object? value, CollectionActionState state)
    {
        return new ObjectPropertyValueChanged(ObjectType, propertyName, value, state);
    }

    public override void HandleEvent(ObjectPropertyValueChanged propertyChanged)
    {
        if (null == propertyChanged) return;

        if (!ObjectType.Equals(propertyChanged.ObjectType)) return;

        switch (propertyChanged.ActionState)
        {
            case CollectionActionState.Added: Add(propertyChanged.PropertyName, propertyChanged.Value); break;
            case CollectionActionState.Removed: Remove(propertyChanged.PropertyName); break;
            case CollectionActionState.Replaced: this[propertyChanged.PropertyName] = propertyChanged.Value!; break;
        };
    }
}

public abstract class TypedPropertyMap<TObjectType, TEvent>
    : PropertyMap<TEvent>
    , ITypedObject<TObjectType>
    where TObjectType : notnull
{
    public TypedPropertyMap(TObjectType objectType)
        : this(objectType, new EquatableSortedDictionary<string, object>())
    {
    }

    public TypedPropertyMap(TObjectType objectType, EquatableSortedDictionary<string, object> dictionary)
        : base(dictionary)
    {
        ObjectType = objectType.ThrowIfNull();
    }

    protected override abstract TEvent CreateChangedEvent(string propertyName, object? value, CollectionActionState state);

    public override abstract void HandleEvent(TEvent @event);

    public TObjectType ObjectType { get; }

    public override string ToString() => $"{nameof(ObjectType)}: {ObjectType}, {base.ToString()}";
}

