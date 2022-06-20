namespace Foundation;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// RangeExpression with a minimum and a maximum value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBetweenRangeExpression<T> : IRangeExpression<T>
{
    T Max { get; }
    T Min { get; }
}

/// <summary>
/// ValueRangeExpression with multiple values.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMultiValueRangeExpression<T> : IRangeExpression<T>, IValueRangeExpression
{
    IEnumerable<T> Values { get; }
}

/// <summary>
/// RangeExpression to check if a value is in the range of values.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRangeExpression<in T>
{
    bool IsInRange(T value);
}

/// <summary>
/// ValueRangeExpression with a single value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISingleValueRangeExpression<T> : ISingleValueRangeExpression<T, T>
{
}

/// <summary>
/// ValueRangeExpression with a single value. IsInRange(TIn value).
/// </summary>
/// <typeparam name="typeparam name="TIn"">the type of the IRangeExpression.</typeparam>
/// <typeparam name="TOut">the type of the output value.</typeparam>
public interface ISingleValueRangeExpression<TIn, TOut> : IRangeExpression<TIn>, IValueRangeExpression
{
    TOut Value { get; }
}

/// <summary>
/// Marker interface for RangeExpressions containing values.
/// </summary>
public interface IValueRangeExpression
{

}

/// <summary>
/// RangeExpression to check if a value is in a range of a minimum and a maximum value.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Between<T>
    : IBetweenRangeExpression<T>
    , IEquatable<IBetweenRangeExpression<T>>
    where T : struct
{
    private int _hashCode;
    private Func<T, T, bool>? _greaterThanOrEqual;
    private Func<T, T, bool>? _lessThanOrEqual;

    public Between(T min, T max)
    {
        if (!NumericHelper.LessThanOrEqual(min, max))
            throw new ArgumentException("max must be greater or equal than min");

        Min = min;
        Max = max;

        _hashCode = System.HashCode.Combine(Min, Max);
    }

    public override bool Equals(object? obj) => obj is IBetweenRangeExpression<T> other && Equals(other);

    public bool Equals(IBetweenRangeExpression<T>? other) => null != other && Min.Equals(other.Min) && Max.Equals(other.Max);

    public override int GetHashCode() => _hashCode;

    /// <summary>
    /// Returns true, if the value is between Min and Max.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsInRange(T value)
    {
        if (null == _greaterThanOrEqual)
        {
            var expression = NumericHelper.CreateGreaterThanOrEqualExpression<T>(nameof(value), nameof(Min));
            _greaterThanOrEqual = expression.Compile();
        }

        if (null == _lessThanOrEqual)
        {
            var expression = NumericHelper.CreateLessThanOrEqualExpression<T>(nameof(value), nameof(Max));
            _lessThanOrEqual = expression.Compile();
        }

        return _greaterThanOrEqual(value, Min) && _lessThanOrEqual(value, Max);
    }

    /// <summary>
    /// The maximum value.
    /// </summary>
    public T Max { get; }

    /// <summary>
    /// The minimum value.
    /// </summary>
    public T Min { get; }

    public override string ToString() => $"Min: {Min}, Max: {Max}";
}

/// <summary>
/// RangeExpression to check if a value equals a certain value.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Exactly<T>
    : ISingleValueRangeExpression<T>
    , IEquatable<ISingleValueRangeExpression<T>>
{
    public Exactly(T value)
    {
        Value = value.ThrowIfNull();
    }

    public override bool Equals(object? obj) => obj is ISingleValueRangeExpression<T> other && Equals(other);

    public bool Equals(ISingleValueRangeExpression<T>? other) => null != other && Value!.Equals(other.Value);

    public override int GetHashCode() => Value!.GetHashCode();

    public bool IsInRange(T value) => Value!.Equals(value);

    public override string ToString() => $"{Value}";

    public T Value { get; }

}

/// <summary>
/// RangeExpression to check if a value matches a lambda.
/// </summary>
/// <typeparam name="T">Type of the value.</typeparam>
public class Matching<T> : IRangeExpression<T>
    where T : notnull
{
    private readonly Func<T, bool> _predicate;

    public Matching(Func<T, bool> predicate)
    {
        _predicate = predicate;
    }

    public bool IsInRange(T? value) => null != value && _predicate(value);
}

public class BufferdGenerator<T> : IMultiValueRangeExpression<T>
{
    private readonly Func<T> _factory;
    private readonly HashSet<T> _hashSet;
    private readonly int _quantity;

    public BufferdGenerator(Func<T> factory, int quantity = 1)
    {
        _factory = factory;
        _quantity = quantity;

        _hashSet = new HashSet<T>();
    }

    private T CreateValue()
    {
        var value = _factory();
        _hashSet.Add(value);
        return value;
    }

    public bool IsInRange(T? value)
    {
        if (null == value) return false;

        if (_hashSet.Count < _quantity)
        {
            for (var i = _hashSet.Count; i < _quantity; i++)
                CreateValue();
        }

        return _hashSet.Contains(value);
    }

    public IEnumerable<T> Values
    {
        get
        {
            var e = _hashSet.GetEnumerator();
            for (var i = 0; i < _quantity; i++)
            {
                if (i < _hashSet.Count)
                {
                    e.MoveNext();
                    yield return e.Current;
                }
                else
                    yield return CreateValue();
            }
        }
    }
}

public class Generator<T> : IMultiValueRangeExpression<T>
{
    private readonly Func<T> _factory;
    private readonly int _quantity;

    public Generator(Func<T> factory, int quantity = 1)
    {
        _factory = factory;
        _quantity = quantity;
    }

    public bool IsInRange(T? value)
    {
        if (null == value) return false;

        foreach (var v in Values)
        {
            if (v is null) continue;

            if (v.Equals(value)) return true;
        }
        return false;
    }

    public IEnumerable<T> Values
    {
        get
        {
            for (var i = 0; i < _quantity; i++)
            {
                yield return _factory();
            }
        }
    }
}
/// <summary>
/// RangeExpression to check if a value is in a range of a minimum and a maximum value. You can iterate to all values.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class NumericBetween<T> : Between<T>, IMultiValueRangeExpression<T>
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    private Func<T, T>? _increment;
    private Func<T, T, bool>? _lessThanOrEqual;

    public NumericBetween(T min, T max) : base(min, max)
    {
        if (!typeof(T).IsPrimitive)
            throw new ArgumentException("T must be a primitive type");
    }

    /// <summary>
    /// All values between Min and Max.
    /// </summary>
    public IEnumerable<T> Values
    {
        get
        {
            const string parameter = "i";
            if (null == _lessThanOrEqual)
            {
                var expression = NumericHelper.CreateLessThanOrEqualExpression<T>(parameter, nameof(Max));
                _lessThanOrEqual = expression.Compile();
            }

            if (null == _increment)
            {
                var expression = NumericHelper.CreateIncrementExpression<T>(parameter);
                _increment = expression.Compile();
            }

            for (var i = Min; _lessThanOrEqual(i, Max); i = _increment(i))
                yield return i;

            //for (var i = Min; NumericHelper.LessThanOrEqual(i, Max); NumericHelper.Increment(ref i))
            //    yield return i;
        }
    }
}

/// <summary>
/// RangeExpression to check if a value is in a list of values.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class OneOf<T> : IMultiValueRangeExpression<T>
    where T : notnull
{
    private readonly T[] _values;

    public OneOf(params T[] values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    /// <summary>
    /// Returns true, if the value equals to one of the Values.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsInRange(T? value) => _values.Any(v => v.Equals(value));

    public IEnumerable<T> Values => _values;
}

/// <summary>
/// RangeExpression to check if a value is in a list of values.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class OfType<T> : ISingleValueRangeExpression<object, T?>
{
    /// <summary>
    /// Returns true, if the value equals to one of the Values.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsInRange(object? value)
    {
        if (value is T t)
        {
            Value = t;
            return true;
        }

        return false;
    }

    public T? Value { get; private set; }
}

