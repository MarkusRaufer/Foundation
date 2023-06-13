using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class BinaryExpressionExtensions
{
    public static bool EqualsToExpression(this BinaryExpression lhs, BinaryExpression rhs)
    {
        if (null == lhs) return null == rhs;

        return lhs.NodeType == rhs.NodeType
            && EqualityComparer<object>.Default.Equals(lhs.Type, rhs.Type)
            && EqualityComparer<object>.Default.Equals(lhs.Left, rhs.Left)
            && EqualsToExpression(lhs, rhs);
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
