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

public record TransactionControl(TransactionAction Action) 
    : TransactionControl<TransactionAction>(Action);

public record TransactionControl<TAction>(TAction Action) : ITransactionControl<TAction>;

public static class TransactionTuple
{
    public static TransactionTuple<TChange> New<TChange>(TransactionAction Action, TChange Change)
        => new(Action, Change);

    public static TransactionTuple<TAction, TChange> New<TAction, TChange>(TAction Action, TChange Change)
        => new(Action, Change);
}

public record TransactionTuple<TChange>(TransactionAction Action, TChange Change)
    : TransactionControl(Action);

public record TransactionTuple<TAction, TChange>(TAction Action, TChange Change)
    : TransactionControl<TAction>(Action)
    , ITransactionTuple<TAction, TChange>;
