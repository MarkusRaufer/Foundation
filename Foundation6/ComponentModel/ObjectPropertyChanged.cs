namespace Foundation.ComponentModel;

public record ObjectPropertyChanged<TObjectType>(
    TObjectType ObjectType,
    string PropertyName,
    CollectionActionState ActionState)
    : PropertyChanged(PropertyName, ActionState)
    , IObjectPropertyChanged<TObjectType>;
