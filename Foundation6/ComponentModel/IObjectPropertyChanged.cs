namespace Foundation.ComponentModel
{
    public interface IObjectPropertyChanged<TObjectType> 
        : IPropertyChanged
        , ITypedObject<TObjectType>
    {
    }
}