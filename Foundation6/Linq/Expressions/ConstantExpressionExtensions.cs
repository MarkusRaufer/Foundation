using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ConstantExpressionExtensions
{
    public static bool EqualsToExpression(this ConstantExpression lhs, ConstantExpression rhs)
    {
        if (lhs is null) return rhs is null;
        return rhs is not null && lhs.Value.EqualsNullable(rhs.Value);
    }

    public static int GetExpressionHashCode(this ConstantExpression expression)
    {
        return expression.ThrowIfNull().Value.GetNullableHashCode();
    }
}
