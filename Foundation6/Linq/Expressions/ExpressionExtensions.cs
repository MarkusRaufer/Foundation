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
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ExpressionExtensions
{
    public static bool EqualsToExpression(this Expression? lhs, Expression? rhs, bool ignoreNames = true)
    {
        if (null == lhs) return null == rhs;
        if (rhs == null) return false;

        return lhs switch
        {
            BinaryExpression l => rhs is BinaryExpression r && l.EqualsToExpression(r, ignoreNames),
            ConstantExpression l => rhs is ConstantExpression r && l.EqualsToExpression(r),
            LambdaExpression l => rhs is LambdaExpression r && l.EqualsToExpression(r, ignoreParameterNames: ignoreNames),
            MemberExpression l => rhs is MemberExpression r && l.EqualsToExpression(r),
            ParameterExpression l => rhs is ParameterExpression r && l.EqualsToExpression(r, ignoreName: ignoreNames),
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
        if(null == expression) return 0;

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

    /// <summary>
    /// Returns parameters of the expression. If expression does not include any parameter an empty list is returned.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IEnumerable<ParameterExpression> GetParameters(this Expression expression)
    {
        expression.ThrowIfNull();
        switch(expression)
        {
            case BinaryExpression be:
                foreach (var p in GetParameters(be.Left).Concat(GetParameters(be.Right)).Distinct())
                    yield return p;

                break;
            case LambdaExpression lambda:
                foreach (var p in lambda.Parameters)
                    yield return p;

                break;
            case MethodCallExpression mc:
                foreach (var p in mc.GetParameters())
                    yield return p;
                break;
            case MemberExpression e:
                {
                    var p = e.GetParameter();
                    if (null != p) yield return p;
                }
                break;
            case ParameterExpression p:
                yield return p;
                break;
            case UnaryExpression ue:
                {
                    var p = ue.GetParameter();
                    if(null != p) yield return p;
                }
                break;
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
            ExpressionType.Add or
            ExpressionType.Constant or
            ExpressionType.Divide or
            ExpressionType.MemberAccess or
            ExpressionType.Parameter or
            ExpressionType.Subtract => true,
            ExpressionType.Convert or
            ExpressionType.Negate => exp is UnaryExpression unary && isTerminalNode(unary.Operand),
            ExpressionType.Modulo => exp is BinaryExpression be && isTerminalNode(be.Left) && isTerminalNode(be.Right),
            _ => false
        };
    }
}
