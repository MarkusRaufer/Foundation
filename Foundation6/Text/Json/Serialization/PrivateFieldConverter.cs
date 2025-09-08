using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class PrivateFieldConverter<T> : JsonConverter<T?>
{
    public const string PrivateFieldsPropertyName = "__private_fields__"; 
    private readonly List<FieldInfo> _fields = [];
    private readonly Type _privateFieldAttributeType = typeof(JsonPrivateField);

    private static object? CreateObject(Type typeToConvert, ConstructorInfo? constructorInfo)
    {
        if (constructorInfo is null) return null;

        var parameters = constructorInfo.GetParameters();

        if (parameters.Length == 0) return Activator.CreateInstance(typeToConvert, !constructorInfo.IsPublic);
        
        var values = parameters.Select(x => x.ParameterType.GetDefault()).ToArray();
        return Activator.CreateInstance(typeToConvert, values);
    }

    /// <summary>
    /// Tries to get a default constructor or when not found a constructor marked with the JsonConstructor attribute.
    /// </summary>
    /// <remarks>
    /// If constructor with JsonConstructor attribute is found. make sure that there are no null or range checks in the constructor.
    /// This is typically used with factory methods like New(...) where you do your checks.
    /// </remarks>
    /// <param name="type">The type of the constructor.</param>
    /// <returns></returns>
    private ConstructorInfo? GetConstructor(Type type)
    {
        var constructorInfos = type.GetConstructors(BindingFlags.Instance
                                                   | BindingFlags.Public
                                                   | BindingFlags.NonPublic);

        var constructorInfo = constructorInfos.FirstOrDefault(x => x.GetParameters().Length == 0);
       
        // has default constructor?
        if (constructorInfo is not null) return constructorInfo;

        foreach (var ctorInfo in constructorInfos)
        {
            var hasAttribute = ctorInfo.GetCustomAttributes().Any(x => x is JsonConstructorAttribute);
            if (hasAttribute) return ctorInfo;            
        }

        return null;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(_fields.Count == 0)
        {
            foreach (var field in typeToConvert.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.GetCustomAttributes(_privateFieldAttributeType).Any())
                    _fields.Add(field);
            }
        }

        if (_fields.Count == 0) return default;

        ConstructorInfo? constructorInfo = GetConstructor(typeToConvert);

        var obj = CreateObject(typeToConvert, constructorInfo);

        if (obj is null) return default;

        string? name = null;
        FieldInfo? fieldInfo = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                name = reader.GetString();
                fieldInfo = _fields.FirstOrDefault(x =>  x.Name == name);
                continue;
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (name == PrivateFieldsPropertyName)
                {
                    name = null;
                    continue;
                }

                if (fieldInfo is null)
                {
                    var property = typeToConvert.GetProperty(name);
                    if (property is null)
                    {
                        name = null;
                        continue;
                    }
                    var propertyValue = reader.GetValue(property.PropertyType);
                    property.SetValue(obj, propertyValue, null);
                    name = null;
                    continue;
                }


                var fieldValue = JsonSerializer.Deserialize(ref reader, fieldInfo.FieldType, options);
                fieldInfo.SetValue(obj, fieldValue);
                name = null;
            }
        }

        return obj is T t ? t : default;
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value is null) return;

        if (_fields.Count == 0)
        {
            var type = value.GetType();
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.GetCustomAttributes(_privateFieldAttributeType).Any())
                    _fields.Add(field);
            }
        }
        writer.WriteStartObject();

        writer.WritePropertyName(PrivateFieldsPropertyName);

        // value start of __private_fields__
        writer.WriteStartObject();

        foreach (var field in _fields)
        {
            writer.WritePropertyName(field.Name);

            var fieldValue = field.GetValue(value);
            JsonSerializer.Serialize(writer, fieldValue, options);
        }

        // value end of __private_fields__
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
