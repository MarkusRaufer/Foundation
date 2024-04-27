using Foundation.Collections.Generic;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class NewExpressionExtensions
{
    public static bool EqualsToExpression(this NewExpression lhs, NewExpression rhs, bool ignoreNames = false)
    {
        if (lhs is null) return rhs is null;
        return rhs is not null
            && lhs.NodeType == rhs.NodeType
            && lhs.Arguments.SequenceEqual(rhs.Arguments, (l, r) => l.EqualsToExpression(r, ignoreNames));
    }

    public static int GetExpressionHashCode(this NewExpression expression, bool ignoreName = false)
    {
        if (expression is null) return 0;
        return HashCode.CreateBuilder()
            .AddObject(expression.NodeType)
            .AddHashCodes(expression.Arguments.Select(x => x.GetExpressionHashCode(ignoreName)))
            .GetHashCode();
    }
}
