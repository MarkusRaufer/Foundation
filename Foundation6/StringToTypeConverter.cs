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
﻿using System.Reflection;

namespace Foundation;
public class StringToTypeConverter
{
    private readonly Assembly[] _assemblies;


    public StringToTypeConverter() : this(Assembly.GetExecutingAssembly().Location)
    {
    }

    public StringToTypeConverter(string assemblyLocation) : this(GetAssemblies(assemblyLocation))
    {
    }

    public StringToTypeConverter(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToArray();   
    }

    private static IEnumerable<Assembly> GetAssemblies(string location)
    {
        var dir = Path.GetDirectoryName(location);
        if (dir is null) yield break;

        var dlls = Directory.GetFiles(dir, "*.dll");
        foreach (var dll in dlls)
        {
            yield return Assembly.LoadFile(dll);
        }
    }

    public Type? GetType(string name)
    {
        var type = Type.GetType(name);
        if (type is null)
        {
            var span = name.AsSpan();
            var index = span.IndexOf('+');
            var typeName = span[(index + 1)..].ToString();

            type = _assemblies.SelectMany(x => x.GetTypes()).Where(x => x.Name == typeName).FirstOrDefault();
        }
        return type;
    }
}
