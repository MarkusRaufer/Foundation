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

namespace Foundation.Reflection
{
    public static class ReflectionHelper
    {
        public static IEnumerable<MemberInfo> GetAllInterfaceMembers(Type type, bool includeSelf = false)
        {
            if(includeSelf)
            {
                foreach(var member in type.GetMembers())
                    yield return member;
            }

            foreach(var interfaceType in type.GetInterfaces())
            {
                foreach(var member in interfaceType.GetMembers())
                    yield return member;
            }
        }

        public static IEnumerable<MethodInfo> GetAllInterfaceMethods(Type type, bool includeSelf = false)
        {
            if (includeSelf)
            {
                foreach (var method in type.GetMethods())
                    yield return method;
            }

            foreach (var interfaceType in type.GetInterfaces())
            {
                foreach (var method in interfaceType.GetMethods())
                    yield return method;
            }
        }

        public static FieldInfo? GetBackingField(PropertyInfo property)
        {
            if (property.DeclaringType is null) return null;

            return GetBackingField(property.DeclaringType, property.Name);
        }

        public static FieldInfo? GetBackingField<T>(string propertyName) => GetBackingField(typeof(T), propertyName);

        public static FieldInfo? GetBackingField(this Type type, string propertyName)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            return type.GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type type, IEnumerable<string> memberNames)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            return GetMembers(type,
                              BindingFlags.Public
                            | BindingFlags.NonPublic
                            | BindingFlags.Instance
                            | BindingFlags.FlattenHierarchy,
                              memberNames);
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type type, BindingFlags bindingFlags, IEnumerable<string> memberNames)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            return memberNames.FilterMap(name => type.GetMember(name).FirstAsOption());
        }

        public static IEnumerable<FieldInfo> GetMutableFields(this Type type)
        {
            type.ThrowIfNull();

            return type.GetFields().Where(f => f.IsInitOnly);
        }

        public static IEnumerable<PropertyInfo> GetMutableProperties(this Type type)
        {
            type.ThrowIfNull();

            return type.GetProperties().Where(p => p.CanWrite);
        }

        public static IEnumerable<MemberInfo> GetMutableProperties<T>()
        {
            return GetMutableProperties(typeof(T));
        }

        public static object? GetValue(object obj, MemberInfo memberInfo)
        {
            obj.ThrowIfNull();
            return memberInfo switch
            {
                FieldInfo fi => fi.GetValue(obj),
                PropertyInfo pi => pi.GetValue(obj),
                _ => throw new NotImplementedException(nameof(memberInfo))
            };
        }

        public static IEnumerable<KeyValuePair<MemberInfo, object?>> GetValues(object obj, params MemberInfo[] memberInfos)
        {
            foreach (var memberInfo in memberInfos)
            {
                var value = GetValue(obj, memberInfo);
                yield return Pair.New(memberInfo, value);
            }
        }

        public static object SetValue(object obj, string memberName, object value)
        {
            obj.ThrowIfNull();
            memberName.ThrowIfNullOrWhiteSpace();

            return SetValue(obj.ThrowIfNull(), 
                            memberName, 
                            BindingFlags.Public
                          | BindingFlags.NonPublic
                          | BindingFlags.Instance
                          | BindingFlags.FlattenHierarchy, value);
        }

        public static object SetValue(object obj, string memberName, BindingFlags bindingFlags, object value)
        {
            obj.ThrowIfNull();
            memberName.ThrowIfNullOrWhiteSpace();

            var members = obj.GetType().GetMember(memberName, bindingFlags);

            members.ThrowIfOutOfRange(() => 1 != memberName.Length);

            if (1 != members.Length) throw new ArgumentOutOfRangeException(nameof(memberName));

            return SetValue(obj, members[0], value);
        }

        public static object SetValue(object? obj, MemberInfo memberInfo, object value)
        {
            obj.ThrowIfNull();
            memberInfo.ThrowIfNull();

            switch (memberInfo)
            {
                case FieldInfo fi:
                    if(!fi.IsInitOnly) fi.SetValue(obj, value);
                    break;
                case PropertyInfo pi:
                    if (pi.CanWrite)
                    {
                        pi.SetValue(obj, value);
                        break;
                    }
                        
                    var backingField = GetBackingField(obj!.GetType(), pi.Name);
                    backingField?.SetValue(obj, value);
                    break;
            }

            return obj!;
        }
    }
}
