using System.Text.Json.Serialization;

namespace Foundation.Text.Json;

/// <summary>
/// A parameter of a factory method can be marked.
/// </summary>
/// <param name="parameterName"></param>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public class JsonFactoryMethodParameter(string parameterName) : JsonAttribute
{
    /// <summary>
    /// Should have the name of the factory method parameter.
    /// </summary>
    public string ParameterName { get; } = parameterName;
}
