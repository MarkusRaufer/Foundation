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
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class EnumerableConverter<T, TCollection> : JsonConverter<TCollection>
    where TCollection : IEnumerable<T>
{
    private readonly Func<IEnumerable<T>, TCollection> _factory;

    public EnumerableConverter(Func<IEnumerable<T>, TCollection> factory)
    {
        _factory = factory.ThrowIfNull();
    }

    public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException($"cannot convert to type {typeToConvert.Name}");

        var items = new List<T>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) break;

            if (JsonSerializer.Deserialize<T>(ref reader, options) is T item) items.Add(item);
        }

        return _factory(items);
    }

    public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
            JsonSerializer.Serialize(writer, item, options);

        writer.WriteEndArray();
    }
}
#endif
