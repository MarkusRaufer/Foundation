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
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.ComponentModel;


public interface IObjectEventBuilder<T, TBuilder>
    where TBuilder : IObjectEventBuilder<T, TBuilder>
{
    TBuilder And(Expression<Func<T, object>> propertySelector, object? newValue);
    TBuilder And(IEnumerable<KeyValuePair<string, object?>> properties);
    IEnumerable<KeyValuePair<string, EventActionValue<object?>>> Build(bool ignorePropertiesWithNonPublicSetter = false, bool ignoreInvalidProperties = true);
}

public struct ObjectEventBuilder<T> : IObjectEventBuilder<T, ObjectEventBuilder<T>>
{
    private readonly Dictionary<string, object?> _properties = [];
    private readonly T _source;

    private record struct PropertyValue(object? Value, bool HasPublicSetter);

    public ObjectEventBuilder(T source, Expression<Func<T, object>> propertySelector, object? newValue)
    {
        _source = source.ThrowIfNull();

        AddKeyValue(propertySelector.ThrowIfNull(), newValue);
    }

    public ObjectEventBuilder(T source, IEnumerable<KeyValuePair<string, object?>> properties)
    {
        _source = source.ThrowIfNull();

        foreach (var kvp in properties)
            AddProperty(kvp.Key, kvp.Value);
    }

    private readonly void AddKeyValue(Expression<Func<T, object>> propertySelector, object? newValue)
    {
        var propertyName = GetPropertyName(propertySelector);

        AddProperty(propertyName, newValue);
    }

    private readonly void AddProperty(string name, object? value)
    {
        _properties.Add(name, value);
    }

    public readonly ObjectEventBuilder<T> And(Expression<Func<T, object>> propertySelector, object? newValue)
    {
        AddKeyValue(propertySelector.ThrowIfNull(), newValue);
        return this;
    }

    public ObjectEventBuilder<T> And(IEnumerable<KeyValuePair<string, object?>> properties)
    {
        foreach (var property in properties)
            AddProperty(property.Key, property.Value);

        return this;
    }

    /// <summary>
    /// Creates a list of update events.
    /// </summary>
    /// <param name="ignorePropertiesWithNonPublicSetter"></param>
    /// <param name="ignoreInvalidProperties"></param>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<string, EventActionValue<object?>>> Build(bool ignorePropertiesWithNonPublicSetter = false, bool ignoreInvalidProperties = true)
    {

        return _source.ToPropertyUpdateEvents(_properties, ignorePropertiesWithNonPublicSetter, ignoreInvalidProperties);
    }

    private static MemberExpression GetMember(LambdaExpression lambda)
    {
        var memberExpression = lambda.Body as MemberExpression;
        if (memberExpression is not null) return memberExpression;

        if (lambda.Body is UnaryExpression ue)
        {
            if (ue.Operand is MemberExpression member) return member;
        }

        throw new ArgumentException($"{nameof(lambda)} must be a {nameof(MemberExpression)}", nameof(lambda));
    }

    private static string GetPropertyName(Expression<Func<T, object>> propertySelector)
    {
        var member = GetMember(propertySelector);
        if (member.Member is not PropertyInfo property) throw new ArgumentException("expression must target a property", nameof(propertySelector));
        
        return property.Name;
    }
}
