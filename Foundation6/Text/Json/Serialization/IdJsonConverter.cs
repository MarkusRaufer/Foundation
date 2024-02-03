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
﻿using Foundation.Collections.Generic;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class IdJsonConverter : JsonConverter<Id>
{
    private readonly TypeJsonConverter _typeJsonConverter;

    public IdJsonConverter() : this(new TypeJsonConverter())
    {
    }

    public IdJsonConverter(TypeJsonConverter typeJsonConverter)
    {
        _typeJsonConverter = typeJsonConverter.ThrowIfNull();
    }

    public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) return Id.Empty;

        ReadTypeProperty(ref reader);

        var type = _typeJsonConverter.Read(ref reader, typeToConvert, options);
        if (null == type) throw new JsonException(nameof(Id));

        if (!reader.Read() || reader.TokenType != JsonTokenType.PropertyName) throw new JsonException(nameof(Id));
        
        // Value result
        var result = reader.GetProperty(type);
        if(!result.TryGetOk(out var property)) throw new JsonException(nameof(Id));

        if (property.Value is null) throw new JsonException(nameof(Id));

        reader.Read();

        return Id.New(property.Value);
    }

    private void ReadTypeProperty(ref Utf8JsonReader reader)
    {
        if (!reader.Read()) throw new JsonException(nameof(Id));

        // Type property
        if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException(nameof(Id));

        var propertyName = reader.GetString();
        if (propertyName != nameof(Id.Type)) throw new JsonException(nameof(Id));

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject) throw new JsonException(nameof(Id));
    }

    public override void Write(Utf8JsonWriter writer, Id id, JsonSerializerOptions options)
    {
        if (id.IsEmpty) return;

        writer.WriteStartObject();
        
        // Type property
        writer.WritePropertyName(nameof(Id.Type));

        // Type property value
        _typeJsonConverter.Write(writer, id.Type, options);

        // Value result
        writer.WritePropertyName(nameof(Id.Value));
        writer.WriteValue(id.Value);

        writer.WriteEndObject();
    }
}
