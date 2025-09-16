using Foundation.Collections.Generic;
using Foundation.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class FactoryMethodConverter<T> : JsonConverter<T?>
{
    public const string FactoryMethodPropertyName = "__factory_method_parameters__";

    private MethodInfo? _factoryMethod;
    private readonly Type _factoryMethodAttributeType = typeof(JsonFactoryMethod);
    private readonly Type _factoryMethodParameterAttributeType = typeof(JsonFactoryMethodParameter);
    private List<KeyValuePair<string, MemberInfo>> _factoryMethodParameters = [];

    private static object? CreateObject(Type typeToConvert, ConstructorInfo? constructorInfo)
    {
        if (constructorInfo is null) return null;

        var parameters = constructorInfo.GetParameters();

        if (parameters.Length == 0) return Activator.CreateInstance(typeToConvert, !constructorInfo.IsPublic);
        
        var values = parameters.Select(x => x.ParameterType.GetDefault()).ToArray();
        return Activator.CreateInstance(typeToConvert, values);
    }

    private static T? Deserialize(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize(ref reader, typeToConvert, options) is T t ? t : default;

    private MethodInfo? GetFactoryMethod(Type type)
    {
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
        {
            if (method.GetCustomAttributes(_factoryMethodAttributeType).Any()) return method;
        }

        return null;
    }

    private Dictionary<string, MemberInfo> GetJsonFactoryMethodParameters(Type type)
    {
        Dictionary<string, MemberInfo> parameters = [];
        if (type is null) return parameters;

        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var attribute = field.GetCustomAttribute(_factoryMethodParameterAttributeType);

            if (attribute is JsonFactoryMethodParameter parameter)
                parameters.Add(parameter.ParameterName, field);
        }

        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var attribute = property.GetCustomAttribute(_factoryMethodParameterAttributeType);

            if (attribute is JsonFactoryMethodParameter parameter)
                parameters.Add(parameter.ParameterName, property);
        }

        return parameters;
    }

    [return:NotNull]
    private static ParameterInfo[] GetParameters(MethodInfo? method) => method is null ? [] : method.GetParameters();

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (_factoryMethod is null) _factoryMethod = GetFactoryMethod(typeToConvert);

        if (_factoryMethod is null) return Deserialize(ref reader, typeToConvert, options);

        var parameters = _factoryMethod.GetParameters();
        var factoryParameters = parameters.Length == 0 ? [] : GetJsonFactoryMethodParameters(typeToConvert);

        if (!reader.Read()) return default;

        var propertyName = reader.TokenType == JsonTokenType.PropertyName
            ? reader.GetString()
            : default;

        if (!FactoryMethodPropertyName.EqualsNullable(propertyName)) return Deserialize(ref reader, typeToConvert, options);

        Dictionary<string, object?> parameterValues = [];

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propertyName = reader.GetString();
                continue;
            }

            if (!string.IsNullOrEmpty(propertyName))
            {
                if (!factoryParameters.TryGetValue(propertyName!, out var memberInfo)) continue;

                var parameterValue = JsonSerializer.Deserialize(ref reader, memberInfo.GetMemberType(), options);
                parameterValues.Add(propertyName!, parameterValue);
                propertyName = null;
            }
        }

        if (parameters.Length != parameterValues.Count) return default;

        return _factoryMethod.Invoke(null, parameterValues.Values.ToArray()) is T t ? t : default;
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value is null) return;

        var type = value.GetType();

        if (_factoryMethod is null)
        {
            _factoryMethod = GetFactoryMethod(type);
            var parameters = GetParameters(_factoryMethod);
            var factoryMethodParams = GetJsonFactoryMethodParameters(type);
            foreach (var parameter in parameters)
            {
                if (parameter is null || string.IsNullOrEmpty(parameter.Name)) throw new JsonException("invalid factory method parameter");

                if (!factoryMethodParams.TryGetValue(parameter.Name, out var member))
                    throw new JsonException($"parameter {parameter.Name} needs a member marked with attribute JsonFactoryMethodParameter");

                _factoryMethodParameters.Add(new KeyValuePair<string, MemberInfo>(parameter.Name, member));
            }
        }

        writer.WriteStartObject();

        writer.WritePropertyName(FactoryMethodPropertyName);

        // value start of __factory_method__
        writer.WriteStartObject();

        foreach (var (propertyName, member) in _factoryMethodParameters)
        {
            writer.WritePropertyName(propertyName);

            var memberValue = member.GetValue(value);
            JsonSerializer.Serialize(writer, memberValue, options);
        }

        // value end of __factory_method__
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
