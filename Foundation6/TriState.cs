namespace Foundation;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// can be IsTrue, IsFalse or IsNone. IsNone means that it is neither true nor false.
/// </summary>
public struct TriState : IEquatable<TriState>
{
    public TriState(bool isTrue)
    {
        IsTrue = isTrue;
        IsFalse = !isTrue;
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
        if (IsNone) return other.IsNone;
        return IsTrue && other.IsTrue || IsFalse && other.IsFalse;
    }

    public override int GetHashCode() => IsNone ? 0 : IsFalse ? 1 : 2;

    public bool IsFalse { get; }

    public bool IsNone => !IsFalse && !IsTrue;

    public bool IsTrue { get; }

    public override string ToString() => IsNone ? $"{nameof(IsNone)}" : IsTrue ? $"{nameof(IsTrue)}" : $"{nameof(IsFalse)}";
}

public struct TriState<TState1, TState2> : IEquatable<TriState<TState1, TState2>>
{

    public TriState([DisallowNull] TState1 state1)
    {
        State1 = Opt.Some(state1);
        State2 = Opt.None<TState2>();
    }

    public TriState([DisallowNull] TState2 state2)
    {
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
        if (IsNone) return other.IsNone;
        return State1.IsSome && other.State1.IsSome || State2.IsSome && other.State2.IsSome;
    }

    public override int GetHashCode() => IsNone ? 0 : State1.IsSome ? State1.GetHashCode() : State2.GetHashCode();

    public bool IsNone => State1.IsNone && State2.IsNone;

    public Opt<TState1> State1 { get; }
    public Opt<TState2> State2 { get; }

    public override string ToString() => IsNone ? $"{nameof(IsNone)}" : State1.IsSome ? $"{State1}" : $"{State2}";
}