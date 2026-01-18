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
﻿// The MIT License (MIT)
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
using System.Reflection;

namespace Foundation.ComponentModel;

public static class ObjectExtensions
{
    /// <summary>
    /// Creates update events from a list of key values. The keys represent property names.
    /// </summary>
    /// <typeparam name="T">The type of object that should be updated.</typeparam>
    /// <param name="obj">An instance of an object.</param>
    /// <param name="properties">The properties that should be updated.</param>
    /// <param name="ignorePropertiesWithNonPublicSetter">if true, an update event is only created when the kv has a public setter.</param>
    /// <param name="ignoreInvalidProperties">if false, an ArgumentException is thrown.</param>
    /// <returns>A list of update events.</returns>
    /// <exception cref="ArgumentException">Is thrown when <paramref name="ignoreInvalidProperties"/> is false and an invalid property is used.</exception>
    public static IEnumerable<KeyValuePair<string, EventActionValue<object?>>> ToPropertyUpdateEvents<T>(
        this T? obj,
        IEnumerable<KeyValuePair<string, object?>> properties,
        bool ignorePropertiesWithNonPublicSetter = false,
        bool ignoreInvalidProperties = false)
    {
        if (obj is null) yield break;

        var type = obj.GetType();
        if (type is null) yield break;

        foreach (var property in properties)
        {
            if (string.IsNullOrWhiteSpace(property.Key))
            {
                if (!ignoreInvalidProperties) throw new ArgumentException("property without name are not allowed", nameof(properties));
                continue;
            }

            var propertyInfo = type.GetProperty(property.Key);
            if (propertyInfo is null)
            {
                if (!ignoreInvalidProperties) throw new ArgumentException($"type {type.Name} has no property {property.Key}", nameof(properties));
                continue;
            }

            if (ignorePropertiesWithNonPublicSetter && !propertyInfo.CanWrite) continue;

            var value = propertyInfo.GetValue(obj);
            if (property.Value.EqualsNullable(value)) continue;

            yield return new KeyValuePair<string, EventActionValue<object?>>(property.Key, new EventActionValue<object?>(EventAction.Update, property.Value));
        }
    }

    /// <summary>
    /// Creates update events from a list of key values.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">An instance of an object of type <typeparamref name="T"/>.</param>
    /// <param name="keyValues">The members that should be updated. Could be properties and fields.</param>
    /// <param name="hasPublicPropertySetter">If true, an update event is only created when the property has a public setter. For fields the update event is always created.</param>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<string, EventActionValue<object?>>> ToMemberUpdateEvents<T>(
        this T? obj,
        IEnumerable<KeyValuePair<string, object?>> keyValues,
        bool hasPublicPropertySetter = false)
    {
        if (obj is null) yield break;

        var type = obj.GetType();
        if (type is null) yield break;

        foreach (var kv in keyValues)
        {
            if (string.IsNullOrWhiteSpace(kv.Key)) continue;

            if (!TryGetValue(type, obj, kv.Key, hasPublicPropertySetter, out var value)) continue;

            if (kv.Value.EqualsNullable(value)) continue;

            yield return new KeyValuePair<string, EventActionValue<object?>>(kv.Key, new EventActionValue<object?>(EventAction.Update, kv.Value));
        }
    }

    private static bool TryGetValue<T>(Type type, T obj, string memberName, bool considerPropertySetter, out object? value)
    {
        value = null;
        var propertyInfo = type.GetProperty(memberName);
        if (propertyInfo is not null)
        {
            if (considerPropertySetter && !propertyInfo.CanWrite) return false;

            value = propertyInfo.GetValue(obj);
            return true;
        }

        var fieldInfo = type.GetField(memberName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (fieldInfo is null) return false;

        value = fieldInfo.GetValue(obj);
        return true;
    }
}
