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

namespace Foundation.Linq.Expressions
{
    public class ExpressionExtractor : ExpressionVisitor
    {
        private readonly List<Expression> _expressions;
        private Func<Expression, bool>? _predicate;

        public ExpressionExtractor()
        {
            _expressions = new List<Expression>();
        }

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

        public IEnumerable<Expression> Extract(Expression expression, ExpressionType type)
        {
            _predicate = (e) => e.NodeType == type;
            _expressions.Clear();

            Visit(expression);

            return _expressions;
        }

        public IEnumerable<Expression> Extract(Expression expression, Type expressionType)
        {
            _predicate = (e) => expressionType.IsAssignableFrom(e.GetType());
            _expressions.Clear();

            Visit(expression);

            return _expressions;
        }

        public static IEnumerable<TExpression> ExtractExpressions<TExpression>(Expression expression)
            where TExpression : Expression
        {
            var extractor = new ExpressionExtractor();
            return extractor.Extract<TExpression>(expression);
        }

        public static IEnumerable<Expression> ExtractExpressions(Expression expression, ExpressionType type)
        {
            var extractor = new ExpressionExtractor();
            return extractor.Extract(expression, type);
        }

        public static IEnumerable<Expression> ExtractExpressions(Expression expression, Type expressionType)
        {
            var extractor = new ExpressionExtractor();
            return extractor.Extract(expression, expressionType);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            AddExpression(node);
            return base.VisitBinary(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            AddExpression(node);
            return base.VisitConstant(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            AddExpression(node);
            return base.VisitMember(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            AddExpression(node);
            return base.VisitParameter(node);
        }
    }
}
