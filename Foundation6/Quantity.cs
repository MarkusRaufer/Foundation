// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using Foundation.Buffers;
using System.Globalization;

namespace Foundation;

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity(string Unit, decimal Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();

    public static Quantity New(string unit, decimal value) => new(unit, value);

    public static Quantity<TValue> New<TValue>(string unit, TValue value) => new(unit, value);

    public static Quantity<TUnit, TValue> New<TUnit, TValue>(TUnit unit, TValue value) => new(unit, value);

    /// <summary>
    /// Converts a string representation of a quantity to a <see cref="Quantity"/>.
    /// </summary>
    /// <param name="str">The string represents the string format when you call <see cref="ToString"/>.
    /// </param>
    /// <returns></returns>
    public static Quantity Parse(string str)
    {
        return Parse(str, (string unit, decimal value) => new Quantity(unit, value));
    }

    /// <summary>
    /// Converts a string representation of a quantity to a <see cref="Quantity{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the quantity value.</typeparam>
    /// <param name="str">The string represents the string format when you call <see cref="ToString"/>.</param>
    /// <returns></returns>
    public static Quantity<TValue> Parse<TValue>(string str)
    {
        return Parse(str, (string unit, TValue value) => new Quantity<TValue>(unit, value));
    }

    private static TQuantity Parse<TValue, TQuantity>(string str, Func<string, TValue, TQuantity> factory)
        where TQuantity : struct
    {
        var span = str.AsSpan();
        var leftBraceIndex = span.IndexOf('{');
        var rightBraceIndex = span.IndexOf('}');
        var withoutBraces = span[(leftBraceIndex + 1)..(rightBraceIndex - 1)].Trim();

        var splitter = new CharSplitEnumerator(withoutBraces, true, ',');
        if (!splitter.MoveNext()) throw new InvalidArgumentException(nameof(str));

        var unitTuple = splitter.Current.Trim();
        var equalsSignIndex = unitTuple.IndexOf('=');
        var unitValue = unitTuple[(equalsSignIndex + 1)..].Trim();
        var unit = unitValue.ToString();

        if (!splitter.MoveNext()) throw new InvalidArgumentException(nameof(str));

        var valueTuple = splitter.Current.Trim();
        equalsSignIndex = valueTuple.IndexOf('=');
        var valueValue = valueTuple[(equalsSignIndex + 1)..].Trim();
        var value = ParseValue<TValue>(valueValue.ToString());

        return factory(unit, value);
    }

    /// <summary>
    /// Converts a string representation of a quantity to a <see cref="Quantity"/>.
    /// </summary>
    /// <typeparam name="TValue">The value of the quantity.</typeparam>
    /// <param name="str">In newer .NET version the string must be a JSON string.
    /// In older .NET versions the string represents the string format when you call <see cref="ToString"/>.</param>
    /// <param name="quantity">The converted <see cref="Quantity"/>.</param>
    /// <returns></returns>
    public static bool TryParse(string str, out Quantity quantity)
    {
        try
        {
            quantity = Parse(str);
            return true;
        }
        catch (Exception)
        {
            quantity = default;
            return false;
        }
    }

    /// <summary>
    /// Converts a string representation of a quantity to a <see cref="Quantity{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The value of the quantity.</typeparam>
    /// <param name="str">In newer .NET version the string must be a JSON string.
    /// In older .NET versions the string represents the string format when you call <see cref="ToString"/>.</param>
    /// <param name="quantity">The converted <see cref="Quantity{TValue}"/></param>
    /// <returns></returns>
    public static bool TryParse<TValue>(string str, out Quantity<TValue> quantity)
    {
        try
        {
            quantity = Parse<TValue>(str);
            return true;
        }
        catch (Exception)
        {
            quantity = default;
            return false;
        }
    }

    private static TValue ParseValue<TValue>(string valueStr)
    {
        Type type = typeof(TValue);
        object value = type switch
        {
            _ when type == typeof(bool) => bool.Parse(valueStr),
            _ when type == typeof(char) => char.Parse(valueStr),
            _ when type == typeof(decimal) => decimal.Parse(valueStr, CultureInfo.InvariantCulture),
            _ when type == typeof(double) => double.Parse(valueStr, CultureInfo.InvariantCulture),
            _ when type == typeof(int) => int.Parse(valueStr, CultureInfo.InvariantCulture),
            _ when type == typeof(long) => long.Parse(valueStr, CultureInfo.InvariantCulture),
            _ when type == typeof(Guid) => Guid.Parse(valueStr),
            _ when type == typeof(short) => short.Parse(valueStr, CultureInfo.InvariantCulture),
            _ when type == typeof(string) => valueStr,
            // Add more types as needed
            _ => throw new NotSupportedException($"Type '{type.Name}' is not supported.")
        };
        return (TValue)value;
    }

    public override string ToString()
    {
        return $"Quantity {{ Unit = {Unit}, Value = {Value.ToString(CultureInfo.InvariantCulture)} }}";
    }
}

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <typeparam name="TValue">Type of the value of the quantity.</typeparam>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity<TValue>(string Unit, TValue Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();

    public override string ToString()
    {
        var value = ((FormattableString)$"{Value}").ToString(CultureInfo.InvariantCulture);
        return $"Quantity {{ Unit = {Unit}, Value = {value} }}";
    }
}

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <typeparam name="TUnit">The unit type of the quantity.</typeparam>
/// <typeparam name="TValue">Type of the value of the quantity.</typeparam>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity<TUnit, TValue>(TUnit Unit, TValue Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();

    public override string ToString()
    {
        var value = ((FormattableString)$"{Value}").ToString(CultureInfo.InvariantCulture);
        return $"Quantity {{ Unit = {Unit}, Value = {value} }}";
    }
}

