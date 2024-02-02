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

public class ExpressionTreeFlattener : ExpressionVisitor
{
    private readonly ICollection<Expression> _expressions;

    public ExpressionTreeFlattener()
    {
        _expressions = new List<Expression>();
    }

    public void ClearExpressions()
    {
        _expressions.Clear();
    }

    public IEnumerable<Expression> Flatten(Expression expression)
    {
        ClearExpressions();

        Visit(expression);
        return _expressions;
    }
    
    protected override Expression VisitBinary(BinaryExpression node)
    {
        _expressions.Add(node);
        return base.VisitBinary(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _expressions.Add(node);
        return base.VisitConstant(node);
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        _expressions.Add(node);
        return base.VisitLambda(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        _expressions.Add(node);
        return base.VisitMember(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        _expressions.Add(node);
        return base.VisitMethodCall(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        _expressions.Add(node);
        return base.VisitParameter(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        _expressions.Add(node);
        return base.VisitUnary(node);
    }
}
