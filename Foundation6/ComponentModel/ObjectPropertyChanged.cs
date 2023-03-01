namespace Foundation.ComponentModel;

public record ObjectPropertyChanged<TObjectType>(
    TObjectType ObjectType,
    string PropertyName,
    PropertyChangedState ChangedState)
    : PropertyChanged(PropertyName, ChangedState)
    , IObjectPropertyChanged<TObjectType>;
