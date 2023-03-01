namespace Foundation.ComponentModel;

public interface IObjectPropertyValueChanged<TObjectType, TValue> : IObjectPropertyChanged<TObjectType>
{
    TValue? Value { get; }
}
