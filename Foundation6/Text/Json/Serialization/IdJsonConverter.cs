// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System.Text.Json;
using System.Text.Json.Serialization;
using Foundation.Reflection;

namespace Foundation.Text.Json.Serialization;

public class IdJsonConverter : JsonConverter<Id>
{
    private readonly ObjectJsonConverter _objectJsonConverter;
    private readonly Dictionary<string, IObjectTypeValueConverter> _objectTypeConverters = [];
    private readonly ITypeNameValueConverter _primitiveTypeConverter;

    public IdJsonConverter() : this([])
    {
    }

    public IdJsonConverter(IEnumerable<IObjectTypeValueConverter> converters)
    {
        foreach (var converter in converters)
        {
            if (converter is PrimitiveObjectTypeConverter primitiveTypeConverter)
            {
                _primitiveTypeConverter = primitiveTypeConverter;
            }
            _objectTypeConverters.Add(converter.ObjectType, converter);
        }

        _objectJsonConverter = new ObjectJsonConverter();
        if (_primitiveTypeConverter is null) _primitiveTypeConverter = new PrimitiveObjectTypeConverter();
    }

    public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) return Id.Empty;

        if (!reader.GetProperty().TryGetOk(out var typeProperty) || typeProperty.Value is not string typeString) return Id.Empty;

        if (!reader.GetProperty().TryGetOk(out var valueProperty) || valueProperty.Value is null) return Id.Empty;

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndObject) return Id.Empty;

        // check primitive type
        {
            var result = _primitiveTypeConverter.Convert(typeString, valueProperty.Value);
            if (result.TryGetOk(out var ok)) return Id.New(ok);
        }

        if (!_objectTypeConverters.TryGetValue(typeString, out var objectTypeConverter) || objectTypeConverter is null) return Id.Empty;

        return objectTypeConverter.Convert(typeString, valueProperty.Value).TryGetOk(out var value)
            ? Id.New(value)
            : Id.Empty;
    }

    public override void Write(Utf8JsonWriter writer, Id id, JsonSerializerOptions options)
    {
        if (id.IsEmpty) return;

        writer.WriteStartObject();
        
        // Type property
        writer.WritePropertyName(nameof(Id.Type));
        writer.WriteStringValue(id.Type.FullName);

        // Value property
        writer.WritePropertyName(nameof(Id.Value));
        _objectJsonConverter.Write(writer, id.Value, options);

        writer.WriteEndObject();
    }
}
