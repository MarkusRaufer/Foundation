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
using Foundation.Reflection;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class MemberExpressionExtensions
{
    public static bool EqualsToExpression(this MemberExpression lhs, MemberExpression rhs, bool ignoreName)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return false;

        return lhs.NodeType == rhs.NodeType 
            && lhs.Member.EqualsToMemberInfo(rhs.Member, ignoreName)
            && lhs.Expression.EqualsToExpression(rhs.Expression, ignoreName);
    }

    public static IEnumerable<MemberExpression> DistinctMembers(this IEnumerable<MemberExpression> members)
    {
        var hs = new HashSet<MemberExpression>(MemberExpressionHelper.CreateExpressionEqualityComparer());
        members.ForEach(x => hs.Add(x));
        return hs;
    }

    public static int GetExpressionHashCode(this MemberExpression? expression, bool ignoreName = false)
    {
        if (expression is null) return 0;
        return (ignoreName)
            ? HashCode.FromOrderedObject(expression.Member, expression.Type)
            : HashCode.FromOrderedObject<object>(expression.Member, expression.Member.Name, expression.Type);
    }

    public static IEnumerable<ParameterExpression> GetParameters(this MemberExpression member)
    {
        if (member.Expression is ParameterExpression p)
        {
            yield return p;
            yield break;
        }

        if (member.Expression is MemberExpression me)
        {
            foreach(var parameter in GetParameters(me))
                yield return parameter;

            yield break;
        }

        if (member.Expression is MethodCallExpression mc)
        {
            foreach (var parameter in mc.GetParameters())
                yield return parameter;
        }
    }
}
