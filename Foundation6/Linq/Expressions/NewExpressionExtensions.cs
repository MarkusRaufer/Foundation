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

namespace Foundation.Linq.Expressions;

public static class NewExpressionExtensions
{
    public static bool EqualsToExpression(this NewExpression lhs, NewExpression rhs, bool ignoreNames = false)
    {
        if (lhs is null) return rhs is null;
        return rhs is not null
            && lhs.NodeType == rhs.NodeType
            && lhs.Arguments.SequenceEqual(rhs.Arguments, (l, r) => l.EqualsToExpression(r, ignoreNames));
    }

    public static int GetExpressionHashCode(this NewExpression expression, bool ignoreName = false)
    {
        if (expression is null) return 0;
        return HashCode.CreateBuilder()
            .AddObject(expression.NodeType)
            .AddHashCodes(expression.Arguments.Select(x => x.GetExpressionHashCode(ignoreName)))
            .GetHashCode();
    }
}
