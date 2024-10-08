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
﻿using Foundation;
using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Linq.Expressions;

public static class ExpressionHelper
{
    public static bool AreEqualTerminators(Expression? lhs, Expression? rhs, bool ignoreNames)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return false;

        return AreEqualTerminators(lhs, rhs, false, ignoreNames);
    }

    public static bool AreEqualTerminators(Expression lhs, Expression rhs, bool same, bool ignoreNames)
    {
        if (lhs.NodeType != rhs.NodeType) return false;
        if (lhs.Type != rhs.Type) return false;

        return lhs switch
        {
            ConstantExpression l => rhs is ConstantExpression r && (same ? ReferenceEquals(l, r) : l.EqualsToExpression(r, ignoreNames)),
            MemberExpression l => rhs is MemberExpression r && (same ? ReferenceEquals(l, r) : l.EqualsToExpression(r, ignoreNames)),
            ParameterExpression l => rhs is ParameterExpression r && (same ? ReferenceEquals(l, r) : l.EqualsToExpression(r, ignoreNames)),
            _ => false
        };
    }

    public static bool AreSameTerminators(Expression? lhs, Expression? rhs)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return false;

        return AreEqualTerminators(lhs, rhs, true);
    }

    public static BinaryExpression? Concat(IEnumerable<BinaryExpression> expressions, ExpressionType binaryType)
    {
        return expressions.AggregateAsOption(x => x, (acc, x) => Expression.MakeBinary(binaryType, acc, x))
                          .TryGet(out var binaryExpression) ? binaryExpression : null;
    }

    //public static Expression<Func<T, object>> ConvertToReturnTypeObject<T>(MemberExpression member)
    //{
    //    var objectMember = Expression.Convert(member, typeof(object));

    //    return member.Expression is ParameterExpression parameter
    //        ? Expression.Lambda<Func<T, object>>(objectMember, parameter)
    //        : Expression.Lambda<Func<T, object>>(objectMember);
    //}

    //public static Expression<Func<T, object>> ConvertToReturnTypeObject<T, TMember>(Expression<Func<T, TMember>> expression)
    //{
    //    var converted = Expression.Convert(expression.Body, typeof(object));
    //    return Expression.Lambda<Func<T, object>>(converted, expression.Parameters);
    //}

    //public static int CreateHashCode(this Expression expression)
    //{
    //    return CreateHashCode(expression.Flatten());
    //}

    //public static int CreateHashCode(IEnumerable<Expression> expressions)
    //{
    //    return HashCode.FromOrderedHashCode(expressions.Select(x => x.GetExpressionHashCode()).ToArray());
    //}

    //public static IEnumerable<(int hashcode, Expression expression)> CreateHashCodeTuples(
    //    IEnumerable<Expression> expressions)
    //{
    //    foreach (var expr in expressions)
    //    {
    //        var builder = HashCode.CreateBuilder();

    //        switch (expr)
    //        {
    //            case BinaryExpression be:
    //                if (ExpressionType.Modulo == be.NodeType) break;

    //                builder.AddObject(be.NodeType);
    //                builder.AddObject(be.Type);

    //                yield return (builder.GetHashCode(), be);
    //                break;
    //            case ConstantExpression ce:
    //                builder.AddHashCode(CreateHashCode(ce));

    //                yield return (builder.GetHashCode(), ce);
    //                break;
    //            case LambdaExpression le:
    //                builder.AddObject(le.NodeType);
    //                builder.AddObject(le.Type);
    //                builder.AddObject(le.ReturnType);

    //                yield return (builder.GetHashCode(), le);
    //                break;
    //            case MemberExpression me:
    //                builder.AddHashCode(CreateHashCode(me));
    //                yield return (builder.GetHashCode(), me);
    //                break;
    //            case ParameterExpression pe:
    //                builder.AddHashCode(CreateHashCode(pe));
    //                yield return (builder.GetHashCode(), pe);
    //                break;
    //            case UnaryExpression ue:
    //                builder.AddHashCode(CreateHashCode(ue));
    //                yield return (builder.GetHashCode(), ue);
    //                break;
    //        };
    //    }
    //}


    public static string ExpressionToString(Expression expression, bool addSpace = false)
    {
        return ExpressionTypeToString(expression.NodeType, addSpace);
    }

    public static string ExpressionTypeToString(ExpressionType expressionType, bool addSpace = false)
    {
        var strType = expressionType.ToCsharpString();

        return addSpace ? $" {strType} " : strType;
    }

    /// <summary>
    /// Returns all MemberExpressions of a BinaryExpression. It checks Left and Right.
    /// Can be one, two or empty.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IEnumerable<MemberExpression> GetMemberExpressions(BinaryExpression expression)
    {
        if (expression.Left is MemberExpression left) yield return left;
        if (expression.Right is MemberExpression right) yield return right;
    }

    public static MemberInfo GetMemberInfo(Expression expression)
    {
        expression.ThrowIfNull();

        MemberExpression? me;
        if (expression is LambdaExpression lambda)
        {
            me = lambda.Body as MemberExpression;
            if (null == me)
            {
                if (lambda.Body is UnaryExpression unary)
                    me = unary.Operand as MemberExpression;
            }
        }
        else
            me = expression as MemberExpression;

        if (null == me)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a member expression");

        return me.Member;
    }

    public static MemberInfo GetMemberInfoFromLambda(Expression expression)
    {
        expression.ThrowIfNull();

        if (expression is not LambdaExpression lambda)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a lambda");

        if (lambda.Body is not MemberExpression me)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a member expression");

        return me.Member;
    }

    /// <summary>
    /// Returns a ParameterExpression. It checks Left and Right.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>A ParameterExpression or null.</returns>
    public static IEnumerable<ParameterExpression> GetParameterExpressions(Expression expression)
    {
        expression.ThrowIfNull();

        if (expression is LambdaExpression lambda)
            expression = lambda.Body;

        if (expression is BinaryExpression binary)
        {
            foreach (var left in GetParameterExpressions(binary.Left))
                yield return left;

            foreach (var right in GetParameterExpressions(binary.Right))
                yield return right;

            yield break;
        }

        if (expression is ParameterExpression parameter)
            yield return parameter;

        if (expression is MemberExpression member && member.Expression is ParameterExpression p)
            yield return p;
    }

    public static object GetValue(MemberExpression member)
    {
        member.ThrowIfNull();

        var objectMember = Expression.Convert(member, typeof(object));
        var getterLambda = Expression.Lambda<Func<object>>(objectMember);
        var getter = getterLambda.Compile();

        return getter();
    }

    public static string NameOf(Expression<Func<object>> expression)
    {
        expression.ThrowIfNull();

        var mi = GetMemberInfo(expression);
        if (null == mi)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a member access");

        return mi.Name;
    }

    public static IEnumerable<Expression> Sort(this IEnumerable<Expression> expressions)
    {
        return expressions.OrderBy(x => x.GetExpressionHashCode());
    }
}
