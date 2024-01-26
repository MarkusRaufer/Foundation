namespace Foundation.ComponentModel;

public record ObjectPropertyChanged<TObjectType>(
    TObjectType ObjectType,
    string PropertyName,
    DictionaryAction Action)
    : PropertyChanged(PropertyName, Action)
    , IObjectPropertyChanged<TObjectType>;
