using Foundation.ComponentModel;

namespace Foundation;

public interface IObjectTypeValueConverter 
    : ITypeNameValueConverter
    , ITypedObject<string>
{
}
