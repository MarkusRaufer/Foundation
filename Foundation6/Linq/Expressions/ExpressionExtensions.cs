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
    public static bool EqualsToExpression(this Expression? lhs, Expression? rhs, bool ignoreNames = false)
    {
        if (null == lhs) return null == rhs;
        if (rhs == null) return false;

        return lhs switch
        {
            LambdaExpression l => rhs is LambdaExpression r && l.EqualsToExpression(r, ignoreNames),
            BinaryExpression l => rhs is BinaryExpression r && l.EqualsToExpression(r, ignoreNames),
            ConstantExpression l => rhs is ConstantExpression r && l.EqualsToExpression(r),
            MemberExpression l => rhs is MemberExpression r && l.EqualsToExpression(r, ignoreNames),
            MethodCallExpression l => rhs is MethodCallExpression r && l.EqualsToExpression(r, ignoreNames),
            NewExpression l => rhs is NewExpression r && l.EqualsToExpression(r, ignoreNames),
            ParameterExpression l => rhs is ParameterExpression r && l.EqualsToExpression(r, ignoreNames),
            UnaryExpression l => rhs is UnaryExpression r && l.EqualsToExpression(r),
            _ => false
        };
    }

    public static IEnumerable<TExpression> ExtractExpressions<TExpression>(this Expression expression)
        where TExpression : Expression
    {
        if (null == expression) return [];

        var extractor = new ExpressionExtractor();
        return extractor.Extract<TExpression>(expression);
    }

    public static IEnumerable<Expression> Flatten(this Expression expression)
    {
        var flattener = new ExpressionTreeFlattener();
        return flattener.Flatten(expression);
    }

    public static int GetExpressionHashCode(this Expression expression, bool ignoreName = false)
    {
        if(null == expression) return 0;

        return expression switch
        {
            BinaryExpression e    => e.GetExpressionHashCode(ignoreName),
            ConstantExpression e  => e.GetExpressionHashCode(),
            LambdaExpression e    => e.GetExpressionHashCode(ignoreName),
            MemberExpression e    => e.GetExpressionHashCode(ignoreName),
            NewExpression e       => e.GetExpressionHashCode(ignoreName),
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
                foreach (var p in be.GetParameters())
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
                    foreach (var p in e.GetParameters())
                        yield return p;
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

    public static IEnumerable<BinaryExpression> GetTerminalBinaryExpressions(this Expression expression)
    {
        var extractor = new TerminalBinaryExpressionExtractor();
        return extractor.Extract(expression);
    }

    public static bool HasParameter(this Expression expression, ParameterExpression parameter, bool ignoreNames = false)
        => expression.GetParameters().Any(x => x.EqualsToExpression(parameter, ignoreNames));

    public static bool IsConstant(this Expression expression)
    {
        return expression.NodeType == ExpressionType.Constant;
    }

    public static bool IsConvert(this Expression expression)
    {
        return expression.NodeType == ExpressionType.Convert;
    }

    public static bool IsMethodCall(this Expression expression) => expression.NodeType == ExpressionType.Call;

    public static bool IsPredicate(this Expression expression)
    {
        return expression is LambdaExpression lambda
            && lambda.Body is BinaryExpression binaryExpression
            && binaryExpression.IsPredicate();
    }

    public static bool IsTerminal(this Expression expression)
    {
        expression.ThrowIfNull();

        if (expression.NodeType.IsTerminal()) return true;

        return expression.NodeType switch
        {
            ExpressionType.Convert or
            ExpressionType.Negate => expression is UnaryExpression unary && IsTerminal(unary.Operand),
            _ => IsTerminalBinary(expression)
        };
    }

    /// <summary>
    /// Checks if expression is a BinaryExpression which is not a predicate and where Left and Right does not contain a predicate.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static bool IsTerminalBinary(this Expression expression)
    {
        if (expression is not BinaryExpression be) return false;
        if (be.IsPredicate()) return false;

        return be.Left.IsTerminal() && be.Right.IsTerminal();
    }

    public static bool IsTerminalPredicate(this Expression expression)
    {
        if (expression is not BinaryExpression be) return false;
        if (!be.IsPredicate()) return false;

        return be.Left.IsTerminal() && be.Right.IsTerminal();
    }

    /// <summary>
    /// Replaces an expression in an expression tree.
    /// </summary>
    /// <param name="expression">The expression including the expression which should be replaced.</param>
    /// <param name="toBeReplaced">The expression which should be replaced. Must be part of expression.</param>
    /// <param name="replacement">The replacement of <paramref name="toBeReplaced"/></param>
    /// <returns></returns>
    public static Expression? Replace(this Expression expression, Expression toBeReplaced, Expression replacement)
    {
        var replacer = new ExpressionReplacer();
        return replacer.Replace(expression, toBeReplaced, replacement);
    }
}
