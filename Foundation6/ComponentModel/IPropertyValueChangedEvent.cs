namespace Foundation.ComponentModel;

public interface IPropertyValueChangedEvent<TEventId, TValue, TPropertyChanged>
    : IPropertyChangedEvent<TEventId, TPropertyChanged>
    where TPropertyChanged : IPropertyValueChanged<TValue>
{
}
