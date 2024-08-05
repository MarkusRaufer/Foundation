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

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T AssignIfNull<T>(this object? obj, Func<T> creator)
    {
        if (obj is T t) return t;

        creator.ThrowIfNull();

        return creator();
    }

    /// <summary>
    /// Generic cast works on reference types and value types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T? CastTo<T>(this object obj)
    {
        if (obj is T t) return t;
        return default;
    }

    /// <summary>
    /// Compares to objects. Allows that lhs and rhs are null without throwing an exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<int, Error> CompareToNullable<T>(this T? lhs, T? rhs)
    {
        if (lhs is null) return rhs is null ? Result.Ok(0) : Result.Ok(-1);
        if (rhs is null) return Result.Ok(1);

        if (lhs is IComparable<T> lhsComparable) return Result.Ok(lhsComparable.CompareTo(rhs));

        if (rhs is IComparable<T> rhsComparable) return Result.Ok(rhsComparable.CompareTo(lhs));

       return Result.Error<int>(Error.FromException(new ArgumentException($"at least one side must implement IComparable<{nameof(T)}>")));
    }

    /// <summary>
    /// Converts an object to a target type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? ConvertTo<T>(this object? obj)
    {
        if (null == obj) return default;

        if (obj is T target) return target;

        var targetType = typeof(T);
        var objectType = obj.GetType();

        {
            if (TypeDescriptor.GetConverter(obj) is TypeConverter converter)
            {
                if (converter.CanConvertTo(targetType) && converter.ConvertTo(obj, targetType) is T t) return t;
            }
        }

        {
            if (TypeDescriptor.GetConverter(targetType) is TypeConverter converter)
            {
                if (converter.CanConvertFrom(objectType) && converter.ConvertFrom(obj) is T t) return t;
            }
        }

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? ConvertTo(this object? obj, Type targetType)
    {
        targetType.ThrowIfNull();

        if (null == obj) return null;

        var objectType = obj.GetType();
        if (objectType == targetType) return obj;

        var converter = TypeDescriptor.GetConverter(obj);
        if (null == converter) return null;

        if (converter.CanConvertTo(targetType))
        {
            var converted = converter.ConvertTo(obj, targetType);
            if (converted is not null) return converted;
        }

        converter = TypeDescriptor.GetConverter(targetType);
        if (null == converter) return null;

        if (converter.CanConvertFrom(objectType))
        {
            var converted = converter.ConvertFrom(obj);
            if (converted is not null) return converted;
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? DynamicCastTo(this object? obj, params Type[] typeArgs)
    {
        if (null == obj) return null;

        var castMethod = typeof(ObjectExtensions).GetMethod("CastTo")?.MakeGenericMethod(typeArgs);
        return castMethod?.Invoke(null, new[] { obj });
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult EitherNullable<T, TResult>(this T? item, Func<T, TResult> notNull, Func<TResult> isNull)
    {
        notNull.ThrowIfNull();
        isNull.ThrowIfNull();

        if (item is T t)
        {
            var result = notNull(t);
            return result ?? throw new ArgumentNullException(nameof(isNull), $"{nameof(notNull)} returned null");
        }
        var alternative = isNull();
        return alternative ?? throw new ArgumentNullException(nameof(isNull), $"{nameof(isNull)} returned null");
    }

    /// <summary>
    /// Checks equality of two objects. Allows that lhs and rhs are null without throwing an exception.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>True if both sides are equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsNullable<T>(this T? lhs, T? rhs)
    {
        return EqualsNullable(lhs, rhs, (x, y) => x!.Equals(y));
    }

    /// <summary>
    /// Checks equality of two objects. Allows that lhs and rhs are null without throwing an exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="equals">Overrides the standard equality comparer.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsNullable<T>(this T? lhs, T? rhs, Func<T, T, bool> equals)
    {
        if (ReferenceEquals(lhs, rhs)) return true;

        if (lhs is null) return rhs is null;
        if (rhs is null) return false;
        if (lhs.GetHashCode() != rhs.GetHashCode()) return false;

        return equals(lhs, rhs);
    }

    /// <summary>
    /// Casts dynamically to a generic type. The first type must be a generic type. eg: List<>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="typeArguments"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? GenericDynamicCastTo(this object? obj, params Type[] typeArguments)
    {
        if (null == obj) return null;

        var genericType = obj.GetType().CreateGenericType(typeArguments);

        return DynamicCastTo(obj, genericType);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetInstanceHashCode(this object? obj) => RuntimeHelpers.GetHashCode(obj);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNullableHashCode(this object? obj) => (obj is null) ? 0 : obj.GetHashCode();

    /// <summary>
    /// Checks if an object is of a generic type.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type">e.g. IList<> (without definded generic parameter.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOfGenericType(this object obj, Type type)
    {
        obj.ThrowIfNull();
        type.ThrowIfNull();

        var objType = obj.GetType();
        if (!objType.IsGenericType) return false;

        var generic = objType.GetGenericTypeDefinition();
        return generic == type;
    }

  
    /// <summary>
    /// Returns a default value when <paramref name="obj"/> is null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="obj">The value that can be null.</param>
    /// <param name="default">The default value that may not be null.</param>
    /// <param name="paramName">the name of the parameter which is by default the caller name.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T OrDefault<T>(this T? obj, [NotNull] Func<T> @default, [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        if (@default is null) throw new ArgumentNullException(nameof(@default));

        if (obj is null)
        {
            if (@default() is T value) return value;
            throw new ArgumentNullException(paramName, $"{nameof(@default)} may not be null");
        }
        return obj;
    }

    /// <summary>
    /// Returns a transformed value when value is not null or a transformed default value when <paramref name="obj"/> is null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TResult">The type of the transformed value.</typeparam>
    /// <param name="obj">The value.</param>
    /// <param name="notNull">Is called when value is not null.</param>
    /// <param name="default">Is called when value is null.</param>
    /// <param name="paramName">The callers name.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult OrDefault<T, TResult>(this T? obj, [NotNull] Func<T, TResult> notNull, [NotNull] Func<TResult> @default, [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        if (notNull is null) throw new ArgumentNullException(nameof(notNull));
        if (@default is null) throw new ArgumentNullException(nameof(@default));

        if (obj is null)
        {
            if (@default() is TResult value) return value;
            throw new ArgumentNullException(paramName, $"{nameof(@default)} may not return null");
        }
        if (notNull(obj) is TResult result) return result;

        throw new ArgumentNullException(paramName, $"{nameof(notNull)} may not return null");
    }

    /// <summary>
    /// Throws an exception if <paramref name="predicate"/> returns true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="predicate"></param>
    /// <param name="message"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIf<T>(
        this T? obj,
        Func<bool> predicate,
        string? message = null,
        [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        predicate.ThrowIfNull();

        Func<ArgumentException> exception = message is null 
            ? () => new ArgumentException(paramName) 
            : () => new ArgumentException(message, paramName);

        return predicate() ? throw exception() : obj.ThrowIfNull();
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIf<T>(
        this T? obj,
        Func<bool> predicate,
        Func<Exception> exception)
    {
        predicate.ThrowIfNull();
        exception.ThrowIfNull();

        return predicate() ? throw exception() : obj.ThrowIfNull();
    }

    /// <summary>
    /// Throws ArgumentNullException if obj is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfNull<T>(this T? obj, [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        return ThrowIfNull(obj, () => new ArgumentNullException(paramName));
    }

    /// <summary>
    /// Throws Exception if <paramref name="obj"/> is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">Object that can be null.</param>
    /// <param name="paramName">The name of the caller.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfNull<T>(this T? obj, Func<Exception> exeption, [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        return obj ?? throw exeption();
    }

    /// <summary>
    /// Throws ArgumentOutOfRangeException if outOfRangePredicate returns true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="outOfRangePredicate"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfOutOfRange<T>(
        this T obj,
        Func<bool> outOfRangePredicate,
        [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        return outOfRangePredicate() ? throw new ArgumentOutOfRangeException(paramName) : obj.ThrowIfNull();
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfOutOfRange<T>(
        this T obj,
        Func<bool> outOfRangePredicate,
        Func<string> message,
        [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        message.ThrowIfNull();

        if (outOfRangePredicate()) throw new ArgumentOutOfRangeException(paramName, message());
        
        return obj.ThrowIfNull();
    }

    /// <summary>
    /// Throws ArgumentOutOfRangeException if outOfRangePredicate returns true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="outOfRangePredicate"></param>
    /// <param name="min">min value in message.</param>
    /// <param name="max">max value in message.</param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfOutOfRange<T>(
        this T obj,
        Func<bool> outOfRangePredicate,
        T min,
        T max,
        [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        return outOfRangePredicate() 
            ? throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be between {min} and {max}.")
            : obj.ThrowIfNull();
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfOutOfRange<T>(
        this T obj,
        Func<bool> isOutOfRange,
        string message,
        [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        return isOutOfRange() ? throw new ArgumentOutOfRangeException(paramName, message) : obj.ThrowIfNull();
    }

    /// <summary>
    /// transforms an object to a boolean if it is convertible.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="anyValue">if true, it tries to convert the object to byte, number or string and checks if it has the value 0 respectively True or False.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBool(this object? obj, bool anyValue = false)
    {
        if (null == obj) return false;

        if (obj is bool boolean) return boolean;
        if (!anyValue) return false;

        return obj switch
        {
            byte b => 0 != b,
            decimal m => 0 != m,
            double d => 0 != d,
            float f => 0 != f,
            Int16 i16 => 0 != i16,
            UInt16 ui16 => 0 != ui16,
            Int32 i32 => 0 != i32,
            UInt32 ui32 => 0 != ui32,
            Int64 i64 => 0 != i64,
            UInt64 ui64 => 0 != ui64,
            string str => bool.TryParse(str, out bool result) && result,
            _ => false
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ToByteArray(this object? obj, Encoding? encoding = null)
    {
        return obj switch
        {
            bool b => BitConverter.GetBytes(b),
            byte b => [b],
            char c => BitConverter.GetBytes(c),
            DateTime dt => BitConverter.GetBytes(dt.Ticks),
            decimal m => m.ToByteArray(),
            double d => BitConverter.GetBytes(d),
            float f => BitConverter.GetBytes(f),
            Int16 i16 => BitConverter.GetBytes(i16),
            Int32 i32 => BitConverter.GetBytes(i32),
            Int64 i64 => BitConverter.GetBytes(i64),
            UInt16 ui16 => BitConverter.GetBytes(ui16),
            UInt32 ui32 => BitConverter.GetBytes(ui32),
            UInt64 ui64 => BitConverter.GetBytes(ui64),
#if NET6
            SByte sb => BitConverter.GetBytes(sb),
#elif NET8
            SByte sb => BitConverter.GetBytes((sbyte)sb),
#endif
            string str => null != encoding ? encoding.GetBytes(str) : Encoding.UTF8.GetBytes(str),
            _ => []
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? ToDateTime(this object? value)
    {
        return value switch
        {
#if NET6_0_OR_GREATER
            DateOnly d => d.ToDateTime(),
            TimeOnly to => to.ToDateTime(),
#endif
            DateTime dt => dt,
            int i => new DateTime(i),
            long l => new DateTime(l),
            _ => null
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? ToDateTime(this object? value, DateTimeKind kind)
    {
        return value switch
        {
#if NET6_0_OR_GREATER
            DateOnly d => d.ToDateTime(kind),
            TimeOnly to => to.ToDateTime(kind),
#endif
            DateTime dt => new DateTime(dt.Ticks, kind),
            int i => new DateTime(i, kind),
            long l => new DateTime(l, kind),
            _ => null
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? ToInt32(this object? value)
    {
        if(value is int number) return number;

        if (value is string str && int.TryParse(str, out int result))
            return result;

        if (value is byte[] buffer && 4 == buffer.Length)
        {
#if NETSTANDARD2_0
            return BitConverter.ToInt32(buffer, 0);
#else
            return BitConverter.ToInt32(buffer);
#endif
        }
            

        return null;
    }

    /// <summary>
    /// Returns Some(T) if <paramref name="obj"/> <typeparamref name="T"/> otherwise None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T>(this T? value) => Option.Maybe(value);

    /// <summary>
    /// Transforms <paramref name="value"/> into a <see cref="Result{TOk, TError}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TOk">Is called when value is not null.</typeparam>
    /// <typeparam name="TError">Is called when value is null.</typeparam>
    /// <param name="value"></param>
    /// <param name="onOk"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public static Result<TOk, TError> ToResult<T, TOk, TError>(this T? value, Func<T, TOk> onOk, Func<TError> onError)
    {
        if (value is not null) return Result.Ok<TOk, TError>(onOk(value));
        return Result.Error<TOk, TError>(onError());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringOrEmpty(this object? obj)
    {
        if (obj is null) return String.Empty;

        return obj.ToString() ?? String.Empty;
    }

    /// <summary>
    /// Returns a value of type <typeparamref name="T"/> or null.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="obj">The object which is casted to <typeparamref name="T"/></param>
    /// <param name="paramName"></param>
    /// <returns>Value of type <typeparamref name="T"/> if compatible otherwise null</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? ToType<T>(this object? obj, [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        return obj is T t ? t : default;
    }

    /// <summary>
    /// Returns a value of type <typeparamref name="TResult"/> or null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="obj"></param>
    /// <param name="exception"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult? ToType<T, TResult>(this T? obj, Func<T, TResult> project, [CallerArgumentExpression(nameof(obj))] string paramName = "")
    {
        if (obj is null) return default;

        return project(obj);
    }
}

