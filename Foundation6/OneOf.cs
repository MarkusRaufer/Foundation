namespace Foundation;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class OneOf<T1, T2> : IEquatable<OneOf<T1, T2>>
{
    protected OneOf()
    {
    }

    public OneOf(T1 t1)
    {
        t1.ThrowIfNull();
        Item1 = Opt.Some(t1);

        OrdinalIndex = 1;
        SelectedType = typeof(T1);
        Value = t1!;
    }

    public OneOf(T2 t2)
    {
        t2.ThrowIfNull();
        Item2 = Opt.Some(t2);
        
        OrdinalIndex = 2;
        SelectedType = typeof(T2);
        Value = t2!;
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2>);

    public bool Equals(OneOf<T1, T2>? other)
    {
        return null != other 
            && SelectedType!.Equals(other.SelectedType)
            && Value!.Equals(other.Value);
    }

    public override int GetHashCode() => System.HashCode.Combine(SelectedType, Value);
    
    public void Invoke(Action<T1> action)
    {
        if(Value is T1 value) action(value);
    }

    public void Invoke(Action<T2> action)
    {
        if (Value is T2 value) action(value);
    }

    public Opt<T1> Item1 { get; }

    public Opt<T2> Item2 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> t1Projection,
        Func<T2, TResult> t2Projection)
    {
        return Value switch
        {
            T1 t1 => t1Projection(t1),
            T2 t2 => t2Projection(t2),
            _ => throw new NotSupportedException()
        };
    }

    public int OrdinalIndex { get; protected set; }

    public virtual bool TryGet<T>([MaybeNullWhen(false)] out T? value)
    {
        if (0 == OrdinalIndex)
        {
            value = default;
            return false;
        }

        static Opt<T> setValue<TItem>(TItem item)
        {
            return item is T t ? Opt.Some(t) : Opt.None<T>();
        }

        var foundValue = OrdinalIndex switch
        {
            1 => setValue(Item1.OrThrow()),
            2 => setValue(Item2.OrThrow()),
            _ => Opt.None<T>()
        };

        value = foundValue.IsSome ? foundValue.OrThrow() : default;
        return foundValue.IsSome;
    }

    public Type? SelectedType { get; protected set; }

    public override string ToString() => SelectedType!.ToString();

    public object? Value { get; protected set; }
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class OneOf<T1, T2, T3> : OneOf<T1, T2>
{
    protected OneOf()
    {
    }

    public OneOf(T1 t1) : base(t1)
    {
    }

    public OneOf(T2 t2) : base(t2)
    {
    }

    public OneOf(T3 t3)
    {
        t3.ThrowIfNull();
        Item3 = Opt.Some(t3);

        OrdinalIndex = 3;
        SelectedType = typeof(T3);
        Value = t3!;
    }

    public void Invoke(Action<T3> action)
    {
        if (Value is T3 value) action(value);
    }

    public Opt<T3> Item3 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> t1Projection,
        Func<T2, TResult> t2Projection,
        Func<T3, TResult> t3Projection)
    {
        return Value switch
        {
            T1 t1 => t1Projection(t1),
            T2 t2 => t2Projection(t2),
            T3 t3 => t3Projection(t3),
            _ => throw new NotSupportedException()
        };
    }

    public override bool TryGet<T>([MaybeNullWhen(false)] out T value)
    {
        if (3 == OrdinalIndex)
        {
            if (Item3.OrThrow() is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
        }
        return base.TryGet(out value);
    }
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
public class OneOf<T1, T2, T3, T4> : OneOf<T1, T2, T3>
{
    protected OneOf()
    {
    }

    public OneOf(T1 t1) : base(t1)
    {
    }

    public OneOf(T2 t2) : base(t2)
    {
    }

    public OneOf(T3 t3) : base(t3)
    {
    }

    public OneOf(T4 t4)
    {
        t4.ThrowIfNull();
        Item4 = Opt.Some(t4);

        OrdinalIndex = 4;
        SelectedType = typeof(T4);
        Value = t4!;
    }

    public void Invoke(Action<T4> action)
    {
        if (Value is T4 value) action(value);
    }

    public Opt<T4> Item4 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> t1Projection,
        Func<T2, TResult> t2Projection,
        Func<T3, TResult> t3Projection,
        Func<T4, TResult> t4Projection)
    {
        return Value switch
        {
            T1 t1 => t1Projection(t1),
            T2 t2 => t2Projection(t2),
            T3 t3 => t3Projection(t3),
            T4 t4 => t4Projection(t4),
            _ => throw new NotSupportedException()
        };
    }

    public override bool TryGet<T>([MaybeNullWhen(false)] out T value)
    {
        if (4 == OrdinalIndex)
        {
            if (Item4.OrThrow() is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
        }
        return base.TryGet(out value);
    }
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
public class OneOf<T1, T2, T3, T4, T5> : OneOf<T1, T2, T3, T4>
{
    public OneOf(T5 t5)
    {
        t5.ThrowIfNull();
        Item5 = Opt.Some(t5);

        OrdinalIndex = 5;
        SelectedType = typeof(T5);
        Value = t5!;
    }

    public void Invoke(Action<T5> action)
    {
        if (Value is T5 value) action(value);
    }

    public Opt<T5> Item5 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> t1Projection,
        Func<T2, TResult> t2Projection,
        Func<T3, TResult> t3Projection,
        Func<T4, TResult> t4Projection,
        Func<T5, TResult> t5Projection)
    {
        return Value switch
        {
            T1 t1 => t1Projection(t1),
            T2 t2 => t2Projection(t2),
            T3 t3 => t3Projection(t3),
            T4 t4 => t4Projection(t4),
            T5 t5 => t5Projection(t5),
            _ => throw new NotSupportedException()
        };
    }

    public override bool TryGet<T>([MaybeNullWhen(false)] out T value)
    {
        if (5 == OrdinalIndex)
        {
            if (Item5.OrThrow() is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
        }
        return base.TryGet(out value);
    }
}
