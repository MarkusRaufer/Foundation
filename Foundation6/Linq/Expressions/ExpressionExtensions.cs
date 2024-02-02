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
﻿using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ExpressionExtensions
{
    public static bool EqualsToExpression(this Expression lhs, Expression rhs)
    {
        return lhs switch
        {
            BinaryExpression l => rhs is BinaryExpression r && l.EqualsToExpression(r),
            ConstantExpression l => rhs is ConstantExpression r && l.EqualsToExpression(r),
            LambdaExpression l => rhs is LambdaExpression r && l.EqualsToExpression(r),
            MemberExpression l => rhs is MemberExpression r && l.EqualsToExpression(r),
            ParameterExpression l => rhs is ParameterExpression r && l.EqualsToExpression(r),
            UnaryExpression l => rhs is UnaryExpression r && l.EqualsToExpression(r),
            _ => false
        };
    }

    public static IEnumerable<Expression> Flatten(this Expression expression)
    {
        var flattener = new ExpressionTreeFlattener();
        return flattener.Flatten(expression);
    }

    public static int GetExpressionHashCode(this Expression expression, bool ignoreName = true)
    {
        expression.ThrowIfNull();
        return expression switch
        {
            BinaryExpression e    => e.GetExpressionHashCode(ignoreName),
            ConstantExpression e  => e.GetExpressionHashCode(),
            LambdaExpression e    => e.GetExpressionHashCode(ignoreName),
            MemberExpression e    => e.GetExpressionHashCode(),
            ParameterExpression e => e.GetExpressionHashCode(ignoreName),
            UnaryExpression e     => e.GetExpressionHashCode(ignoreName),
            _ => 0
        };
    }

    public static bool IsConstant(this Expression expression)
    {
        return expression.NodeType == ExpressionType.Constant;
    }

    public static bool IsConvert(this Expression expression)
    {
        return expression.NodeType == ExpressionType.Convert;
    }

    public static bool IsPredicate(this Expression expression)
    {
        return expression is LambdaExpression lambda
            && lambda.Body is BinaryExpression binaryExpression
            && binaryExpression.IsPredicate();
    }

    public static bool IsTerminal(this Expression expression)
    {
        if(isTerminalNode(expression)) return true;

        if (expression is not BinaryExpression binary) return false;

        return isTerminalNode(binary.Left) && isTerminalNode(binary.Right);

        static bool isTerminalNode(Expression exp) => exp.NodeType switch
        {
            ExpressionType.Constant or
            ExpressionType.MemberAccess or
            ExpressionType.Parameter => true,
            ExpressionType.Convert => exp is UnaryExpression unary && isTerminalNode(unary.Operand),
            ExpressionType.Modulo => exp is BinaryExpression be && isTerminalNode(be.Left) && isTerminalNode(be.Right),
            _ => false
        };
    }
}
