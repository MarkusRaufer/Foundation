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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions
{
    public class ExpressionExtractor : ExpressionVisitor
    {
        private readonly List<Expression> _expressions = new();
        private Func<Expression, bool>? _predicate;

        protected void AddExpression(Expression expression)
        {
            if (null != _predicate && _predicate(expression))
                _expressions.Add(expression);
        }

        public IEnumerable<TExpression> Extract<TExpression>(Expression expression)
            where TExpression : Expression
        {
            return Extract(expression, typeof(TExpression)).OfType<TExpression>();
        }

        public IEnumerable<Expression> Extract(Expression expression, Func<Expression, bool> predicate)
        {
            expression.ThrowIfNull();
            _predicate = predicate.ThrowIfNull();
            _expressions.Clear();

            Visit(expression);

            return _expressions;
        }

        public IEnumerable<Expression> Extract(Expression expression, ExpressionType type)
        {
            return Extract(expression, e => e.NodeType == type);
        }

        public IEnumerable<Expression> Extract(Expression expression, Type expressionType)
        {
            return Extract(expression, e => expressionType.IsAssignableFrom(e.GetType()));
        }

        public IEnumerable<TExpression> ExtractExpressions<TExpression>(Expression expression)
            where TExpression : Expression
        {
            return Extract(expression, typeof(TExpression)).OfType<TExpression>();
        }

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            if (node is not null) AddExpression(node);

            return base.Visit(node);
        }
    }
}
