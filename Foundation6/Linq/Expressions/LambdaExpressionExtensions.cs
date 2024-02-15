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
// The above copyright notice and this permission notice shall binaryExpression included in all
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

namespace Foundation.Linq.Expressions;

public static class LambdaExpressionExtensions
{
    public static bool EqualsToExpression(this LambdaExpression lhs, LambdaExpression rhs, bool ignoreParameterNames = true)
    {
        if (null == lhs) return null == rhs;
        if (null== rhs) return false;

        return null != rhs
            && lhs.NodeType == rhs.NodeType
            && lhs.Type == rhs.Type
            && lhs.ReturnType == rhs.ReturnType
            && lhs.Parameters.SequenceEqual(rhs.Parameters, (l, r) => l.EqualsToExpression(r, ignoreParameterNames))
            && lhs.Body.EqualsToExpression(rhs.Body);
    }

    public static IEnumerable<BinaryExpression> GetBinaryExpressions(this LambdaExpression lambda)
    {
        if (lambda.Body is not BinaryExpression binaryExpression) yield break;

        yield return binaryExpression;

        foreach(var be in binaryExpression.GetBinaryExpressions())
            yield return be;
    }
    
    public static int GetExpressionHashCode(this LambdaExpression? expression, bool ignoreParameterNames = true)
    {
        if(expression == null) return 0;

        var hashCodes = new List<int>
        {
            expression.NodeType.GetHashCode(),
            expression.Type.GetHashCode(),
            expression.ReturnType.GetHashCode(),
            expression.Body.GetExpressionHashCode(ignoreParameterNames)
        };
        expression.Parameters.Select(x => x.GetExpressionHashCode(ignoreParameterNames))
                             .ForEach(hashCodes.Add);
        
        return HashCode.FromOrderedHashCodes(hashCodes);
    }

    public static MemberExpression? GetMember(this LambdaExpression lambda)
    {
        return lambda.Body switch
        {
            MemberExpression me => me,
            UnaryExpression ue => ue.Operand as MemberExpression,
            _ => null,
        };
    }

    public static IEnumerable<Type> GetUniqueParameterTypes(this LambdaExpression lambda)
        => lambda.Parameters.Select(x => x.Type).Distinct();
    
    public static bool IsPredicate(this LambdaExpression lambda)
    {
        return lambda.ReturnType == typeof(bool);
    }

    public static Delegate ToDelegate(this LambdaExpression lambda)
    {
        return (Delegate)lambda.Compile();
    }

    public static Predicate<T> ToPredicate<T>(this LambdaExpression lambda)
    {
        if(1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), "expects exact one parameter");
        
        if (!lambda.IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), "expects a boolean as return type");

        var func = (Func<T, bool>)lambda.Compile();

        return new Predicate<T>(func);
    }
}
