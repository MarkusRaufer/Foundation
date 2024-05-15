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
﻿using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public class ExpressionEqualityComparer : ExpressionEqualityComparer<Expression>
{
    public ExpressionEqualityComparer(bool ignoreNames = false) : base(ignoreNames)
    {
    }

    public ExpressionEqualityComparer(Func<Expression?, Expression?, bool> equals, Func<Expression, int> hash) : base(equals, hash)
    {
    }
}

public class ExpressionEqualityComparer<TExpression> : IEqualityComparer<TExpression>
    where TExpression : Expression
{
    private readonly Func<TExpression?, TExpression?, bool> _equals;
    private readonly Func<TExpression, int> _hash;
    private readonly bool _ignoreNames;
    public ExpressionEqualityComparer(bool ignoreNames = false)
    {
        _equals = AreEqual;
        _hash = GetHash;
    }

    public ExpressionEqualityComparer(Func<TExpression?, TExpression?, bool> equals, Func<TExpression, int> hash, bool ignoreNames = false)
    {
        _equals = equals.ThrowIfNull();
        _hash = hash.ThrowIfNull();
        _ignoreNames = ignoreNames;
    }

    public bool Equals(TExpression? x, TExpression? y) => _equals(x, y);

    private bool AreEqual(TExpression? x, TExpression? y)
    {
        return x.EqualsToExpression(y, _ignoreNames);
    }

    private int GetHash(TExpression expression) => expression.GetExpressionHashCode();

#if NETSTANDARD2_0
    public int GetHashCode(TExpression expression) => _hash(expression);
#else
    public int GetHashCode([DisallowNull] TExpression expression) => _hash(expression);
#endif
}
