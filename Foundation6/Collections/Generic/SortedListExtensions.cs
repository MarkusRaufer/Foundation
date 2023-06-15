using Foundation.Linq.Expressions;
using System.Linq.Expressions;

namespace Foundation.Collections.Generic;

public static class SortedListExtensions
{
    /// <summary>
    /// Retrieves all the elements that match the conditions defined by the specified lambda.
    /// </summary>
    /// <param name="list">The list to execute the lambda.</param>
    /// <param name="lambda">Filter for FindAll method.</param>
    /// <returns></returns>
    public static List<T> FindAll<T>(this SortedList<T> list, LambdaExpression lambda)
    {
        list.ThrowIfNull();
        if(!lambda.ThrowIfNull().IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        if(1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"one parameter expected");

        if (lambda.Parameters.First().Type != typeof(T))
            throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");

        var func = (Func<T, bool>)lambda.Compile();
        return list.FindAll(new Predicate<T>(func));
    }
}
