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

public class NamedIdFactory
    : IIdFactory<NamedId>
    , IIdentifiableFactory<string>
{
    private readonly Func<NamedId> _idFactory;

    public NamedIdFactory(string factoryId, Func<NamedId> idFactory)
    {
        FactoryId = factoryId.ThrowIfNullOrWhiteSpace();
        _idFactory = idFactory.ThrowIfNull();
    }

    public string FactoryId { get; }

    public NamedId NewId()
    {
        return _idFactory();
    }
}

public class NamedIdFactory<TId>
    : IIdFactory<NamedId<TId>>
    , IIdentifiableFactory<string>
    where TId : struct, IComparable<TId>, IEquatable<TId>
{
    private readonly Func<TId> _idFactory;

    public NamedIdFactory(string factoryId, Func<TId> idFactory)
    {
        FactoryId = factoryId.ThrowIfNullOrWhiteSpace();
        _idFactory = idFactory.ThrowIfNull();
    }

    public string FactoryId { get; }

    public NamedId<TId> NewId()
    {
        var id = _idFactory();
        return new NamedId<TId>(FactoryId, id);
    }
}