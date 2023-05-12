using System.Linq.Expressions;

namespace Foundation.Linq;

public static class FunctorExtensions
{
    public static Expression<Func<T, TResult>> ToExpression<T, TResult>(this Func<T, TResult> func)
    {
        Expression<Func<T, TResult>> expression = x => func(x);
        return expression;
    }
}
