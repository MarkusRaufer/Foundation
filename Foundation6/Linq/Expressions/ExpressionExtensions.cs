using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ExpressionExtensions
{
    public static bool EqualsToExpression(this Expression lhs, Expression rhs)
    {
        return lhs switch
        {
            BinaryExpression l => rhs is BinaryExpression r && EqualsToExpression(l, r),
            ConstantExpression l => rhs is ConstantExpression r && EqualsToExpression(l, r),
            LambdaExpression l => rhs is LambdaExpression r && EqualsToExpression(l, r),
            MemberExpression l => rhs is MemberExpression r && EqualsToExpression(l, r),
            ParameterExpression l => rhs is ParameterExpression r && EqualsToExpression(l, r),
            UnaryExpression l => rhs is UnaryExpression r && EqualsToExpression(l, r),
            _ => false
        };
    }

    public static IEnumerable<Expression> Flatten(this Expression expression)
    {
        var flattener = new ExpressionTreeFlattener();
        return flattener.Flatten(expression);
    }

    public static int GetExpressionHashCode(this Expression expression)
    {
        expression.ThrowIfNull();
        return expression switch
        {
            BinaryExpression e => e.GetExpressionHashCode(),
            ConstantExpression e => e.GetExpressionHashCode(),
            LambdaExpression e => e.GetExpressionHashCode(),
            MemberExpression e => e.GetExpressionHashCode(),
            ParameterExpression e => e.GetExpressionHashCode(false),
            UnaryExpression e => e.GetExpressionHashCode(),
            _ => 0
        };
    }

    public static bool IsConstant(this Expression expression)
    {
        return expression.NodeType == ExpressionType.Constant;
    }

    public static bool IsConvert(this Expression expression)
    {
        return expression.NodeType == ExpressionType.Convert;
    }

    public static bool IsPredicate(this Expression expression)
    {
        return expression is LambdaExpression lambda
            && lambda.Body is BinaryExpression binaryExpression
            && binaryExpression.IsPredicate();
    }

    public static bool IsTerminal(this Expression expression)
    {
        if(isTerminalNode(expression)) return true;

        if (expression is not BinaryExpression binary) return false;

        return isTerminalNode(binary.Left) && isTerminalNode(binary.Right);

        static bool isTerminalNode(Expression exp) => exp.NodeType switch
        {
            ExpressionType.Constant or
            ExpressionType.MemberAccess or
            ExpressionType.Parameter => true,
            ExpressionType.Convert => exp is UnaryExpression unary && isTerminalNode(unary.Operand),
            ExpressionType.Modulo => exp is BinaryExpression be && isTerminalNode(be.Left) && isTerminalNode(be.Right),
            _ => false
        };
    }
}
