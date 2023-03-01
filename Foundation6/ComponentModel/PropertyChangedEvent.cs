namespace Foundation.ComponentModel;

public record PropertyChangedEvent<TEventId, TPropertyChanged>(TEventId EventId, TPropertyChanged PropertyChanged) 
    : IPropertyChangedEvent<TEventId, TPropertyChanged>
    where TPropertyChanged : IPropertyChanged;
