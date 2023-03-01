namespace Foundation.ComponentModel;

public record PropertyValueChangedEvent<TEventId, TValue, TPropertyChanged>(TEventId EventId, TPropertyChanged PropertyChanged) 
    : IPropertyValueChangedEvent<TEventId, TValue, TPropertyChanged>
    where TPropertyChanged : IPropertyValueChanged<TValue>;
