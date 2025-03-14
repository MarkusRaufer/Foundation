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

namespace Foundation.DesignPatterns.Saga;

public class RollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse> 
    : RollbackProvider<TId, IIdentifiableTransactionRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>>
    , IRollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    where TId : notnull
{
    public RollbackProvider(
        IEnumerable<IIdentifiableTransactionRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>> strategies)
        : base(strategies)
    {
    }
}

public class RollbackProvider<TId, TStrategy> : IRollbackProvider<TId, TStrategy>
    where TId : notnull
    where TStrategy: IIdentifiable<TId>
{
    private readonly ICollection<TStrategy> _strategies;

    public RollbackProvider(IEnumerable<TStrategy> strategies)
    {
        _strategies = strategies.ThrowIfNull().ToArray();
    }

    public TStrategy GetRollbackStrategy(TId strategyId)
    {
        return _strategies.First(s => s.Id.Equals(strategyId));
    }
}
