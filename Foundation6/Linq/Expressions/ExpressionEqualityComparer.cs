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

public class ExpressionEqualityComparer<TExpression>(Func<TExpression?, TExpression?, bool> equals, Func<TExpression, int> hash)
    : IEqualityComparer<TExpression>
    where TExpression : Expression
{
    private readonly Func<TExpression?, TExpression?, bool> _equals = equals.ThrowIfNull();
    private readonly Func<TExpression, int> _hash = hash.ThrowIfNull();

    public bool Equals(TExpression? x, TExpression? y) => _equals(x, y);

    public int GetHashCode([DisallowNull] TExpression obj) => _hash(obj);
}
