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

public abstract class MemberCashBase<TInfo> where TInfo : MemberInfo
{
    protected abstract TInfo GetMemberFromLambda(LambdaExpression lambda);

    public IEnumerable<TInfo> GetMembers()
    {
        if (null != Members) return Members;

        var members = GetTypeMembers();
        if (null == MemberNames)
        {
            Members = null == MemberFilter
                ? members.ToArray()
                : members.Where(MemberFilter).ToArray();

            return Members;
        }
        Members = members.Where(member => MemberNames.Contains(member.Name)).ToArray();

        return Members;
    }

    /// <summary>
    /// Return type of meta information you want. Like PropertyInfo, FieldInfo,...
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<TInfo> GetTypeMembers();

    protected Func<TInfo, bool> MemberFilter { get; set; } = _ => true;

    protected string[]? MemberNames { get; set; }

    protected TInfo[]? Members { get; set; }
}
