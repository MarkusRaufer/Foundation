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
﻿namespace Foundation
{
    public static class TriStateExtensions
    {
        /// <summary>
        /// Returns a value if triState has a TState1 or TState2 state.
        /// </summary>
        /// <typeparam name="TState1"></typeparam>
        /// <typeparam name="TState2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="triState"></param>
        /// <param name="onState1">Is called if triState has a TState1 state.</param>
        /// <param name="onState2">Is called if triState has a TState2 state.</param>
        /// <param name="onNone">Is called if triState has no value.</param>
        /// <returns></returns>
        public static Option<TResult> Either<TState1, TState2, TResult>(
           this TriState<TState1, TState2> triState,
           Func<TState1, TResult> onState1,
           Func<TState2, TResult> onState2,
           Func<TResult>? onNone = null)
        {
            if (!triState.State.TryGet(out OneOf<TState1, TState2>? oneOf)) return Option.None<TResult>();
            
            if (oneOf!.TryGet(out TState1? state1)) return onState1(state1!);
            if (oneOf!.TryGet(out TState2? state2)) return onState2(state2!);

            return null != onNone ? onNone() : Option.None<TResult>();
        }

        /// <summary>
        /// Returns a value if triState has a TState1 state.
        /// </summary>
        /// <typeparam name="TState1"></typeparam>
        /// <typeparam name="TState2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="triState"></param>
        /// <param name="onState1">Is called if triState has a TState1 state.</param>
        /// <returns></returns>
        public static Option<TResult> OnState1<TState1, TState2, TResult>(
            this TriState<TState1, TState2> triState,
            Func<TState1, TResult> onState1)
        {
            if (triState.State.TryGet(out OneOf<TState1, TState2>? oneOf)
                && oneOf!.TryGet(out TState1? state1)) return onState1(state1!);

            return Option.None<TResult>();
        }

        /// <summary>
        /// Returns a value if triState has a TState2 state.
        /// </summary>
        /// <typeparam name="TState1"></typeparam>
        /// <typeparam name="TState2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="triState"></param>
        /// <param name="onState2">Is called if triState has a TState2 state.</param>
        /// <returns></returns>
        public static Option<TResult> OnState2<TState1, TState2, TResult>(
            this TriState<TState1, TState2> triState,
            Func<TState2, TResult> onState2)
        {
            if (triState.State.TryGet(out OneOf<TState1, TState2>? oneOf)
                && oneOf!.TryGet(out TState2? state2)) return onState2(state2!);

            return Option.None<TResult>();
        }
    }
}
