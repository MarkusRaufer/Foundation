namespace Foundation.ComponentModel;

public record PropertyChanged(string PropertyName, DictionaryAction Action) : IPropertyChanged;
