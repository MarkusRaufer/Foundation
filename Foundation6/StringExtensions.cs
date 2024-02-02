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
﻿namespace Foundation;

using Foundation.Buffers;
using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static int CompareRegardingNumbers(this string? lhs, string? rhs, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(lhs)) return string.IsNullOrEmpty(rhs) ? 0 : -1;
        if (string.IsNullOrEmpty(rhs)) return 1;

        var lhsIsNumber = double.TryParse(lhs, out double lhsNumber);
        var rhsIsNumber = double.TryParse(rhs, out double rhsNumber);
        if (lhsIsNumber)
        {
            if (rhsIsNumber) return lhsNumber.CompareTo(rhsNumber);
            return -1;
        }
        else if (rhsIsNumber)
        {
            return 1;
        }

        return string.Compare(lhs, rhs, ignoreCase);
    }

    /// <summary>
    /// returns the indices of a character starting from the end.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesFromEnd(this string str, char value)
    {
        if (string.IsNullOrEmpty(str) || 0 == str.Length) yield break;

        var index = str.Length - 1;

        while (0 <= index)
        {
            if (str[index] == value) yield return index;
            
            index--;
        }
    }

    /// <summary>
    /// returns the indices of a substring starting from the end.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>

    public static IEnumerable<int> IndicesFromEnd(this string str, string value)
    {
        if (string.IsNullOrEmpty(str)) return Enumerable.Empty<int>();
        if (string.IsNullOrEmpty(value)) return Enumerable.Empty<int>();

        var strSpan = str.AsSpan();
        var valueSpan = value.AsSpan();

        return strSpan.IndicesFromEnd(valueSpan);
    }

    /// <summary>
    /// Returns the indices of a substring.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <param name="stopAfterNumberOfHits"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOf(this string str, string value, int stopAfterNumberOfHits = -1)
    {
        var numberOfHits = 0;
        var index = str.IndexOf(value);

        while (-1 != index)
        {
            numberOfHits++;
            if (-1 < stopAfterNumberOfHits && numberOfHits > stopAfterNumberOfHits) break;

            yield return index;
            if (str.Length < (index + 1)) break;
            index = str.IndexOf(value, index + 1);
        }
    }

    /// <summary>
    /// Returns a list of indices of the found characters.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="values"></param>
    /// <param name="stopAfterNumberOfHits"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOfAny(this string str, char[] values, int stopAfterNumberOfHits = -1)
    {
        var numberOfHits = 0;
        var i = 0;
        foreach (var c in str)
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits > stopAfterNumberOfHits) break;

            if (values.Contains(c))
            {
                numberOfHits++;
                yield return i;
            }

            i++;
        }
    }

    /// <summary>
    /// Returns a list of indices of the found substrings.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="values"></param>
    /// <param name="stopAfterNumberOfHits"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOfAny(this string str, string[] values, int stopAfterNumberOfHits = -1)
    {
        var numberOfHits = 0;
        var index = values.Min(v => str.IndexOf(v));

        while (-1 != index)
        {
            numberOfHits++;
            if (-1 < stopAfterNumberOfHits && numberOfHits > stopAfterNumberOfHits) break;

            yield return index;
            if (str.Length < (index + 1)) break;

            var indices = values.Select(v => str.IndexOf(v, index + 1)).Where(i => -1 != i);
            index = indices.Any() ? indices.Min() : -1;
        }
    }

    public static int IndexFromEnd(this string str, char value) => IndexFromEnd(str, str.Length - 1, value);

    public static int IndexFromEnd(this string str, string value) => IndexFromEnd(str, str.Length, value);

    public static int IndexFromEnd(this string str, int index, char value)
    {
        while (0 <= index)
        {
            if (str[index] == value) return index;

            index--;
        }
        return -1;
    }

    public static int IndexFromEnd(this string str, int index, string value)
    {
        while (0 <= index)
        {
            var startIndex = index - value.Length;
            if (0 > startIndex) break;

            var sub = str[startIndex..index];
            if (value == sub) return startIndex;
            index--;
        }
        return -1;
    }

    /// <summary>
    /// starts searching from the end.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static int IndexOfAnyFromEnd(this string str, char[] values) => IndexOfAnyFromEnd(str, str.Length - 1, values);
    
    public static int IndexOfAnyFromEnd(this string str, int index, char[] values)
    {
        while (0 <= index)
        {
            var c = str[index];
            if (values.Contains(c)) return index;
            index--;
        }
        return -1;
    }

    public static bool IsNumeric(this string str)
    {
        if (decimal.TryParse(str, out decimal _))
            return true;

        return false;
    }

    /// <summary>
    /// Returns Some if the string contains a primitive type or DateTime.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [Obsolete]
    public static Option<object> ParseScalarType(this string str, Type type)
    {
        type.ThrowIfNull();

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => bool.TryParse(str, out bool boolean)
                   ? Option.Some<object>(boolean)
                   : Option.None<object>(),
            TypeCode.Byte => byte.TryParse(str, out byte @byte)
                   ? Option.Some<object>(@byte)
                   : Option.None<object>(),
            TypeCode.Char => char.TryParse(str, out char @char)
                   ? Option.Some<object>(@char)
                   : Option.None<object>(),
            TypeCode.DateTime => DateTimeHelper.TryParseFromIso8601(str, out DateTime dt)
                   ? Option.Some<object>(dt)
                   : Option.None<object>(),
            TypeCode.Decimal => decimal.TryParse(str, out decimal @decimal)
                   ? Option.Some<object>(@decimal)
                   : Option.None<object>(),
            TypeCode.Double => double.TryParse(str, out double @double)
                   ? Option.Some<object>(@double)
                   : Option.None<object>(),
            TypeCode.Int16 => Int16.TryParse(str, out Int16 @int16)
                   ? Option.Some<object>(@int16)
                   : Option.None<object>(),
            TypeCode.Int32 => Int32.TryParse(str, out Int32 @int32)
                   ? Option.Some<object>(@int32)
                   : Option.None<object>(),
            TypeCode.Int64 => Int64.TryParse(str, out Int64 @int64)
                   ? Option.Some<object>(@int64)
                   : Option.None<object>(),
            TypeCode.SByte => SByte.TryParse(str, out SByte @sbyte)
                   ? Option.Some<object>(@sbyte)
                   : Option.None<object>(),
            TypeCode.Single => Single.TryParse(str, out Single @single)
                   ? Option.Some<object>(@single)
                   : Option.None<object>(),
            TypeCode.String => Option.Some<object>(str),
            TypeCode.UInt16 => UInt16.TryParse(str, out UInt16 @uint16)
                   ? Option.Some<object>(@uint16)
                   : Option.None<object>(),
            TypeCode.UInt32 => UInt32.TryParse(str, out UInt32 @uint32)
                   ? Option.Some<object>(@uint32)
                   : Option.None<object>(),
            TypeCode.UInt64 => UInt64.TryParse(str, out UInt64 @uint64)
                   ? Option.Some<object>(@uint64)
                   : Option.None<object>(),
            _ => Option.None<object>()
        };
    }

    /// <summary>
    /// Multiple spaces in sequence will be reduced to one space.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ReduceSpaces(this string str)
    {
        var re = new Regex("  +");
        var matches = re.Matches(str);
        if (0 == matches.Count) return str;

        var sb = new StringBuilder();
        var nextPos = 0;
        foreach (Match m in matches)
        {
            var substr = str.Substring(nextPos, m.Index - nextPos + 1);
            sb.Append(substr);
            nextPos = m.Index + m.Length;
        }

        var end = str[nextPos..];
        if (!string.IsNullOrWhiteSpace(end))
            sb.Append(end);

        return sb.ToString();
    }

    /// <summary>
    /// Repeats str
    /// </summary>
    /// <param name="str"></param>
    /// <param name="repeat"></param>
    /// <returns></returns>
    public static string Repeat(this string text, uint n)
    {
        var textAsSpan = text.AsSpan();
        var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
        for (var i = 0; i < n; i++)
        {
            textAsSpan.CopyTo(span.Slice(i * textAsSpan.Length, textAsSpan.Length));
        }

        return span.ToString();
    }

    /// <summary>
    /// Splits a string at the indexes.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitAtIndex(this string str, params int[] indexes)
    {
        if (0 == indexes.Length)
        {
            yield return str;
            yield break;
        }

        var i = 0;
        var maxIndex = str.Length - 1;
        var orderedIndexes = indexes.OrderBy(n => n);

        foreach (var index in orderedIndexes)
        {
            if (0 >= index) continue;

            if (i > maxIndex || index > maxIndex)
                yield break;

            yield return str.SubstringFromIndex(i, index - 1);
            i = index;
        }

        if (i < str.Length)
            yield return str.SubstringFromIndex(i, str.Length - 1);
    }

    /// <summary>
    /// searches a substring between to strings.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="inclusive">if true left and right are included.</param>
    /// <returns></returns>
    public static string? SubstringBetween(this string str, string left, string right, bool inclusive = true)
    {
        var indexes = IndicesOfAny(str, new[] { left, right }).ToArray();
        if (2 != indexes.Length) return null;

        var leftIndex = indexes[0];
        if (-1 == leftIndex) return null;

        var rightIndex = indexes[1];
        if (-1 == rightIndex) return null;

        rightIndex += right.Length - 1;
        if (!inclusive)
        {
            leftIndex += left.Length;
            if (leftIndex == rightIndex) return null;

            rightIndex -= right.Length;
            if (leftIndex > rightIndex) return null;
        }

        return SubstringFromIndex(str, leftIndex, rightIndex);
    }

    /// <summary>
    /// returns a substring between two strings. The search starts from the end.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="inclusive"></param>
    /// <returns></returns>
    public static string? SubstringBetweenFromEnd(this string str, string left, string right, bool inclusive = true)
    {
        var rightIndex = IndexFromEnd(str, right);
        if (-1 == rightIndex) return null;

        var leftIndex = IndexFromEnd(str, rightIndex - 1, left);
        if (-1 == leftIndex) return null;

        return inclusive 
            ? str[leftIndex..(rightIndex + right.Length)] 
            : str[(leftIndex + left.Length)..rightIndex];
    }


    public static string SubstringFromIndex(this string str, int start, int end)
    {
        if (null == str) throw new ArgumentNullException(nameof(str));
        if (start > end) throw new ArgumentOutOfRangeException(nameof(start), $"must not be greater than {nameof(end)}");
        if (str.Length < (start - 1)) throw new IndexOutOfRangeException(nameof(start));
        if (str.Length < (end - 1)) throw new IndexOutOfRangeException(nameof(end));

        return str.Substring(start, end - start + 1);
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ThrowIfNull(this string str, [CallerArgumentExpression("str")] string argumentName = "")
    {
        return str ?? throw new ArgumentNullException(argumentName); ;
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ThrowIfNullOrEmpty(this string str, [CallerArgumentExpression("str")] string argumentName = "")
    {
        if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(argumentName);
        return str;
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ThrowIfNullOrWhiteSpace(this string str, [CallerArgumentExpression("str")] string argumentName ="")
    {
        if (string.IsNullOrWhiteSpace(str)) throw new ArgumentNullException(argumentName);
        return str;
    }

    public static double? ToDoubleAsNullable(this string str)
    {
        if (double.TryParse(str, out double value))
            return value;

        return null;
    }

    public static Option<double> ToDoubleAsOption(this string str)
    {
        if (double.TryParse(str, out double value))
            return Option.Some(value);

        return Option.None<double>();
    }

    public static T? ToNullableIfNullOrEmpty<T>(this string str, Func<string, T> projection)
       where T : struct
    {
        projection.ThrowIfNull();

        if (string.IsNullOrEmpty(str)) return null;

        return projection(str);
    }

    public static T? ToNullableIfNullOrWhiteSpace<T>(this string str, Func<string, T> projection)
        where T : struct
    {
        projection.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(str)) return null;

        return projection(str);
    }

    public static object? ToNumber(this string str)
    {
        if (double.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out double doubleValue))
            return doubleValue;

        return null;
    }

    public static object? ToNumber(this string str, NumberStyles style, IFormatProvider provider)
    {
        provider.ThrowIfNull();

        if (double.TryParse(str, style, provider, out double doubleValue))
            return doubleValue;

        if (decimal.TryParse(str, style, provider, out decimal decimalValue))
            return decimalValue;

        return null;
    }

    public static Option<string> ToOptionIfNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str) ? Option.None<string>() : Option.Some(str);
    }

    public static Option<T> ToOptIfNullOrEmpty<T>(this string str, Func<string, T> projection)
    {
        projection.ThrowIfNull();

        return string.IsNullOrEmpty(str) ? Option.None<T>() : Option.Some(projection(str));
    }

    public static Option<string> ToOptionIfNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str) ? Option.None<string>() : Option.Some(str);
    }

    public static Option<T> ToOptionIfNullOrWhiteSpace<T>(this string str, Func<string, T> projection)
    {
        projection.ThrowIfNull();

        return string.IsNullOrWhiteSpace(str) ? Option.None<T>() : Option.Some(projection(str));
    }

    /// <summary>
    /// Removes \" and ' from string.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string TrimApostrophes(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        var apostrophes = new[] { '\'', '\"' };
        var start = 0;
        var length = str.Length;
        if (apostrophes.Any(c => c == str[0]))
        {
            start = 1;
            length--;
        }

        if (apostrophes.Any(c => c == str[^1]))
            length--;

        return str.Substring(start, length);
    }
}

