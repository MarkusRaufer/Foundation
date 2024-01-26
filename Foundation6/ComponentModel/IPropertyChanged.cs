namespace Foundation.ComponentModel;

public interface IPropertyChanged
{
    DictionaryAction Action { get; }
    string PropertyName { get; }
}
