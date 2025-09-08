using System.Text.Json.Serialization;

namespace Foundation.Text.Json;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]

public class JsonFactoryMethod : JsonAttribute
{
}
