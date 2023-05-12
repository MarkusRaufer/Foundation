using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ExpressionTypeExtensions
{
    /// <summary>
    /// Checks if <see cref="=ExpressionType"/> is a predicate.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsPredicate(this ExpressionType type)
    {
        return type switch
        {
            ExpressionType.And or
            ExpressionType.AndAlso or
            ExpressionType.Equal or
            ExpressionType.ExclusiveOr or
            ExpressionType.GreaterThan or
            ExpressionType.GreaterThanOrEqual or
            ExpressionType.LessThan or
            ExpressionType.LessThanOrEqual or
            ExpressionType.Not or
            ExpressionType.NotEqual or
            ExpressionType.Or or
            ExpressionType.OrElse => true,
            _ => false,
        };
    }

    public static string ToCsharpString(this ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.And => "&&",
            ExpressionType.AndAlso => "&&",
            ExpressionType.Equal => "=",
            ExpressionType.ExclusiveOr => "^",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.Not => "!",
            ExpressionType.NotEqual => "!=",
            ExpressionType.Or => "||",
            ExpressionType.OrElse => "||",
            _ => throw new NotImplementedException(expressionType.ToString()),
        };
    }
}
