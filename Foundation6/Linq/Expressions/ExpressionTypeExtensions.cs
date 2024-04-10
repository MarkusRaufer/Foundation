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

public static class ExpressionTypeExtensions
{
    /// <summary>
    /// Checks if <see cref="=ExpressionType"/> is a predicate.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsBinary(this ExpressionType type)
    {
        return type switch
        {
            ExpressionType.Add or
            ExpressionType.And or
            ExpressionType.AndAlso or
            ExpressionType.Divide or
            ExpressionType.Equal or
            ExpressionType.ExclusiveOr or
            ExpressionType.GreaterThan or
            ExpressionType.GreaterThanOrEqual or
            ExpressionType.LessThan or
            ExpressionType.LessThanOrEqual or
            ExpressionType.Multiply or
            ExpressionType.MultiplyAssign or
            ExpressionType.Not or
            ExpressionType.NotEqual or
            ExpressionType.Or or
            ExpressionType.OrElse or
            ExpressionType.Subtract => true,
            _ => false,
        };
    }

    public static bool IsTerminalBinary(this ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.Add or
            ExpressionType.Divide or
            ExpressionType.Equal or
            ExpressionType.GreaterThan or
            ExpressionType.GreaterThanOrEqual or
            ExpressionType.LessThan or
            ExpressionType.LessThanOrEqual or
            ExpressionType.Multiply or
            ExpressionType.Not or
            ExpressionType.NotEqual or
            ExpressionType.Subtract => true,
            _ => false,
        };
    }

    public static bool IsTerminal(this ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.Constant or
            ExpressionType.MemberAccess or
            ExpressionType.New or
            ExpressionType.Parameter => true,
            _ => IsTerminalBinary(expressionType),
        };
    }

    public static string ToCsharpString(this ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.And => "&&",
            ExpressionType.AndAlso => "&&",
            ExpressionType.Equal => "==",
            ExpressionType.ExclusiveOr => "^",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.Not => "!",
            ExpressionType.NotEqual => "!=",
            ExpressionType.Or => "||",
            ExpressionType.OrElse => "||",
            _ => throw new NotImplementedException(expressionType.ToString()),
        };
    }
}
