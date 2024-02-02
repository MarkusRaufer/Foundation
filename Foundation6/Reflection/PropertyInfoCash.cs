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
﻿using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public class PropertyInfoCash : MemberCash<PropertyInfo>
{
    public PropertyInfoCash(Type type) : base(type)
    {
    }

    public PropertyInfoCash(Type type, PropertyInfo[] members) : base(type, members)
    {
    }

    public PropertyInfoCash(Type type, string[] memberNames) : base(type, memberNames)
    {
    }

    protected override PropertyInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if (null == member) throw new InvalidArgumentException($"{lambda.Body}");

        return member.Member is PropertyInfo pi
            ? pi
            : throw new InvalidArgumentException($"{member.Member}");
    }

    protected override IEnumerable<PropertyInfo> GetTypeMembers() => Type.GetProperties();
}

public class PropertyInfoCash<T> : MemberCash<T, PropertyInfo>
{
    public PropertyInfoCash() : base()
    {
    }

    public PropertyInfoCash(PropertyInfo[] members) : base(members)
    {
    }
    
    public PropertyInfoCash(string[] memberNames) : base(memberNames)
    {
    }

    public PropertyInfoCash(params Expression<Func<T, object>>[] members) : base(members)
    {
    }

    protected override PropertyInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if (null == member) throw new InvalidArgumentException($"{lambda.Body}");

        return member.Member is PropertyInfo pi 
            ? pi 
            : throw new InvalidArgumentException($"{member.Member}");
    }

    protected override IEnumerable<PropertyInfo> GetTypeMembers() => typeof(T).GetProperties();
}
