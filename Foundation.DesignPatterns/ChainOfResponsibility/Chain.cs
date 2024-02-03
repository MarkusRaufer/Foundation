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
using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.ChainOfResponsibility;

public class Chain
{
    public static IEnumerable<ChainHandler<TIn, TOut>> Handlers<TIn, TOut>(IEnumerable<Func<TIn, Option<TOut>>> handlers)
    {
        var prevHandler = handlers.FirstOrDefault();
        if (prevHandler is null) yield break;

        var prevChainHandler = ChainHandler.New(prevHandler);
        yield return prevChainHandler;

        foreach (var handler in handlers.Skip(1))
        {
            prevChainHandler = ChainHandler.New(handler, prevChainHandler);
            yield return prevChainHandler;
        }
    }

    public static IEnumerable<ChainHandler<TIn, TOut>> Handlers<TIn, TOut>(IEnumerable<ChainHandler<TIn, TOut>> handlers)
    {
        return handlers.Pairs((lhs, rhs) =>
        {
            return (lhs, new ChainHandler<TIn, TOut>(rhs.TryHandle, lhs));
        });
    }
}
