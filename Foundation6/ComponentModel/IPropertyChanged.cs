namespace Foundation.ComponentModel;

public interface IPropertyChanged
{
    PropertyChangedState ChangedState { get; }
    string PropertyName { get; }
}
