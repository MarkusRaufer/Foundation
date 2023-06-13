using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ParameterExpressionExtensions
{
    public static bool EqualsToExpression(this ParameterExpression lhs, ParameterExpression rhs, bool ignoreName = true)
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;

        return ignoreName
            ? lhs.Type == rhs.Type
            : lhs.Name.EqualsNullable(rhs.Name) && lhs.Type == rhs.Type;
    }

    public static int GetExpressionHashCode(this ParameterExpression expression, bool ignoreName = true)
    {
        expression.ThrowIfNull();
        return ignoreName ? expression.Type.GetHashCode()
                          : System.HashCode.Combine(expression.Type, expression.Name);
    }
}
