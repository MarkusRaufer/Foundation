using System.Collections;

namespace Foundation.Collections.Generic;

public static class EnumerableDecorator
{
    public static IEnumerable<T> New<T>(Func<IEnumerator<T>> func) => new EnumerableDecorator<T>(func);
}

public class EnumerableDecorator<T>(Func<IEnumerator<T>> func) : IEnumerable<T>
{
    private readonly Func<IEnumerator<T>> _func = func.ThrowIfNull();

    public IEnumerator<T> GetEnumerator() => _func();

    IEnumerator IEnumerable.GetEnumerator() => _func();

}
