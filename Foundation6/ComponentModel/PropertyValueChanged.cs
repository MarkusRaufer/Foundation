namespace Foundation.ComponentModel;

public record PropertyValueChanged<TValue>(string PropertyName, PropertyChangedState ChangedState, TValue? Value)
    : PropertyChanged(PropertyName, ChangedState)
    , IPropertyValueChanged<TValue>;
