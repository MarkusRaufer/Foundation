namespace Foundation.ComponentModel;

public record PropertyValueChanged(string PropertyName, CollectionActionState ActionState, object? Value) 
    : PropertyValueChanged<object>(PropertyName, ActionState,  Value);

public record PropertyValueChanged<TValue>(string PropertyName, CollectionActionState ActionState, TValue? Value)
    : PropertyChanged(PropertyName, ActionState)
    , IPropertyValueChanged<TValue>;
