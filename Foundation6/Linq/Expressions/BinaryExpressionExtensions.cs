using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class BinaryExpressionExtensions
{
    public static bool EqualsToExpression(this BinaryExpression lhs, BinaryExpression rhs)
    {
        if (null == lhs) return null == rhs;

        return lhs.NodeType == rhs.NodeType
            && lhs.Type == rhs.Type
            && lhs.Left.EqualsToExpression(rhs.Left)
            && lhs.Right.EqualsToExpression(rhs.Right);
    }

    public static int GetExpressionHashCode(this BinaryExpression expression)
    {
        expression.ThrowIfNull();
        return HashCode.FromOrderedHashCode(expression.NodeType.GetHashCode(),
                                            expression.Type.GetHashCode(),
                                            expression.Left.GetExpressionHashCode(),
                                            expression.Right.GetExpressionHashCode());
    }

    public static bool IsPredicate(this BinaryExpression expression)
    {
        return expression.ThrowIfNull().NodeType.IsPredicate();
    }
}
