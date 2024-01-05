using Foundation.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class IdJsonConverter : JsonConverter<Id>
{
    private readonly Assembly[] _assemblies;

    public IdJsonConverter() : this(GetAssemblies(Assembly.GetExecutingAssembly().Location))
    {
    }

    public IdJsonConverter(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ThrowIfNull().ToArray();
    }

    private static IEnumerable<Assembly> GetAssemblies(string location)
    {
        var dir = Path.GetDirectoryName(location);
        if (dir is null) yield break;

        var dlls = Directory.GetFiles(dir, "*.dll");
        foreach(var dll in dlls)
        {
            yield return Assembly.LoadFile(dll);
        }
    }

    public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) return Id.Empty;
        if (!reader.Read()) throw new JsonException($"missing {nameof(Id)} object in JSON");

        // Type property
        if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException($"missing {nameof(Id.Type)} property");

        var typePropertyName = reader.GetString();
        if (nameof(Id.Type) != typePropertyName) throw new JsonException($"missing {nameof(Id.Type)} property");

        if (!reader.Read()) throw new JsonException($"missing {nameof(Id.Type)} property");

        var typePropertyValue = reader.GetString();
        if (null == typePropertyValue) throw new JsonException($"missing {nameof(Id.Type)} property value");

        var type = Type.GetType(typePropertyValue);
        if (type is null)
        {
            var span = typePropertyValue.AsSpan();
            var index = span.IndexOf('+');
            var typeName = span[(index + 1)..].ToString();

            type = _assemblies.SelectMany(x => x.GetTypes()).Where(x => x.Name == typeName).FirstOrDefault();
            if (type is null) throw new JsonException($"cannot deserialize type {typePropertyValue}");
        }


        // Value property
        if (!reader.Read()) throw new JsonException($"missing {nameof(Id.Value)} property"); ;

        if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException($"missing {nameof(Id.Value)} property");

        var valuePropertyName = reader.GetString();
        if (nameof(Id.Value) != valuePropertyName) throw new JsonException($"missing {nameof(Id.Value)} property");

        if (!reader.Read()) throw new JsonException($"missing {nameof(Id.Value)} property");

        var propertyValue = reader.GetValue(type);
        if (null == propertyValue) throw new JsonException($"value of property {nameof(Id.Value)} must not be null");

        reader.Read();

        return Id.New(propertyValue);
    }

    public override void Write(Utf8JsonWriter writer, Id id, JsonSerializerOptions options)
    {
        if (id.IsEmpty) return;

        writer.WriteStartObject();
        
        // Type property
        writer.WritePropertyName(nameof(Id.Type));
        var name = id.Type.FullName ?? id.Type.Name;
        writer.WriteStringValue(name);

        // Value property
        writer.WritePropertyName(nameof(Id.Value));
        writer.WriteValue(id.Value);

        writer.WriteEndObject();
    }
}
