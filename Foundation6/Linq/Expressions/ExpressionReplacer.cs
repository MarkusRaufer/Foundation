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

public class ExpressionReplacer : ExpressionVisitor
{
    private bool _ignoreNames = false;
    private bool _isInitialized;
    private Expression? _replacement;
    private Expression? _toBeReplaced;

    public Expression? Replace(Expression source, Expression toBeReplaced, Expression replacement, bool ignoreNames = false)
    {
        source.ThrowIfNull();
        _toBeReplaced = toBeReplaced.ThrowIfNull();
        _replacement = replacement.ThrowIfNull();
        _ignoreNames = ignoreNames;
        _isInitialized = true;

        return Visit(source);
    }

    public override Expression? Visit(Expression? node)
    {
        if (!_isInitialized) throw new InvalidOperationException("dont't call Visit use Replace method instead.");

        return base.Visit(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (_toBeReplaced is BinaryExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitBinary(node);
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        if (_toBeReplaced is BlockExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitBlock(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        if (_toBeReplaced is ConditionalExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitConditional(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (_toBeReplaced is ConstantExpression x && node.EqualsToExpression(x)) return _replacement!;

        return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (_toBeReplaced is MemberExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitMember(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (_toBeReplaced is MethodCallExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_toBeReplaced is ParameterExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitParameter(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (_toBeReplaced is UnaryExpression x && node.EqualsToExpression(x, _ignoreNames)) return _replacement!;

        return base.VisitUnary(node);
    }
}
