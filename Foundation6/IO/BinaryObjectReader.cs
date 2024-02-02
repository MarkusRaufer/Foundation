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
﻿using Foundation.Collections.Generic;
using Foundation.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO;

public static class BinaryObjectReader
{
    public static BinaryObjectReader<T> New<T>(params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        return new(new MemberInfoCash<T>(members));
    }
}

public class BinaryObjectReader<T>
{
     private readonly MemberInfoCash<T> _memberCash;


    public BinaryObjectReader() : this(new MemberInfoCash<T>(typeof(T).GetProperties()))
    {
    }

    public BinaryObjectReader(MemberInfo[] members) 
        : this(new MemberInfoCash<T>(members))
    {
        members.ThrowIfNull();
    }

    public BinaryObjectReader(string[] memberNames)
        : this(new MemberInfoCash<T>(memberNames))
    {
    }

    public BinaryObjectReader(MemberInfoCash<T> memberCash)
    {
        _memberCash = memberCash.ThrowIfNull();
    }

    public IEnumerable<KeyValuePair<string, object>> ReadKeyValues(Stream stream)
    {
        return ReadMembers(stream).Select(tuple => Pair.New(tuple.member.Name, tuple.value));
    }
    public IEnumerable<(MemberInfo member, object value)> ReadMembers(Stream stream)
    {
        stream.ThrowIfNull();

        var reader = new BinaryReader(stream);
        return reader.ReadFromMembers(_memberCash.GetMembers());
    }

    public IEnumerable<(PropertyInfo property, object value)> ReadProperties(Stream stream)
    {
        stream.ThrowIfNull();

        var reader = new BinaryReader(stream);
        return reader.ReadFromProperties(_memberCash.GetMembers().OfType<PropertyInfo>());
    }
}
