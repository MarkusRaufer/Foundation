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
﻿namespace Foundation;

public abstract class DisjointTypesTupleBase<TTuple> : IEquatable<DisjointTypesTupleBase<TTuple>>
    where TTuple : notnull
{
    protected DisjointTypesTupleBase(TTuple tuple)
    {
        Tuple = tuple.ThrowIfNull();
    }

    public override bool Equals(object? obj) => Equals(obj as DisjointTypesTupleBase<TTuple>);

    public bool Equals(DisjointTypesTupleBase<TTuple>? other)
    {
        return null != other && Tuple.Equals(other.Tuple);
    }

    public abstract Option<T> Get<T>();

    public override int GetHashCode() => Tuple.GetHashCode();

    public override string ToString() => $"{Tuple}";

    protected TTuple Tuple { get; }
}

public class DisjointTypesTuple<T1> : DisjointTypesTupleBase<Tuple<T1>>
{
    public DisjointTypesTuple(T1 value) : base(System.Tuple.Create(value))
    {
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);

        return Option.None<T>();
    }
}

public class DisjointTypesTuple<T1, T2> : DisjointTypesTupleBase<Tuple<T1, T2>>
{
    public DisjointTypesTuple(T1 item1, T2 item2) : base(System.Tuple.Create(item1, item2))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);

        return Option.None<T>();
    }
}

public class DisjointTypesTuple<T1, T2, T3> : DisjointTypesTupleBase<Tuple<T1, T2, T3>>
{
    public DisjointTypesTuple(T1 item1, T2 item2, T3 item3) : base(System.Tuple.Create(item1, item2, item3))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);

        return Option.None<T>();
    }
}

public class DisjointTypesTuple<T1, T2, T3, T4> : DisjointTypesTupleBase<Tuple<T1, T2, T3, T4>>
{
    public DisjointTypesTuple(T1 item1, T2 item2, T3 item3, T4 item4) : base(System.Tuple.Create(item1, item2, item3, item4))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);

        return Option.None<T>();
    }
}

public class DisjointTypesTuple<T1, T2, T3, T4, T5> : DisjointTypesTupleBase<Tuple<T1, T2, T3, T4, T5>>
{
    public DisjointTypesTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(System.Tuple.Create(item1, item2, item3, item4, item5))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();
        item5.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);
        if (Tuple.Item5 is T item5) return Option.Some(item5);

        return Option.None<T>();
    }
}

public class DisjointTypesTuple<T1, T2, T3, T4, T5, T6> : DisjointTypesTupleBase<Tuple<T1, T2, T3, T4, T5, T6>>
{
    public DisjointTypesTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : base(System.Tuple.Create(item1, item2, item3, item4, item5, item6))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();
        item5.ThrowIfNull();
        item6.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);
        if (Tuple.Item5 is T item5) return Option.Some(item5);
        if (Tuple.Item6 is T item6) return Option.Some(item6);

        return Option.None<T>();
    }
}

public class DisjointTypesTuple<T1, T2, T3, T4, T5, T6, T7> : DisjointTypesTupleBase<Tuple<T1, T2, T3, T4, T5, T6, T7>>
{
    public DisjointTypesTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        : base(System.Tuple.Create(item1, item2, item3, item4, item5, item6, item7))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();
        item5.ThrowIfNull();
        item6.ThrowIfNull();
        item7.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);
        if (Tuple.Item5 is T item5) return Option.Some(item5);
        if (Tuple.Item6 is T item6) return Option.Some(item6);
        if (Tuple.Item7 is T item7) return Option.Some(item7);

        return Option.None<T>();
    }
}