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
using Foundation.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.ComponentModel;


public interface IExistingObjectBuilder<T, TBuilder>
    where TBuilder : IExistingObjectBuilder<T, TBuilder>
{
    TBuilder And(Expression<Func<T, object>> propertySelector, object? newValue);
    TBuilder And(IEnumerable<KeyValuePair<string, object?>> properties);
    T Build(Action<IDictionary<string, object?>> trackedChanges);
}

public class ExistingObjectBuilder
{
    public enum BuildMode
    {
        ChangeWith,
        NewWith,
    };
}

public struct ExistingObjectBuilder<T> : IExistingObjectBuilder<T, ExistingObjectBuilder<T>>
{
    private readonly ExistingObjectBuilder.BuildMode _buildMode;
    private static readonly KeyValuePair<string, object?> _empty = default;
    private Dictionary<string, object?> _properties = [];
    private readonly T _source;

    public ExistingObjectBuilder(ExistingObjectBuilder.BuildMode mode, T source, Expression<Func<T, object>> propertySelector, object? newValue)
    {
        _buildMode = mode;
        _source = source.ThrowIfNull();

        AddKeyValue(_source, propertySelector.ThrowIfNull(), newValue);
    }

    public ExistingObjectBuilder(ExistingObjectBuilder.BuildMode mode, T source, IEnumerable<KeyValuePair<string, object?>> properties)
    {
        _buildMode = mode;
        _source = source.ThrowIfNull();

        foreach (var property in properties)
            AddProperty(property);            
    }

    private void AddKeyValue(T source, Expression<Func<T, object>> propertySelector, object? newValue)
    {
        var keyValue = ToKeyValue(_source, propertySelector, newValue);
        if (keyValue.Equals(_empty)) return;

        AddProperty(keyValue);
    }

    private void AddProperty(KeyValuePair<string, object?> property)
    {
        _properties.Add(property.Key, property.Value);
    }

    public ExistingObjectBuilder<T> And(Expression<Func<T, object>> propertySelector, object? newValue)
    {
        AddKeyValue(_source, propertySelector.ThrowIfNull(), newValue);
        return this;
    }

    public ExistingObjectBuilder<T> And(IEnumerable<KeyValuePair<string, object?>> properties)
    {
        foreach (var property in properties)
            AddProperty(property);

        return this;
    }

    public T Build(Action<IDictionary<string, object?>> trackedChanges)
    {
        var type = typeof(T);

        return _buildMode switch
        {
            ExistingObjectBuilder.BuildMode.ChangeWith => ChangeObject(type, trackedChanges),
            ExistingObjectBuilder.BuildMode.NewWith => CreateNewObject(type, trackedChanges),
            _ => throw new NotImplementedException(_buildMode.ToString())
        };
    }

    private T ChangeObject(Type type, Action<IDictionary<string, object?>> trackedChanges)
    {
        Dictionary<string, object?> changes = [];

        foreach (var (name, newValue) in _properties)
        {
            var property = type.GetProperty(name);
            if (property == null) throw new ArgumentException($"property {name} does not exist");

            var value = property.GetValue(_source);
            if (value.EqualsNullable(newValue)) continue;

            property.SetValue(_source, newValue);
            changes.Add(property.Name, newValue);
        }

        if (changes.Count > 0) trackedChanges(changes);

        return _source;
    }

    private T CreateNewObject(Type type, Action<IDictionary<string, object?>> trackedChanges)
    {
        var (ctor, args) = GetCtorWithArguments(type);

        var ctorArgs = args.ToArray();

        var (ctorProperties, properties) = type.GetProperties()
                                               .Partition(x => ctorArgs.Contains(x.Name),
                                                    x => x.ToArray(),
                                                    x => x.ToArray());

        Dictionary<string, object?> changes = [];
        var ctorValues = GetValues(ctorProperties, changes).ToArray();

        var newInstance = (T)ctor.Invoke([.. ctorValues]);


        foreach (var property in properties)
        {
            var value = property.GetValue(_source);
            if (!_properties.TryGetValue(property.Name, out var newValue))
            {
                property.SetValue(newInstance, value);
                continue;
            }

            if (value.EqualsNullable(newValue)) property.SetValue(newInstance, value);
            else
            {
                property.SetValue(newInstance, newValue);

                changes.Add(property.Name, newValue);
            }
        }

        if (changes.Count > 0) trackedChanges(changes);

        return newInstance;
    }

    private IEnumerable<string> GetArguments(Type type, ConstructorInfo ctor)
    {
        var paramters = ctor.GetParameters();

        foreach (var property in type.GetProperties())
        {
            if (paramters.Any(x => string.Equals(x.Name, property.Name, StringComparison.InvariantCultureIgnoreCase)))
                yield return property.Name;
        }
    }

    private (ConstructorInfo ctor, IEnumerable<string> args) GetCtorWithArguments(Type type)
    {
        var ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
        return (ctor, GetArguments(type, ctor));
    }

    private static PropertyInfo GetPropertyInfo(Expression<Func<T, object>> propertySelector)
    {
        propertySelector.ThrowIfNull();

        var member = GetMember(propertySelector);
        
        if (member.Member is not PropertyInfo property)
            throw new ArgumentException("expression must target a property", nameof(propertySelector));

        return property;
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
    private IEnumerable<object?> GetValues(PropertyInfo[] properties, Dictionary<string, object?> changes)
    {
        foreach (var property in properties)
        {
            var value = property.GetValue(_source);
            if (!_properties.TryGetValue(property.Name, out var newValue))
            {
                yield return value;
                continue;
            }

            if (value.EqualsNullable(newValue))
            {
                yield return value;
                continue;
            }

            changes.Add(property.Name, newValue);
            yield return newValue;
        }
    }

    private KeyValuePair<string, object?> ToKeyValue(T source, Expression<Func<T, object>> propertySelector, object? newValue)
    {
        var property = GetPropertyInfo(propertySelector.ThrowIfNull());

        if (_buildMode == ExistingObjectBuilder.BuildMode.ChangeWith
            && !property.CanWrite)
            throw new ArgumentException($"property {property.Name} has no setter");

        var value = property.GetValue(source);
        if (value.EqualsNullable(newValue)) return default;

        return new KeyValuePair<string, object?>(property.Name, newValue);
    }
}
