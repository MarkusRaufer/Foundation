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
﻿// The MIT License (MIT)
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
using Foundation.Collections.Generic;
using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public static class DuckType
{

    public static Result<Error> Action(LambdaExpression actionSelector, object obj)
    {
        if (actionSelector is null)
            return Result.Error(Error.FromException(new ArgumentNullException(nameof(actionSelector))));

        if (obj is null) return Result.Error(Error.FromException(new ArgumentNullException(nameof(obj))));


        if (actionSelector.Body is not MethodCallExpression methodCall)
            return Result.Error(Error.FromException(new ArgumentException("actionSelector must select a method")));

        var type = obj.GetType();
        var methodInfo = type.GetMethod(methodCall.Method.Name, methodCall.Method.GetParameters().Select(p => p.ParameterType).ToArray());

        if (methodInfo is null)
            return Result.Error(Error.FromException(new ArgumentException($"{nameof(obj)} does not have method {methodCall.Method.Name}")));

        var parameters = methodCall.Arguments.Select(arg =>
        {
            var lambda = Expression.Lambda(arg);
            var compiled = lambda.Compile();
            return compiled.DynamicInvoke();
        }).ToArray();

        try
        {
            methodInfo.Invoke(obj, parameters);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Error(Error.FromException(ex));
        }
    }

    private static (MethodInfo? methodInfo, (int index, ParameterInfo outParm)[]? outParms) GetMethodInfo(
        IEnumerable<MethodInfo> methodInfos,
        int numberOfOutParameters)
    {
        foreach (var methodInfo in methodInfos)
        {
            var parameters = methodInfo.GetParameters()
                                       .Enumerate()
                                       .Where(tuple => tuple.item.IsOut)
                                       .ToArray();

            if (parameters.Length == 0) return default;

            if (numberOfOutParameters == 0) return (methodInfo, parameters);

            if (parameters.Length == numberOfOutParameters) return (methodInfo, parameters);
        }

        return default;
    }

    /// <summary>
    /// Calls a methodInfo on the given methodInfo name with the provided inParameters.
    /// </summary>
    /// <param name="obj">The object with the methodInfo that should be called.</param>
    /// <param name="methodName">The name of the methodInfo that should be called.</param>
    /// <param name="parameters">The inParameters of the methodInfo that should be called.</param>
    /// <returns>The value that is returned by the methodInfo with <paramref name="methodName"/>.</returns>
    public static NullableResult<object, Error> Method(object? obj, string methodName, params object[] parameters)
    {
        if (obj is null) return NullableResult.Error<object, Error>(Error.FromException(new ArgumentNullException(nameof(obj))));

        var duckType = obj.GetType();

        try
        {
            var methodInfo = duckType.GetMethods().FirstOrDefault(x => x.Name == methodName && x.GetParameters().Length == parameters.Length);
            if (methodInfo == null) return NullableResult.Error<object, Error>(Error.FromException(new InvalidOperationException($"method {methodName} not found")));

            if (methodInfo.ReturnType == typeof(void))
                return NullableResult.Error<object, Error>(Error.FromException(new InvalidOperationException($"method {methodName} does not return a value")));

            try
            {
                var result = methodInfo.Invoke(obj, parameters);
                return NullableResult.Ok(result);
            }
            catch (Exception ex)
            {
                return NullableResult.Error<object, Error>(Error.FromException(ex));
            }

        }
        catch (Exception ex)
        {
            return NullableResult.Error<object, Error>(Error.FromException(ex));
        }
    }

    /// <summary>
    /// Invokes a method specified by an expression on the given object with the provided inParameters and returns the
    /// result or an error if invocation fails.
    /// </summary>
    /// <remarks>If the expression does not select a valid method, or if the method invocation throws an
    /// exception, the returned result will contain an error. The caller should check the result for errors before using
    /// the value.</remarks>
    /// <typeparam name="T">The type of the object on which the method is to be invoked.</typeparam>
    /// <param name="methodSelector">An expression that selects the method to invoke on the object. Must reference a method of type <typeparamref
    /// name="T"/>.</param>
    /// <param name="obj">The object instance on which to invoke the selected method. Must be compatible with <typeparamref name="T"/>.</param>
    /// <param name="parameters">An array of arguments to pass to the invoked method. The number and types of arguments must match the method's
    /// signature.</param>
    /// <returns>A <see cref="NullableResult{Object, Error}"/> containing the result of the method invocation if successful;
    /// otherwise, an error describing the failure.</returns>
    public static NullableResult<object, Error> Method(LambdaExpression methodSelector, object obj)
    {
        if (methodSelector is null)
            return Result.Error(Error.FromException(new ArgumentNullException(nameof(methodSelector))));

        if (obj is null) return Result.Error(Error.FromException(new ArgumentNullException(nameof(obj))));


        if (methodSelector.Body is not MethodCallExpression methodCall)
            return Result.Error(Error.FromException(new ArgumentException("methodSelector must select a method")));

        var type = obj.GetType();
        var methodInfo = type.GetMethod(methodCall.Method.Name, methodCall.Method.GetParameters().Select(p => p.ParameterType).ToArray());

        if (methodInfo is null)
            return Result.Error(Error.FromException(new ArgumentException($"{nameof(obj)} does not have method {methodCall.Method.Name}")));

        var parameters = methodCall.Arguments.Select(arg =>
        {
            var lambda = Expression.Lambda(arg);
            var compiled = lambda.Compile();
            return compiled.DynamicInvoke();
        }).ToArray();

        try
        {
            var result = methodInfo.Invoke(obj, parameters);
            return NullableResult.Ok(result);
        }
        catch (Exception ex)
        {
            return NullableResult.Error<object, Error>(Error.FromException(ex));
        }
    }

    /// <summary>
    /// Attempts to get the value of a property. If successful the result contains the value. The value can be null.
    /// </summary>
    /// <param name="propertySelector">A lambda the selects a property. Exampe: (IDuckType x) => x.Count.</param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static NullableResult<object, Error> Property(LambdaExpression propertySelector, object obj)
    {
        if (propertySelector is null)
            return Result.Error(Error.FromException(new ArgumentNullException(nameof(propertySelector))));

        var member = propertySelector.GetMember();
        if (member is null || member.Member is not PropertyInfo propertyInfo)
            return NullableResult.Error<object, Error>(Error.FromException(new ArgumentException("propertySelector must select a property")));

        try
        {
            var duckType = obj.GetType();
            var property = duckType.GetProperty(propertyInfo.Name);
            if (property == null)
                return NullableResult.Error<object, Error>(Error.FromException(new InvalidOperationException($"property {propertyInfo.Name} not found")));

            var value = property.GetValue(obj);
            return NullableResult.Ok(value);
        }
        catch (Exception ex)
        {
            return NullableResult.Error<object, Error>(Error.FromException(ex));
        }
    }

    /// <summary>
    /// Attempts to invoke a method with the specified name on the given object, passing the provided <paramref name="inParameters"/>.
    /// If successful the out paramters are copied to <paramref name="outParameters"/>. <paramref name="outParameters"/> must have the size of the expected out paramters.
    /// </summary>
    /// <remarks>The target method must have at least one out parameter. If the call was successful the result returns Ok otherwise Error.</remarks>
    /// <param name="obj">The object instance on which to invoke the method. Cannot be null.</param>
    /// <param name="methodName">The name of the methodInfo to invoke. The methodInfo must exist on the object's type and return a boolean value.</param>
    /// <param name="outParameters">The array with the out parameters. Must have the size of the number of expected out parameters.</param>
    /// <param name="inParameters">An array of arguments to pass to the method.</param>
    /// <returns>true if the methodInfo was found, invoked successfully, and returned true; otherwise, false.</returns>
    public static NullableResult<object, Error> OutParameterMethod(string methodName, object obj, object[] outParameters, params object[] inParameters)
    {        
        if (string.IsNullOrWhiteSpace(methodName))
            return NullableResult.Error<object>(Error.FromException(new ArgumentNullException(nameof(methodName))));

        if (obj is null)
            return NullableResult.Error<object>(Error.FromException(new ArgumentNullException(nameof(obj))));

        if (outParameters is null)
            return NullableResult.Error<object>(Error.FromException(new ArgumentNullException(nameof(methodName))));

        if (outParameters.Length == 0)
            return NullableResult.Error<object>(Error.FromException(new ArgumentException($"must have at least sizeof 1", nameof(outParameters))));

        var type = obj.GetType();

        var (methodInfo, outParams) = GetMethodInfo(type.GetMethods().Where(x => x.Name == methodName), outParameters.Length);

        if (methodInfo is null || outParams is null)
            return NullableResult.Error<object>(Error.FromException(new ArgumentException($"{nameof(obj)} does not have method {methodName}")));

        try
        {
            object? placeHolder = null;

#pragma warning disable CS8601 // Possible null reference assignment.
            object[] parms = [..inParameters.Concat(outParams.Select(x => placeHolder))];
#pragma warning restore CS8601 // Possible null reference assignment.
            var result = methodInfo.Invoke(obj, parms);
            
            foreach (var (i, (parmsIndex, outParameter)) in outParams.Enumerate())
            {
                outParameters[i] = parms[parmsIndex];
            }
            return NullableResult.Ok(result);
        }
        catch (Exception ex)
        {
            return NullableResult.Error<object>(Error.FromException(ex));
        }
    }

    /// <summary>
    /// Attempts to invoke a method of <paramref name="obj"/> from the specified <paramref name="methodSelector"/>.
    /// If the method call was successful the result returns Ok and include the out parameters in <paramref name="outParameters"/> other result contains an Error.
    /// </summary>
    /// <param name="methodSelector">A lambda that selects the method. Example: (IDuckType x) => x.Remove(2). The lambda can not have an out parameter for this
    /// you need to define Out arguments. Example: (IDuckType x) => x.TryGetValue(2, new Out()). Add as much Out variable as out parameters are expected.</param>
    /// <param name="obj">The object instance on which to invoke the method. Cannot be null.</param>
    /// <param name="outParameters">The out parameters of the method call.</param>
    /// <returns></returns>
    public static NullableResult<object, Error> OutParameterMethod(LambdaExpression methodSelector, object obj, out object?[]? outParameters)
    {
        outParameters = null;
        if (methodSelector is null)
            return NullableResult.Error<object>(Error.FromException(new ArgumentNullException(nameof(methodSelector))));

        if (obj is null) return NullableResult.Error<object>(Error.FromException(new ArgumentNullException(nameof(obj))));


        if (methodSelector.Body is not MethodCallExpression methodCall)
            return NullableResult.Error<object>(Error.FromException(new ArgumentException("methodSelector must select a method")));

        var type = obj.GetType();

        var (origOutParms, origInParms) = methodCall.Method.GetParameters()
                                        .Enumerate()
                                        .Partition(tuple => tuple.item.ParameterType == typeof(Out),
                                                   tuple => tuple.ToArray(),
                                                   tuple => tuple.ToArray());

        if (origOutParms.Length == 0 || origOutParms is null)
            return NullableResult.Error<object>(Error.FromException(new ArgumentException($"{methodCall.Method.Name} has no out parameter")));

        // get method with out paramters of obj.
        var (methodInfo, outParams) = GetMethodInfo(type.GetMethods().Where(x => x.Name == methodCall.Method.Name), origOutParms.Length);

        if (methodInfo is null || outParams is null)
            return NullableResult.Error<object>(Error.FromException(new ArgumentException($"{nameof(obj)} has no method {methodCall.Method.Name} with matching out parameters")));

        var origParameters = methodCall.Arguments.Where(x =>x.Type != typeof(Out))
                                                 .Select(arg =>
        {
            var lambda = Expression.Lambda(arg);
            var compiled = lambda.Compile();
            return compiled.DynamicInvoke();
        }).ToArray();

        try
        {
            object? placeHolder = null;

#pragma warning disable CS8601 // Possible null reference assignment.

            //needs the size of input + output parameters
            object[] parms = [.. origParameters.Concat(origOutParms.Select(x => placeHolder))];
#pragma warning restore CS8601 // Possible null reference assignment.

            var result = methodInfo.Invoke(obj, parms);

            outParameters = new object[outParams.Length];

            foreach (var (i, (parmsIndex, outParameter)) in outParams.Enumerate())
            {
                outParameters[i] = parms[parmsIndex];
            }
            return NullableResult.Ok<object, Error>(result);
        }
        catch (Exception ex)
        {
            return NullableResult.Error<object, Error>(Error.FromException(ex));
        }
    }
}
