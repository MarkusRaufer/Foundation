using Foundation.Linq.Expressions;
using System.Linq.Expressions;

namespace Foundation.Collections.Generic;

public static class SortedListExtensions
{
    public static SortedList<T> FindAll<T>(this SortedList<T> list, LambdaExpression lambda)
    {
        list.ThrowIfNull();
        if(!lambda.ThrowIfNull().IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        if(1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"one parameter expected");

        if (lambda.Parameters.First().Type != typeof(T))
            throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");


        var predicateExpression = lambda as Expression<Func<T, bool>>;
        if(null ==  predicateExpression)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        return list.FindAll(new Predicate<T>(predicateExpression.Compile()));
    }
}
