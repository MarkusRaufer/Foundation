namespace Foundation.ComponentModel;

public record ObjectPropertyValueChanged(
    string ObjectType,
    string PropertyName,
    object? Value,
    DictionaryAction Action)
    : ObjectPropertyValueChanged<string, object>(ObjectType, PropertyName, Value, Action);

public record ObjectPropertyValueChanged<TObjectType, TValue>(
    TObjectType ObjectType,
    string PropertyName,
    TValue? Value,
    DictionaryAction Action)
    : ObjectPropertyChanged<TObjectType>(ObjectType, PropertyName, Action)
    , IObjectPropertyValueChanged<TObjectType, TValue>;
