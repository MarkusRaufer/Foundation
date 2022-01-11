namespace Foundation;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

public static class NumericHelper
{
    /// <summary>
    /// Adds two values.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>Sum of left and right.</returns>
    public static T Add<T>(T left, T right) where T : struct
    {
        var expression = CreateAddExpression<T>(nameof(left), nameof(right));

        var add = expression.Compile();

        return add(left, right);


    }

    /// <summary>
    /// Adds a value to a number and assigns the new value to the number (like the operator +=).
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="left">Number to which a value is assigned.</param>
    /// <param name="right">Value that is assigned.</param>
    /// <returns>The assigned value.</returns>
    public static T AddAssign<T>(T left, T right) where T : struct
    {
        var expression = CreateAddAssingExpression<T>(nameof(left), nameof(right));

        var addAssign = expression.Compile();

        return addAssign(left, right);
    }

    /// <summary>
    /// Assigns a value to a number.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="left">Number to which a value is assigned.</param>
    /// <param name="right">Value that is assigned.</param>
    /// <returns>The assigned value.</returns>
    public static T Assign<T>(T left, T right) where T : struct
    {
        var expression = CreateAssingExpression<T>(nameof(left), nameof(right));

        var assign = expression.Compile();

        // call it
        return assign(left, right);
    }

    public static Expression<Func<T, T, T>> CreateAddAssingExpression<T>(string leftName, string rightName)
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.AddAssign(left, right);

        return Expression.Lambda<Func<T, T, T>>(body, left, right);
    }


    public static Expression<Func<T, T, T>> CreateAddExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.Add(left, right);

        return Expression.Lambda<Func<T, T, T>>(body, left, right);
    }

    public static Expression<Func<T, T, T>> CreateAssingExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.Assign(left, right);

        return Expression.Lambda<Func<T, T, T>>(body, left, right);
    }

    public static Expression<Func<T, T>> CreateDecrementExpression<T>(string parameterName) where T : struct
    {
        // declare the parameters
        var parameter = Expression.Parameter(typeof(T), parameterName);

        // add the parameters together
        var body = Expression.Decrement(parameter);

        return Expression.Lambda<Func<T, T>>(body, parameter);
    }

    public static Expression<Func<T, T, bool>> CreateEqualExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.Equal(left, right);

        return Expression.Lambda<Func<T, T, bool>>(body, left, right);
    }

    public static Expression<Func<T, T, bool>> CreateGreaterThanExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.GreaterThan(left, right);

        return Expression.Lambda<Func<T, T, bool>>(body, left, right);
    }

    public static Expression<Func<T, T, bool>> CreateGreaterThanOrEqualExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.GreaterThanOrEqual(left, right);

        return Expression.Lambda<Func<T, T, bool>>(body, left, right);
    }

    public static Expression<Func<T, T>> CreateIncrementExpression<T>(string parameterName) where T : struct
    {
        // declare the parameters
        var parameter = Expression.Parameter(typeof(T), parameterName);

        // add the parameters together
        var body = Expression.Increment(parameter);

        return Expression.Lambda<Func<T, T>>(body, parameter);
    }

    public static Expression<Func<T, T, bool>> CreateLessThanExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.LessThan(left, right);

        return Expression.Lambda<Func<T, T, bool>>(body, left, right);
    }

    public static Expression<Func<T, T, bool>> CreateLessThanOrEqualExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.LessThanOrEqual(left, right);

        return Expression.Lambda<Func<T, T, bool>>(body, left, right);
    }

    public static Expression<Func<T, T, T>> CreateModuloExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        // add the parameters together
        var body = Expression.Modulo(left, right);

        // compile it
        return Expression.Lambda<Func<T, T, T>>(body, left, right);
    }

    public static Expression<Func<T, T, T>> CreateSubtractExpression<T>(string leftName, string rightName) where T : struct
    {
        // declare the parameters
        var left = Expression.Parameter(typeof(T), leftName);
        var right = Expression.Parameter(typeof(T), rightName);

        var body = Expression.Subtract(left, right);

        return Expression.Lambda<Func<T, T, T>>(body, left, right);
    }

    //public static T Cast<T>(object value)
    //{
    //    var convert = Expression.Convert(Expression.Constant(value), typeof(T));
    //    var str = convert.ToString();
    //    return Expression.Lambda<Func<T>>(convert).Compile()();
    //}

    /// <summary>
    /// Decrements a number.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="value">value which will be decremented.</param>
    public static void Decrement<T>(ref T value) where T : struct
    {
        var expression = CreateDecrementExpression<T>(nameof(value));
        var decrement = expression.Compile();

        value = decrement(value);
    }

    /// <summary>
    /// Decrements a number.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="a"></param>
    /// <returns>The decremented value.</returns>
    public static T Decrement<T>(T a) where T : struct
    {
        var expression = CreateDecrementExpression<T>(nameof(a));
        var decrement = expression.Compile();

        return decrement(a);
    }

    /// <summary>
    /// Checks equality between two numbers.
    /// </summary>
    /// <typeparam name="T">Type of the numbers.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true if equal.</returns>
    public static bool Equal<T>(T left, T right) where T : struct
    {
        var expression = CreateEqualExpression<T>(nameof(left), nameof(right));

        var equal = expression.Compile();

        return equal(left, right);
    }

    /// <summary>
    /// Checks if left is greater than right.
    /// </summary>
    /// <typeparam name="T">Typ der Zahlen.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true, if left > right.</returns>
    public static bool GreaterThan<T>(T left, T right) where T : struct
    {
        var expression = CreateGreaterThanExpression<T>(nameof(left), nameof(right));

        var greaterThan = expression.Compile();

        return greaterThan(left, right);
    }

    /// <summary>
    /// Checks if left is greater or equal than right.
    /// </summary>
    /// <typeparam name="T">Typ der Zahlen.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true, if left >= right.</returns>
    public static bool GreaterThanOrEqual<T>(T left, T right) where T : struct
    {
        var expression = CreateGreaterThanOrEqualExpression<T>(nameof(left), nameof(right));

        var greaterThanOrEqual = expression.Compile();

        return greaterThanOrEqual(left, right);
    }

    /// <summary>
    /// Increments a number.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="value">The value which will be incremented.</param>
    public static void Increment<T>(ref T value) where T : struct
    {
        var expression = CreateIncrementExpression<T>(nameof(value));

        // compile it
        var increment = expression.Compile();
        value = increment(value);
    }

    /// <summary>
    /// Increments a number.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="value"></param>
    /// <returns>The incremented value.</returns>
    public static T Increment<T>(T value) where T : struct
    {
        var expression = CreateIncrementExpression<T>(nameof(value));

        // compile it
        var increment = expression.Compile();
        return increment(value);
    }

    /// <summary>
    /// Checks if left is less than right.
    /// </summary>
    /// <typeparam name="T">Typ der Zahlen.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true, if left < right.</returns>
    public static bool LessThan<T>(T left, T right) where T : struct
    {
        var expression = CreateLessThanExpression<T>(nameof(left), nameof(right));

        var lessThan = expression.Compile();

        return lessThan(left, right);
    }

    /// <summary>
    /// Checks if left is less or equal than right.
    /// </summary>
    /// <typeparam name="T">Type of the number.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>true, if left <= right.</returns>
    public static bool LessThanOrEqual<T>(T left, T right) where T : struct
    {
        var expression = CreateLessThanOrEqualExpression<T>(nameof(left), nameof(right));

        var lessThanOrEqual = expression.Compile();

        return lessThanOrEqual(left, right);
    }

    /// <summary>
    /// Return the modulo of to values.
    /// </summary>
    /// <typeparam name="T">Typ der Zahlen.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>Rest der Division.</returns>
    public static T Modulo<T>(T left, T right) where T : struct
    {
        var expression = CreateModuloExpression<T>(nameof(left), nameof(right));

        var modulo = expression.Compile();

        return modulo(left, right);
    }

    /// <summary>
    /// Subtracts right from left.
    /// </summary>
    /// <typeparam name="T">Typ der Zahlen.</typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>The difference between left and right.</returns>
    public static T Subtract<T>(T left, T right) where T : struct
    {
        var expression = CreateSubtractExpression<T>(nameof(left), nameof(right));

        var subtract = expression.Compile();

        return subtract(left, right);
    }

    /// <summary>
    /// Creates a total sum of the values.
    /// </summary>
    /// <typeparam name="T">Typ der Zahlen</typeparam>
    /// <param name="values"></param>
    /// <returns>Summe aller Zahlen in values.</returns>
    public static T Sum<T>([DisallowNull] IEnumerable<T> values) where T : struct
    {
        T sum = default;

        foreach (var value in values)
        {
            sum = Add(sum, value);
        }

        return sum;
    }
}

