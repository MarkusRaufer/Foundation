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
namespace Foundation;

using System.Diagnostics;

public static class FuncOption
{
    public static NoneFunc<T> None<T>()
    {
        return new NoneFunc<T>();
    }

    public static NoneFunc<T> None<T>(T value)
    {
        return new NoneFunc<T>(value);
    }

    public static SomeFunc<T> Some<T>(T value, bool isCalled = false)
    {
        return new SomeFunc<T>(value, isCalled);
    }
}

[DebuggerDisplay("HasValue={HasValue}, Value={Value}, IsCalled={IsCalled}")]
public abstract class FuncOption<T>
{
    protected FuncOption()
    {
    }

    protected FuncOption(T? value, bool isCalled = false)
    {
        if (null == value) return;

        Value = value;
        HasValue = true;
        IsCalled = isCalled;
    }

    public bool HasValue { get; protected set; }
    public bool IsCalled { get; protected set; }
    public T? Value { get; private set; }
}

[DebuggerDisplay("HasValue={HasValue}, Value={Value}")]
public class NoneFunc<T> : FuncOption<T>
{
    public NoneFunc()
    {
    }

    public NoneFunc(T? value)
        : base(value)
    {
    }
}

[DebuggerDisplay("HasValue={HasValue}, Value={Value}, IsCalled={IsCalled}")]
public class SomeFunc<T> : FuncOption<T>
{
    public SomeFunc(T? value, bool isCalled = false)
        : base(value, isCalled)
    {
    }
}
