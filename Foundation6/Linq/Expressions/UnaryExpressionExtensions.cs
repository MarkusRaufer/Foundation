using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class UnaryExpressionExtensions
{
    public static int GetExpressionHashCode(this UnaryExpression expression)
    {
        expression.ThrowIfNull();
        return System.HashCode.Combine(expression.NodeType.GetHashCode(),
                                       expression.Type.GetHashCode(),
                                       expression.Operand.GetExpressionHashCode());
    }
}
