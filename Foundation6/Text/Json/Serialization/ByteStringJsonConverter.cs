using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class ByteStringJsonConverter : JsonConverter<ByteString>
{
    public ByteStringJsonConverter()
    {
    }

    public override ByteString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var bytes = reader.GetBytesFromBase64();
            return new ByteString(bytes);
        }
        {
            var bytes = new List<byte>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    var b = reader.GetByte();
                    bytes.Add(b);
                }
            }
            return new ByteString([.. bytes]);
        }
    }

    public override void Write(Utf8JsonWriter writer, ByteString value, JsonSerializerOptions options)
    {
        var base64 = value.ToBase64String();
        writer.WriteBase64StringValue(Convert.FromBase64String(base64));
    }
}
