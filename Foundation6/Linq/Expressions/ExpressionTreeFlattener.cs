using System.Linq.Expressions;

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
