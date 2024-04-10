using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class MethodCallExpressionHelper
{
    public static ExpressionEqualityComparer<MethodCallExpression> CreateExpressionEqualityComparer(bool ignoreName = false)
    {
        return new ExpressionEqualityComparer<MethodCallExpression>(equals, MethodCallExpressionExtensions.GetExpressionHashCode);

        bool equals(MethodCallExpression? lhs, MethodCallExpression? rhs)
        {
            if (lhs is null) return rhs is null;
            if (rhs is null) return false;

            return lhs.EqualsToExpression(rhs, ignoreName);
        }
    }
}
