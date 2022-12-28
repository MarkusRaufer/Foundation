namespace Foundation
{
    public static class TriStateExtensions
    {
        public static Option<TResult> OnState1<TState1, TState2, TResult>(
            this TriState<TState1, TState2> triState,
            Func<TState1, TResult> onState1)
        {
            if (triState.State.TryGet(out OneOf<TState1, TState2>? oneOf)
                && oneOf!.TryGet(out TState1? state1)) return onState1(state1!);

            return Option.None<TResult>();
        }

        public static Option<TResult> OnState2<TState1, TState2, TResult>(
            this TriState<TState1, TState2> triState,
            Func<TState2, TResult> onState2)
        {
            if (triState.State.TryGet(out OneOf<TState1, TState2>? oneOf)
                && oneOf!.TryGet(out TState2? state2)) return onState2(state2!);

            return Option.None<TResult>();
        }
    }
}
