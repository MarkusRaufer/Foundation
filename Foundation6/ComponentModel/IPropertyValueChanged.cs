namespace Foundation.ComponentModel;

public interface IPropertyValueChanged<TValue> : IPropertyChanged
{
    TValue? Value { get; }
}
