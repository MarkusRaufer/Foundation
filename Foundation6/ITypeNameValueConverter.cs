namespace Foundation;

public interface ITypeNameValueConverter
{
    Result<object?, Error> Convert(string typeName, object? value);
}