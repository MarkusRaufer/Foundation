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

public class Transaction : Transaction<Guid, Unit, Action>
{
    public Transaction() : this(Guid.NewGuid())
    {
    }

    public Transaction(Guid transactionId) : base(transactionId)
    {
    }

    public override void Commit()
    {
        Committed.Publish();
    }
}

/// <summary>
///  With this class you can record changes and on <see cref="Commit"/> the event <see cref="Committed"/> is called.
/// </summary>
/// <typeparam name="TChange">The recorded type of change.</typeparam>
public class Transaction<TChange> : Transaction<Guid, TChange>
{
    public Transaction() : this(Guid.NewGuid())
    {
    }

    public Transaction(Guid transactionId) : base(transactionId)
    {
    }
}

/// <summary>
/// With this class you can record changes and on <see cref="Commit"/> the event <see cref="Committed"/> is called.
/// </summary>
/// <typeparam name="TId">The identifier of the transaction.</typeparam>
/// <typeparam name="TChange">The recorded type of change.</typeparam>
public class Transaction<TId, TChange> : Transaction<TId, TChange, Action<IEnumerable<TChange>>>
    where TId : notnull
{
    public Transaction(TId transactionId) : base(transactionId)
    {
    }
}

public class Transaction<TId, TChange, TDelegate>
    : ITransaction<TId, TDelegate>
    , IEquatable<Transaction<TId, TChange, TDelegate>>
    where TId : notnull
    where TDelegate : Delegate
{
    private bool _disposed;

    public Transaction(TId transactionId)
    {
        TransactionId = transactionId;
    }

    public void Add(TChange change) => Changes.Add(change);

    protected IList<TChange> Changes { get; } = new List<TChange>();

    public virtual void Commit()
    {
        Committed.Publish(Changes);
    }

    public Event<TDelegate> Committed { get; } = new Event<TDelegate>();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Committed.Dispose();
                Changes.Clear();

            }
            _disposed = true;
        }
    }

    public override bool Equals(object? obj) => Equals(obj as Transaction<TId, TChange, TDelegate>);

    public bool Equals(Transaction<TId, TChange, TDelegate>? other)
    {
        return other != null && TransactionId.Equals(other.TransactionId);
    }

    public override int GetHashCode() => TransactionId.GetHashCode();

    public bool HasChanges => Changes.Count > 0;

    public override string ToString() => $"{nameof(TransactionId)}: {TransactionId}";

    public TId TransactionId { get; }
}
