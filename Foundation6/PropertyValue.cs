namespace Foundation;

public struct PropertyValue<TValue>
{
    private bool _isEmpty;

    public PropertyValue(PropertyType propertyType, TValue? value)
    {
        PropertyType = propertyType;
        Value = value;
        CollectionValue = default;
        _isEmpty = false;
    }

    public PropertyValue(PropertyType propertyType, ICollection<TValue>? value)
    {
        PropertyType = propertyType;
        CollectionValue = value;
        Value = default;
        _isEmpty = false;
    }

    public ICollection<TValue>? CollectionValue { get; }

    public bool IsEmpty => !_isEmpty;

    public PropertyType PropertyType { get; }

    public TValue? Value { get; }
}
