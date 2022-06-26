namespace Foundation;

/// <summary>
/// Can have three states: <see cref="State"/> can be IsNone or<typeparamref name="TState1"/> and <typeparamref name="TState2"/>.
/// </summary>
public struct TriState : IEquatable<TriState>
{
    public TriState(bool isTrue)
    {
        State = Opt.Some(isTrue);
    }

    public static bool operator ==(TriState left, TriState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TriState left, TriState right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is TriState other && Equals(other);

    public bool Equals(TriState other)
    {
        if (State.IsNone) return other.State.IsNone;

        return other.State.IsSome && State.Equals(other.State);
    }

    public override int GetHashCode()
    {
        if(State.IsNone) return 0;
        
        return State.OrThrow() ? 2 : 1;
    }
        
    public Opt<bool> State { get; }

    public override string ToString() => $"{State}";

    public bool TryGet(out bool state)
    {
        if(State.IsNone)
        {
            state = default;
            return false;
        }
        state = State.OrThrow();
        return true;
    }
}

/// <summary>
/// Can have three states: <see cref="NoState"/>, <typeparamref name="TState1"/> and <typeparamref name="TState2"/>.
/// </summary>
/// <typeparam name="TState1"></typeparam>
/// <typeparam name="TState2"></typeparam>
public struct TriState<TState1, TState2> : IEquatable<TriState<TState1, TState2>>
{

    public TriState(TState1 state1)
    {
        state1.ThrowIfNull();

        State1 = Opt.Some(state1);
        State2 = Opt.None<TState2>();
    }

    public TriState(TState2 state2)
    {
        state2.ThrowIfNull();

        State1 = Opt.None<TState1>(); 
        State2 = Opt.Some(state2);
    }

    public static bool operator ==(TriState<TState1, TState2> left, TriState<TState1, TState2> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TriState<TState1, TState2> left, TriState<TState1, TState2> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is TriState<TState1, TState2> other && Equals(other);

    public bool Equals(TriState<TState1, TState2> other)
    {
        if (NoState) return other.NoState;
        return State1.IsSome && other.State1.IsSome || State2.IsSome && other.State2.IsSome;
    }

    public override int GetHashCode()
    {
        if (NoState) return 0;

        return State1.IsSome? State1.GetHashCode() : State2.GetHashCode();
    }

    public TResult Match<TResult>(
        Func<TState1, TResult> fromState1, 
        Func<TState2, TResult> fromState2,
        Func<TResult> fromNoState)
    {
        if (State1.IsSome) return fromState1(State1.OrThrow());
        if (State2.IsSome) return fromState2(State2.OrThrow());

        return fromNoState();
    }

    public bool NoState => State1.IsNone && State2.IsNone;

    public Opt<TState1> State1 { get; }
    public Opt<TState2> State2 { get; }

    public override string ToString()
    {
        if (NoState) return $"{nameof(NoState)}";

        return State1.IsSome ? $"{State1}" : $"{State2}";
    }

    public bool TryGet(out TState1? state)
    {
        if (State1.IsSome)
        {
            state = State1.OrThrow();
            return true;
        }
        state = default;
        return false;
    }

    public bool TryGet(out TState2? state)
    {
        if (State2.IsSome)
        {
            state = State2.OrThrow();
            return true;
        }
        state = default;
        return false;
    }
}