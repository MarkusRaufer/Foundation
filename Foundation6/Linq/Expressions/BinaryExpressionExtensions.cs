using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class BinaryExpressionExtensions
{
    public static BinaryExpression? Concat(this IEnumerable<BinaryExpression> expressions, ExpressionType binaryType)
    {
        return ExpressionHelper.Concat(expressions, binaryType);
    }

    public static bool EqualsToExpression(this BinaryExpression lhs, BinaryExpression rhs)
    {
        if (null == lhs) return null == rhs;

        return lhs.NodeType == rhs.NodeType
            && lhs.Type == rhs.Type
            && lhs.Left.EqualsToExpression(rhs.Left)
            && lhs.Right.EqualsToExpression(rhs.Right);
    }

    public static int GetExpressionHashCode(this BinaryExpression expression, bool ignoreName = false)
    {
        expression.ThrowIfNull();
        return HashCode.FromOrderedHashCode(expression.NodeType.GetHashCode(),
                                            expression.Type.GetHashCode(),
                                            expression.Left.GetExpressionHashCode(ignoreName),
                                            expression.Right.GetExpressionHashCode(ignoreName));
    }

    public static bool IsPredicate(this BinaryExpression expression)
    {
        return expression.ThrowIfNull().NodeType.IsPredicate();
    }
}
