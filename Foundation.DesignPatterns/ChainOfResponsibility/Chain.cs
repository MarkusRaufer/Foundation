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
using Foundation;
using System;

namespace Foundation.DesignPatterns.ChainOfResponsibility;

public static class Chain
{
    public static ChainHandler<T, T>? Create<T>(params Func<T, Option<T>>[] handlers)
    {
        return Create<T, T>(handlers);
    }

    public static ChainHandler<T, T>? Create<T>(IEnumerable<Func<T, Option<T>>> handlers)
    {
        return Create<T, T>(handlers);
    }

    public static ChainHandler<TIn, TOut>? Create<TIn, TOut>(params Func<TIn, Option<TOut>>[] handlers)
    {
        return Create((IEnumerable<Func<TIn, Option<TOut>>>)handlers);
    }

    public static ChainHandler<TIn, TOut>? Create<TIn, TOut>(IEnumerable<Func<TIn, Option<TOut>>> handlers)
    {
        ChainHandler<TIn, TOut>? next = null;

        foreach (var handler in handlers.Reverse())
        {
            if (null == next)
            {
                next = ChainHandler.New(handler);
                continue;
            }

            var chainHandler = ChainHandler.New(handler, next);
            next = chainHandler;
        }

        return next;
    }
}
