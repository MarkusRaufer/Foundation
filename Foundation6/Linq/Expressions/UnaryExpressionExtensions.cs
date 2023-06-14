using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class UnaryExpressionExtensions
{
    public static int GetExpressionHashCode(this UnaryExpression expression, bool ignoreName = true)
    {
        expression.ThrowIfNull();

        return HashCode.FromOrderedHashCode(expression.NodeType.GetHashCode(),
                                            expression.Type.GetHashCode(),
                                            expression.Operand.GetExpressionHashCode(ignoreName));
    }
}
