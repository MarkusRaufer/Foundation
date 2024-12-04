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
namespace Foundation.DesignPatterns.ChainOfResponsibility;

public static class ChainHandler
{
    public static ChainHandler<T, T> New<T>(Func<T, Option<T>> TryHandle)
        => new(TryHandle, null);

    public static ChainHandler<T, T> New<T>(Func<T, Option<T>> TryHandle, ChainHandler<T, T> Successor)
        => new(TryHandle, Successor);

    public static ChainHandler<TIn, TOut> New<TIn, TOut>(Func<TIn, Option<TOut>> TryHandle)
        => new(TryHandle, null);

    public static ChainHandler<TIn, TOut> New<TIn, TOut>(Func<TIn, Option<TOut>> TryHandle, ChainHandler<TIn, TOut> Successor)
        => new(TryHandle, Successor);
}

public record ChainHandler<TIn, TOut>(Func<TIn, Option<TOut>> TryHandle, ChainHandler<TIn, TOut>? Successor)
{
    public Option<TOut> Handle(TIn @in)
    {
        var result = TryHandle(@in);
        if (result.IsSome) return result;

        return Successor is null ? Option.None<TOut>() : Successor.Handle(@in);
    }
}
