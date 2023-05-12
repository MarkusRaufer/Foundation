using System.Linq.Expressions;

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
