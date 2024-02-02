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
﻿namespace Foundation.ComponentModel;

/// <summary>
/// Contract of a factory that uses a delegate to create an id.
/// </summary>
/// <typeparam name="TFunc">Should be a functor that returns a value.</typeparam>
public interface IDelegateIdFactory<TFunc>
    where TFunc : Delegate
{
    TFunc CreateId { get; }
}

/// <summary>
/// Contract of a factory that uses a delegate to create an id. The lambda expects an input value to create an id.
/// </summary>
/// <typeparam name="TIdSelector">An input value to create an id.</typeparam>
/// <typeparam name="TId">Type of the id.</typeparam>
public interface IDelegateIdFactory<TIdSelector, TId> : IDelegateIdFactory<Func<TIdSelector, TId>>
{
}

/// <summary>
/// Contract of a factory that uses a delegate to create an id. The lambda expects an input value to create an id.
/// </summary>
/// <typeparam name="TFactoryId">The identifier of the factory.</typeparam>
/// <typeparam name="TIdSelector">An input value to create an id.</typeparam>
/// <typeparam name="TId">Type of the id.</typeparam>
public interface IDelegateIdFactory<TFactoryId, TIdSelector, TId> 
    : IDelegateIdFactory<TIdSelector, TId>
    , IIdentifiableFactory<TFactoryId>
{
}
