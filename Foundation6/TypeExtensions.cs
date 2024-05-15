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
﻿namespace Foundation;

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

public static class TypeExtensions
{
    private readonly static Type _anyType = typeof(Any);
    private readonly static Type _isExternalInitType = typeof(IsExternalInit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare(this Type type, Type other, Func<Type, IComparable> selector)
    {
        return selector(type).CompareTo(selector(other));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(this Type type, Type other, Func<Type, T> selector)
        where T : IComparable<T>
    {
        return selector(type).CompareTo(selector(other));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type CreateGenericType(this Type objectType, params Type[] genericTypeArguments)
    {
        if (!objectType.IsGenericType)
            throw new ArgumentException("type is not a generic type");

        if (null == genericTypeArguments || genericTypeArguments.Length == 0)
            throw new ArgumentException("genericTypeArguments must have at least 1 type argument");

        return objectType.MakeGenericType(genericTypeArguments);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type? GetAssignableInterface<T>(this Type self)
    {
        var interfaceType = typeof(T);
        return GetAssignableInterface(self, interfaceType);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type? GetAssignableInterface(this Type self, Type interfaceType)
    {
        foreach (var type in self.GetInterfaces())
        {
            if (HasInterface(type, interfaceType))
                return type;
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? GetDefault(this Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type);

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<Type> GetGenericInterfaceTypeArguments(this Type self, Type interfaceType)
    {
        foreach (var type in self.GetInterfaces())
        {
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                if (genericTypeDef == interfaceType)
                {
                    var arg = type.GenericTypeArguments[0];
                    yield return arg;
                }
            }
        }
    }

    /// <summary>
    /// Returns the name of the type without generics.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetNameWithoutGenericArity(this Type type)
    {
        var sb = new StringBuilder();
        foreach (var ch in type.Name)
        {
            if ('`' == ch) break;

            sb.Append(ch);
        }
        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<Type> GetNestedParentTypes(this Type? type)
    {
        if (null == type) throw new ArgumentNullException(nameof(type));
        if (!type.IsNested) yield break;

        if (type.DeclaringType is Type declType)
            yield return declType;

        foreach (var parent in GetNestedParentTypes(type.DeclaringType))
        {
            yield return parent;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasAnyGenericArgument(this Type type)
    {
        var args = type.GetGenericArguments();
        if (0 == args.Length) return false;

        return _anyType == args[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasInterface<T>(this Type type)
    {
        return HasInterface(type, typeof(T));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasInterface(this Type type, Type @interface)
    {
        return @interface.IsAssignableFrom(type);
    }

    /// <summary>
    /// Returns true, if type implements the interface type.
    /// </summary>
    /// <param name="type">The type, which implements a specific interface.</param>
    /// <param name="interfaceType">Type of interface. You can also use generic interface types E.g. typeof(IEquatable{})</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ImplementsInterface(this Type type, Type interfaceType)
    {
        foreach (var iface in type.GetInterfaces())
        {
            if(interfaceType.IsGenericTypeDefinition && iface.IsGenericType && interfaceType == iface.GetGenericTypeDefinition()) return true;

            if (interfaceType == iface) return true;
        }

        return false;
    }

    /// <summary>
    /// Returns all inherted types of type. If includingInterfaces is true all implemented interfaces are also returned.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="includingInterfaces">If true all implemented interfaces are also returned.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<Type> GetInheritedTypes(this Type type, bool includingInterfaces = false)
    {
        foreach (var t in type.Assembly.GetTypes())
        {
            if (t.IsSubclassOf(type)) yield return t;
        }

        if(!includingInterfaces) yield break;

        foreach (var i in type.GetInterfaces())
            yield return i;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAction(Type? type)
    {
        if (null == type) return false;

        if (type == typeof(Action)) return true;

        var generic = type switch
        {
            Type { IsGenericTypeDefinition: true } => type,
            Type { IsGenericType: true } => type.GetGenericTypeDefinition(),
            _ => null
        };

        return generic == typeof(Action<>);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEnumerable(this Type type)
    {
        return (typeof(string) != type) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLambda(this Type? type)
    {
        var generic = type switch
        {
            Type { IsGenericTypeDefinition: true } => type,
            Type { IsGenericType: true } => type.GetGenericTypeDefinition(),
            _ => null
        };

        if (generic == null) return false;
        if (generic.Namespace != "System") return false;
        if (generic.BaseType != typeof(MulticastDelegate)) return false;
        return generic.Name.StartsWith("Func`");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsImmutable(this Type? type)
    {
        type.ThrowIfNull();

        if (!type.GetFields().Where(x => !x.IsStatic && x != type).All(x => x.IsInitOnly)) return false;

        foreach (var property in type.GetProperties())
        {
            if (property.PropertyType == type) continue;
            if (isStatic(property)) continue;

            if (!property.CanWrite) continue;

            if (property.SetMethod == null || !property.SetMethod.IsPublic) continue;

            var types = property.SetMethod.ReturnParameter.GetRequiredCustomModifiers();
            if (!types.Contains(_isExternalInitType)) return false;
        }

        return true;

        static bool isStatic(PropertyInfo pi)
        {
            if (pi.GetMethod is MethodInfo m && m.IsStatic) return true;

            return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNumeric(this Type? type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.SByte:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOfGenericType(this Type type, Type other)
    {
        if (!type.IsGenericType || !other.IsGenericType) return false;

        return type.GetGenericTypeDefinition() == other.GetGenericTypeDefinition();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOfGenericType<T>(this Type type)
    {
        return IsOfGenericType(type, typeof(T));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsScalar(this Type? type)
    {
        type.ThrowIfNull();

        return type!.IsPrimitive || TypeHelper.ScalarTypes(true).Any(x => x == type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsScalarArrayType(this Type? type)
    {
        type.ThrowIfNull();

        if (!type.IsArray) return false;

        return type.GetElementType().IsScalar();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsScalarEnumerableType(this Type? type)
    {
        type.ThrowIfNull();

        if (!type!.IsEnumerable()) return false;
        if (!type.IsGenericType) return false;

        return type.GenericTypeArguments.All(t => t.IsScalar());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToGenericsString(this Type? type)
    {
        type.ThrowIfNull();

        if (!type!.IsGenericTypeDefinition && !type.IsGenericType)
            return type.ToString();

        var name = type.Name.Split('`').First();
        var sb = new StringBuilder();
        sb.Append(name);
        sb.Append('<');
        var args = type.GetGenericArguments();

        var i = 0;
        foreach (var argType in args)
        {
            if (i > 0)
                sb.Append(',');

            sb.Append(argType.Name);
            i++;
        }
        sb.Append('>');
        return sb.ToString();
    }
}

