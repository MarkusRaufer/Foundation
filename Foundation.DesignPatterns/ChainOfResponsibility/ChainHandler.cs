namespace Foundation.DesignPatterns.ChainOfResponsibility;

public static class ChainHandler
{
    public static ChainHandler<TIn, TOut> New<TIn, TOut>(Func<TIn, Option<TOut>> TryHandle)
        => new(TryHandle, null);

    public static ChainHandler<TIn, TOut> New<TIn, TOut>(Func<TIn, Option<TOut>> TryHandle, ChainHandler<TIn, TOut> Successor)
        => new(TryHandle, Successor);
}

public record ChainHandler<TIn, TOut>(Func<TIn, Option<TOut>> TryHandle, ChainHandler<TIn, TOut>? Successor)
{
    public Option<TOut> Handle(TIn @in)
    {
        var result = TryHandle(@in);
        if(result.IsSome) return result;
        
        return Successor is null ? Option.None<TOut>() : Successor.Handle(@in);
    }
}
