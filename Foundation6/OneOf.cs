using System.Data.Common;

namespace Foundation;

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

    /// <summary>
    /// Sets Value to T1 t1.
    /// </summary>
    /// <param name="t1"></param>
    public OneOf(T1 t1)
    {
        t1.ThrowIfNull();
        Item1 = t1;
        ItemIndex = 1;

        SelectedType = typeof(T1);
        HashCode = System.HashCode.Combine(SelectedType, t1);
    }

    /// <summary>
    /// Sets Value to T2 t2.
    /// </summary>
    /// <param name="t2"></param>
    public OneOf(T2 t2)
    {
        t2.ThrowIfNull();
        Item2 = t2;
        ItemIndex = 2;

        SelectedType = typeof(T2);
        HashCode = System.HashCode.Combine(SelectedType, t2);
    }

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1 or T2.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2)
    {
        return ItemIndex switch
        {
            1 => Item1 is T1 t1
                    ? onT1(t1)
                    : throw new ArgumentException($"{typeof(T1).Name}"),
            2 => Item2 is T2 t2
                    ? onT2(t2)
                    : throw new ArgumentException($"{typeof(T2).Name}"),
            _ => throw new ArgumentException($"ItemIndex = {ItemIndex}")
        };
    }

    public static implicit operator OneOf<T1, T2>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2>(T2 value) => new(value);

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2>);

    public bool Equals(OneOf<T1, T2>? other)
    {
        if (null == other || ItemIndex != other.ItemIndex) return false;

        return ItemIndex switch
        {
            1 => Item1.EqualsNullable(other.Item1),
            2 => Item2.EqualsNullable(other.Item2),
            _ => false
        };
    }

    public override int GetHashCode() => HashCode;

    protected int HashCode { get; set; }

    /// <summary>
    /// Invokes action if Value is of type T1.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T1> action)
    {
        if (TryGet(out T1? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes action if Value is of type T2.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T2> action)
    {
        if (TryGet(out T2? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes an action only if the selected type matches the specific type.
    /// </summary>
    /// <param name="onT1">Is called if the selected type is of type T1.</param>
    /// <param name="onT2">Is called if the selected type is of type T2.</param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2)
    {
        return Invoke(onT1) || Invoke(onT2);
    }

    /// <summary>
    /// Returns true is the selected type if of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsOfType<T>() => SelectedType == typeof(T);

    public T1? Item1 { get; }

    public T2? Item2 { get; }

    /// <summary>
    /// Returns the index of the selected item.
    /// 0: nothing selected
    /// 1: Item1 selected
    /// 2: Item2 selected
    /// </summary>
    public int ItemIndex { get; protected set; }

    public virtual bool TryGet<T>(out T? value)
    {
        switch(ItemIndex)
        {
            case 1:
                if (Item1 is T t1Value)
                {
                    value = t1Value;
                    return true;
                }
                break;
            case 2:
                if (Item2 is T t2Value)
                {
                    value = t2Value;
                    return true;
                }
                break;
        }

        value = default;
        return false;
    }

    public Type? SelectedType { get; protected set; }

    public override string ToString() => SelectedType!.ToString();
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class OneOf<T1, T2, T3> 
    : OneOf<T1, T2>
    , IEquatable<OneOf<T1, T2, T3>>
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
        Item3 = t3;
        ItemIndex = 3;

        SelectedType = typeof(T3);
        HashCode = System.HashCode.Combine(SelectedType, t3);
    }

    public static implicit operator OneOf<T1, T2, T3>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3>(T3 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1, T2 or T3.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3)
    {
        if(3 == ItemIndex)
        {
            if (Item3 is not T3 t3) throw new ArgumentException($"{typeof(T3).Name}");

            return onT3(t3);
        }
        return Either(onT1, onT2);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3>);

    public bool Equals(OneOf<T1, T2, T3>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item3.EqualsNullable(other.Item3);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T3.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T3> action)
    {
        if (TryGet(out T3? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3)
    {
        return Invoke(onT1, onT2) || Invoke(onT3);
    }

    public T3? Item3 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (3 == ItemIndex)
        {
            if (Item3 is T t)
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
        Item4 = t4;
        ItemIndex = 4;

        SelectedType = typeof(T4);
        HashCode = System.HashCode.Combine(SelectedType, t4);
    }

    public static implicit operator OneOf<T1, T2, T3, T4>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4>(T4 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1, T2, T3 or T4.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Throw an exception if it is not of any available type</exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4)
    {
        if (4 == ItemIndex)
        {
            if (Item4 is not T4 t4) throw new ArgumentException($"{typeof(T4).Name}");

            return onT4(t4);
        }
        return Either(onT1, onT2, onT3);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4>);

    public bool Equals(OneOf<T1, T2, T3, T4>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item4.EqualsNullable(other.Item4);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T4.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T4> action)
    {
        if (TryGet(out T4? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4)
    {
        return Invoke(onT1, onT2, onT3) || Invoke(onT4);
    }

    public T4? Item4 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if(4 == ItemIndex)
        {
            if (Item4 is T t)
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

    public OneOf(T4 t4) : base(t4)
    {
    }

    public OneOf(T5 t5)
    {
        t5.ThrowIfNull();
        Item5 = t5;
        ItemIndex = 5;

        SelectedType = typeof(T5);
        HashCode = System.HashCode.Combine(SelectedType, t5);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T5 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1, T2, T3, T4 or T5.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5)
    {
        if (5 == ItemIndex)
        {
            if (Item5 is not T5 t5) throw new ArgumentException($"{typeof(T5).Name}");

            return onT5(t5);
        }
        return Either(onT1, onT2, onT3, onT4);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item5.EqualsNullable(other.Item5);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T5.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T5> action)
    {
        if (TryGet(out T5? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5)
    {
        return Invoke(onT1, onT2, onT3, onT4) || Invoke(onT5);
    }

    public T5? Item5 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (5 == ItemIndex)
        {
            if (Item5 is T t)
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
