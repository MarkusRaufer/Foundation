using Foundation.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.DesignPatterns.ChainOfResponsibility;

public class Chain
{
    public static IEnumerable<ChainHandler<TIn, TOut>> Handlers<TIn, TOut>(IEnumerable<Func<TIn, Option<TOut>>> handlers)
    {
        var prevHandler = handlers.FirstOrDefault();
        if (prevHandler is null) yield break;

        var prevChainHandler = ChainHandler.New(prevHandler);
        yield return prevChainHandler;

        foreach (var handler in handlers.Skip(1))
        {
            prevChainHandler = ChainHandler.New(handler, prevChainHandler);
            yield return prevChainHandler;
        }
    }

    public static IEnumerable<ChainHandler<TIn, TOut>> Handlers<TIn, TOut>(IEnumerable<ChainHandler<TIn, TOut>> handlers)
    {
        return handlers.Pairs((lhs, rhs) =>
        {
            return (lhs, new ChainHandler<TIn, TOut>(rhs.TryHandle, lhs));
        });
    }
}
