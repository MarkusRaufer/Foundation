using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public class ExpressionReplacer : ExpressionVisitor
{
    private bool _isInitialized;
    private Expression? _replacement;
    private Expression? _toBeReplaced;

    public Expression? Replace(Expression source, Expression toBeReplaced, Expression replacement)
    {
        source.ThrowIfNull();
        _toBeReplaced = toBeReplaced.ThrowIfNull();
        _replacement = replacement.ThrowIfNull();
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
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitBinary(node);
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitBlock(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitConditional(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitMember(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitParameter(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node.EqualsToExpression(_toBeReplaced, ignoreNames: false)) return _replacement!;

        return base.VisitUnary(node);
    }
}
