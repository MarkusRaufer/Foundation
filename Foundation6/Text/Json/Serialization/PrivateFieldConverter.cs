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
﻿using System.Reflection;
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
