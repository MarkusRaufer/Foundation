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
﻿using Foundation.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO;

public class BinaryObjectWriter
{
    private readonly MemberInfoCash _memberCash;

    public BinaryObjectWriter(Type type) : this(new MemberInfoCash(type))
    {
    }

    public BinaryObjectWriter(Type type, MemberInfo[] members) : this(new MemberInfoCash(type, members))
    {
    }

    public BinaryObjectWriter(Type type, string[] memberNames) : this(new MemberInfoCash(type, memberNames))
    {
    }

    public BinaryObjectWriter(MemberInfoCash memberCash)
    {
        _memberCash = memberCash.ThrowIfNull();
    }

    public void WriteObject<T>(Stream stream, T obj)
    {
        if (null == stream) throw new ArgumentNullException(nameof(stream));
        if (null == obj) throw new ArgumentNullException(nameof(obj));

        var writer = new BinaryWriter(stream);
        var members = _memberCash.GetMembers();

        writer.WriteObject(obj, members);
    }

    public static BinaryObjectWriter<T> New<T>(params Expression<Func<T, object>>[] members)
    {
        return new(new MemberInfoCash<T>(members));
    }
}

public class BinaryObjectWriter<T>
{
    private readonly MemberInfoCash<T> _memberCash;

    public BinaryObjectWriter() : this(new MemberInfoCash<T>(typeof(T).GetProperties()))
    {
    }

    public BinaryObjectWriter(MemberInfo[] members) : this(new MemberInfoCash<T>(members))
    {
    }

    public BinaryObjectWriter(string[] memberNames) : this(new MemberInfoCash<T>(memberNames))
    {
    }

    public BinaryObjectWriter(MemberInfoCash<T> memberCash)
    {
        _memberCash = memberCash.ThrowIfNull();
    }

    public void WriteObject(Stream stream, T obj)
    {
        if (null == stream) throw new ArgumentNullException(nameof(stream));
        if (null == obj) throw new ArgumentNullException(nameof(obj));

        var writer = new BinaryWriter(stream);
        var members = _memberCash.GetMembers();

        writer.WriteObject(obj, members);
    }
}
