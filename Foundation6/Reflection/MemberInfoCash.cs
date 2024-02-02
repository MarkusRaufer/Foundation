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

public class MemberInfoCash : MemberCash<MemberInfo>
{
    public MemberInfoCash(Type type) : base(type, type.GetProperties())
    {
    }

    public MemberInfoCash(Type type, MemberInfo[] members) : base(type, members)
    {
    }

    public MemberInfoCash(Type type, string[] memberNames) : base(type, memberNames)
    {
    }

    protected override MemberInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if (null == member) throw new InvalidArgumentException($"{lambda}");

        return member.Member;
    }

    protected override IEnumerable<MemberInfo> GetTypeMembers() => Type.GetMembers();
}

public class MemberInfoCash<T> : MemberCash<T, MemberInfo>
{
    public MemberInfoCash() : base()
    {
    }

    public MemberInfoCash(MemberInfo[] members) : base(members)
    {
    }

    public MemberInfoCash(string[] memberNames) : base(memberNames)
    {
    }

    public MemberInfoCash(params Expression<Func<T, object>>[] members) : base(members)
    {
    }

    protected override MemberInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if(null == member) throw new InvalidArgumentException($"{lambda}");

        return member.Member;
    }

    protected override IEnumerable<MemberInfo> GetTypeMembers() => typeof(T).GetMembers();
}

