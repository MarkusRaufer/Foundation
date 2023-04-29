namespace Foundation.ComponentModel;

public record PropertyChanged(string PropertyName, CollectionActionState ActionState) : IPropertyChanged;
