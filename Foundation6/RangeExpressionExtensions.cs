namespace Foundation;

public static class RangeExpressionExtensions
{
    public static IEnumerable<IBetweenRangeExpression<T>> Between<T>(this IEnumerable<IRangeExpression<T>> rangeExpressions)
    {
        foreach (var rangeExpression in rangeExpressions)
        {
            if (rangeExpression is IBetweenRangeExpression<T> expression)
                yield return expression;
        }
    }

    public static IEnumerable<IMultiValueRangeExpression<T>> MultiValue<T>(this IEnumerable<IRangeExpression<T>> rangeExpressions)
    {
        foreach (var rangeExpression in rangeExpressions)
        {
            if (rangeExpression is IMultiValueRangeExpression<T> expression)
                yield return expression;
        }
    }

    public static IEnumerable<ISingleValueRangeExpression<T>> SingleValue<T>(this IEnumerable<IRangeExpression<T>> rangeExpressions)
    {
        foreach (var rangeExpression in rangeExpressions)
        {
            if (rangeExpression is ISingleValueRangeExpression<T> expression)
                yield return expression;
        }
    }

    public static IEnumerable<ISingleValueRangeExpression<TIn, TOut>> SingleValue<TIn, TOut>(this IEnumerable<IRangeExpression<TIn>> rangeExpressions)
    {
        foreach (var rangeExpression in rangeExpressions)
        {
            if (rangeExpression is ISingleValueRangeExpression<TIn, TOut> expression)
                yield return expression;
        }
    }
}

