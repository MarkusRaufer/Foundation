namespace Foundation.ComponentModel;

public record ObjectPropertyValueChanged(
    string ObjectType,
    string PropertyName,
    object? Value,
    CollectionActionState ActionState)
    : ObjectPropertyValueChanged<string, object>(ObjectType, PropertyName, Value, ActionState);

public record ObjectPropertyValueChanged<TObjectType, TValue>(
    TObjectType ObjectType,
    string PropertyName,
    TValue? Value,
    CollectionActionState ActionState)
    : ObjectPropertyChanged<TObjectType>(ObjectType, PropertyName, ActionState)
    , IObjectPropertyValueChanged<TObjectType, TValue>;
