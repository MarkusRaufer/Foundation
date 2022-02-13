namespace Foundation;

using System.Diagnostics;

public class Range
{
    public static Range<T, T> Create<T>(params IRangeExpression<T>[] rangeExpressions)
    {
        return Create<T, T>(rangeExpressions);
    }

    public static Range<TIn, TOut> Create<TIn, TOut>(params IRangeExpression<TIn>[] rangeExpressions)
    {
        return new Range<TIn, TOut>(rangeExpressions);
    }
}

/// <summary>
/// Defines a range of values.
/// </summary>
[DebuggerDisplay("Min={Min}, Max={Max}")]
public struct Range<TIn, TOut>
{
    private readonly bool _containsOnlyValueExpressions;
    private readonly IRangeExpression<TIn>[] _rangeExpressions;

    public Range(params IRangeExpression<TIn>[] rangeExpressions)
    {
        _rangeExpressions = rangeExpressions.ThrowIfNull();
        _containsOnlyValueExpressions = rangeExpressions.All(re => re is IValueRangeExpression);
    }

    /// <summary>
    /// Returns true if all range expressions are of type IValueRangeExpression.
    /// </summary>
    public bool ContainsOnlyValueExpressions => _containsOnlyValueExpressions;

    /// <summary>
    /// Returns true if the value is whithing the range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsInRange(TIn value) => _rangeExpressions.Any(re => re.IsInRange(value));

    public IEnumerable<IRangeExpression<TIn>> RangeExpressions => _rangeExpressions;

    /// <summary>
    /// The values of the range expressions. If ContainsOnlyValueExpressions if false, no value is returned.
    /// </summary>
    public IEnumerable<TOut> Values
    {
        get
        {
            if (!_containsOnlyValueExpressions)
                yield break;

            foreach (var rangeExpression in _rangeExpressions)
            {
                if (rangeExpression is ISingleValueRangeExpression<TIn, TOut> singleValue)
                {
                    yield return singleValue.Value;
                    continue;
                }

                if (rangeExpression is not IMultiValueRangeExpression<TOut> multiValue) continue;

                foreach (var value in multiValue.Values)
                {
                    yield return value;
                }
            }
        }
    }
}

