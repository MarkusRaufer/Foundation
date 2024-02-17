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
﻿namespace Foundation.Reflection;

using System.Reflection;
public static class MemberInfoExtensions
{
    public static bool EqualsToMemberInfo(this MemberInfo lhs, MemberInfo rhs, bool ignoreName = false)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return false;
        return ignoreName
            ? lhs.MemberType == rhs.MemberType && lhs.DeclaringType == rhs.DeclaringType
            : lhs.MemberType == rhs.MemberType && lhs.Name == rhs.Name && lhs.DeclaringType == rhs.DeclaringType;
    }

    public static Type GetMemberType(this MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            FieldInfo fi => fi.FieldType,
            MethodInfo mi => mi.ReturnType,
            PropertyInfo pi => pi.PropertyType,
            _ => throw new NotSupportedException()
        };
    }

#nullable enable
    public static object? GetValue(this MemberInfo memberInfo, object? obj)
    {
        memberInfo.ThrowIfNull();
        if (null == obj) return null;

        return memberInfo switch
        {
            FieldInfo fi => fi.GetValue(obj),
            MethodInfo mi => mi.GetValue(obj),
            PropertyInfo pi => pi.GetValue(obj),
            _ => null
        };
    }
#nullable restore

    public static void SetValue(this MemberInfo memberInfo, object obj, object value)
    {
        switch(memberInfo)
        {
            case FieldInfo fi: fi.SetValue(obj, value); break;
            case MethodInfo mi: mi.SetValue(obj, value); break;
            case PropertyInfo pi: pi.SetValue(obj, value); break;
        }
    }
}

