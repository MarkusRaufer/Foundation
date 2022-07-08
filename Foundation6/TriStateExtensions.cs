using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    public static class TriStateExtensions
    {
        public static Opt<TResult> OnState1<TState1, TState2, TResult>(
            this TriState<TState1, TState2> triState,
            Func<TState1, TResult> onState1)
        {
            if (triState.State.IsNone) return Opt.None<TResult>();

            var state = triState.State.OrThrow();

            if(state.Item2.IsSome) return Opt.None<TResult>();

            return onState1(state.Item1.OrThrow());
        }

        public static Opt<TResult> OnState2<TState1, TState2, TResult>(
            this TriState<TState1, TState2> triState,
            Func<TState2, TResult> onState2)
        {
            if (triState.State.IsNone) return Opt.None<TResult>();

            var state = triState.State.OrThrow();

            if (state.Item1.IsSome) return Opt.None<TResult>();

            return onState2(state.Item2.OrThrow());
        }
    }
}
