namespace Foundation.DesignPatterns.ChainOfResponsibility;

public static class To
{
    public static Func<T, Option<T>> Delegate<T>(this Func<T, Option<T>> func) => func;
    public static Func<TIn, Option<TOut>> Delegate<TIn, TOut>(this Func<TIn, Option<TOut>> func) => func;
}
