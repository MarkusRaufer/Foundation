namespace Foundation.ComponentModel;

public record ObjectPropertyValueChanged<TObjectType, TValue>(
    TObjectType ObjectType,
    string PropertyName,
    TValue? Value,
    PropertyChangedState ChangedState)
    : ObjectPropertyChanged<TObjectType>(ObjectType, PropertyName, ChangedState)
    , IObjectPropertyValueChanged<TObjectType, TValue>;
