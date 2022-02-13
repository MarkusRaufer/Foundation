using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class NumberHelper
{
    public static IEnumerable<int> GetDigits(int value)
    {
        return GetDigits(value, n => n);
    }

    public static IEnumerable<TResult> GetDigits<TResult>(int value, [DisallowNull] Func<int, TResult> projection)
    {
        projection.ThrowIfNull();

        var digits = new Stack<TResult>();

        int number = value;
        while (number > 0)
        {
            var digit = number % 10;
            digits.Push(projection(digit));

            number /= 10;
        }

        foreach (var d in digits)
            yield return d;
    }

    public static IEnumerable<TResult> GetDigits<TResult>(long value, [DisallowNull] Func<long, TResult> projection)
    {
        projection.ThrowIfNull();

        var digits = new Stack<TResult>();

        long number = value;
        while (number > 0)
        {
            var digit = number % 10;
            digits.Push(projection(digit));

            number /= 10;
        }

        foreach (var d in digits)
            yield return d;
    }
}

