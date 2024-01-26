namespace Foundation.ComponentModel;

public record PropertyValueChanged(string PropertyName, DictionaryAction Action, object? Value) 
    : PropertyValueChanged<object>(PropertyName, Action,  Value);

public record PropertyValueChanged<TValue>(string PropertyName, DictionaryAction Action, TValue? Value)
    : PropertyChanged(PropertyName, Action)
    , IPropertyValueChanged<TValue>;
