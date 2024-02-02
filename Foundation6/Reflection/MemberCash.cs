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
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public abstract class MemberCash<TInfo> : MemberCashBase<TInfo>
    where TInfo : MemberInfo
{
    public MemberCash(Type type)
    {
        Type = type.ThrowIfNull();
    }

    public MemberCash(Type type, TInfo[] members) : this(type)
    {
        Members = members.ThrowIfEmpty();
    }

    public MemberCash(Type type, string[] memberNames) : this(type)
    {
        MemberNames = memberNames.ThrowIfEmpty();
    }

    public Type Type { get; }
}

public abstract class MemberCash<T, TInfo> : MemberCashBase<TInfo>
    where TInfo : MemberInfo
{
    public MemberCash()
    {
    }

    public MemberCash(TInfo[] members)
    {
        Members = members.ThrowIfEmpty();
    }

    public MemberCash(string[] memberNames)
    {
        MemberNames = memberNames.ThrowIfEmpty();
    }

    public MemberCash(params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        Members = members.Cast<LambdaExpression>()
                         .Select(GetMemberFromLambda)
                         .Where(MemberFilter)
                         .ToArray();
    }
}
