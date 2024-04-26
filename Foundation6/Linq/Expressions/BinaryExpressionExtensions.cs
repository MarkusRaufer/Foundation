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

public static class BinaryExpressionExtensions
{
    public static BinaryExpression? Concat(this IEnumerable<BinaryExpression> expressions, ExpressionType binaryType)
    {
        return ExpressionHelper.Concat(expressions, binaryType);
    }

    public static bool EqualsToExpression(this BinaryExpression? lhs, BinaryExpression? rhs, bool ignoreNames = true)
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;

        return lhs.NodeType == rhs.NodeType
            && lhs.Type == rhs.Type
            && (lhs.Left.EqualsToExpression(rhs.Left, ignoreNames) && lhs.Right.EqualsToExpression(rhs.Right, ignoreNames)
            || lhs.Left.EqualsToExpression(rhs.Right, ignoreNames) && lhs.Right.EqualsToExpression(rhs.Left, ignoreNames));
    }

    public static IEnumerable<BinaryExpression> GetBinaryExpressions(this BinaryExpression? expression)
    {
        if(null == expression) return [];

        var extractor = new ExpressionExtractor();
        return extractor.Extract<BinaryExpression>(expression);
    }

    public static IEnumerable<Expression> GetLeftAndRightExpression(this BinaryExpression expression)
    {
        expression.ThrowIfNull();

        yield return expression.Left;
        yield return expression.Right;
    }

    public static int GetExpressionHashCode(this BinaryExpression? expression, bool ignoreName = false)
    {
        if(null == expression) return 0;

        return HashCode.FromOrderedHashCode(expression.NodeType.GetHashCode(),
                                            expression.Type.GetHashCode(),
                                            expression.Left.GetExpressionHashCode(ignoreName),
                                            expression.Right.GetExpressionHashCode(ignoreName));
    }

    public static IEnumerable<ParameterExpression> GetParameters(this BinaryExpression? expression)
    {
        if (expression == null) yield break;

        foreach(var l in expression.Left.GetParameters())
            yield return l;

        foreach (var r in expression.Right.GetParameters())
            yield return r;
    }

    public static IEnumerable<BinaryExpression> GetPredicates(this BinaryExpression? expression)
    {
        foreach(var binaryExpression in expression.GetBinaryExpressions())
        {
            if(binaryExpression.IsPredicate()) yield return binaryExpression;
        }
    }

    public static bool HasConstant(this BinaryExpression expression)
        => expression.Left.IsConstant() || expression.Right.IsConstant();

    public static bool HasParameter(this BinaryExpression? expression, ParameterExpression parameter)
    {
        expression.ThrowIfNull();
        parameter.ThrowIfNull();

        return GetParameters(expression).Any(x => x.EqualsToExpression(parameter));
    }

    public static bool IsPredicate(this BinaryExpression expression)
    {
        expression.ThrowIfNull();
        
        return expression.NodeType.IsBinary() && expression.Type == typeof(bool);
    }

    public static bool IsTerminalBinary(this BinaryExpression expression)
    {
        if (!expression.ThrowIfNull().NodeType.IsTerminalBinary()) return false;

        return expression.Left.IsTerminal() && expression.Right.IsTerminal();
    }

    public static bool IsTerminalPredicate(this BinaryExpression expression)
    {
        expression.ThrowIfNull();

        return expression.IsPredicate() && expression.IsTerminalBinary();
    }
}
