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
﻿using Foundation;
using System.Reflection;

namespace Foundation.Reflection;

public class PrimitiveObjectTypeConverter : ITypeNameValueConverter
{
    private static readonly Type _dateOnlyType = typeof(DateOnly);
    private static readonly Type _dateTimeType = typeof(DateTime);
    private static readonly Type _guidType = typeof(Guid);
    private static readonly Type _timeOnlyType = typeof(TimeOnly);

    private readonly Assembly[] _assemblies;

    public PrimitiveObjectTypeConverter() : this(Assembly.GetExecutingAssembly().Location)
    {
    }

    public PrimitiveObjectTypeConverter(string location)
        : this(AssemblyEx.GetAssembliesFromLocation(location))
    {
    }

    public PrimitiveObjectTypeConverter(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToArray();
    }

    public Result<object?, Error> Convert(string objectType, object? value)
    {
        if (string.IsNullOrEmpty(objectType)) return Result.Error<object?, Error>(Error.FromException(new ArgumentNullException(nameof(objectType))));

        var type = TypeHelper.GetFromString(objectType, _assemblies);
        if (type is null)
        {
            return Result.Error<object?, Error>(Error.FromException(new TypeLoadException(objectType)));
        }
        try
        {
            switch(type)
            {
                case Type x when x == _dateOnlyType && value is string str:
                    var dateOnly = DateOnly.Parse(str);
                    return Result.Ok<object?, Error>(dateOnly);
                case Type x when x == _dateTimeType && value is string str:
                    var dateTime = DateTime.Parse(str);
                    return Result.Ok<object?, Error>(dateTime);
                case Type x when x == _guidType && value is string str:
                    var guid = Guid.Parse(str);
                    return Result.Ok<object?, Error>(guid);
                case Type x when x == _timeOnlyType && value is string str:
                    var timeOnly = TimeOnly.Parse(str);
                    return Result.Ok<object?, Error>(timeOnly);
            }

            var convertedValue = System.Convert.ChangeType(value, type);
            return Result.Ok<object?, Error>(convertedValue);
        }
        catch (Exception ex)
        {
            return Result.Error<object?, Error>(Error.FromException(ex));
        }
    }
}
