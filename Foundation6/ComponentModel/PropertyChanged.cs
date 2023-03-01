namespace Foundation.ComponentModel;

public record PropertyChanged(string PropertyName, PropertyChangedState ChangedState) : IPropertyChanged;
