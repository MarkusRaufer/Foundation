using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class MemberExpressionExtensions
{
    public static bool EqualsToExpression(this MemberExpression lhs, MemberExpression rhs)
    {
        if (lhs is null) return rhs is null;

        return rhs is not null && lhs.Member.Equals(rhs.Member);
    }

    public static int GetExpressionHashCode(this MemberExpression expression)
    {
        expression.ThrowIfNull();
        return HashCode.FromOrderedObject(expression.Member, expression.Type);
    }
}
