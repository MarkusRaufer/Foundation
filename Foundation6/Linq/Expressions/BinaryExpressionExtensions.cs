using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class BinaryExpressionExtensions
{
    public static bool IsPredicate(this BinaryExpression expression)
    {
        return expression.ThrowIfNull().NodeType.IsPredicate();
    }
}
