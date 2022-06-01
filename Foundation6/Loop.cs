using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class Loop
{
    /// <summary>
    /// for loop that returns a list of values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="generator"></param>
    /// <returns></returns>
    public static IEnumerable<T> For<T>([DisallowNull] Func<T> generator)
    {
        if (generator is null) yield break;

        while (true)
        {
            yield return generator();
        }
    }

    /// <summary>
    /// for loop that returns a specific number of values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="loop">defines how often generator is called. Number of values returned.</param>
    /// <param name="generator">generator that creates each resulting value.</param>
    /// <returns></returns>
    public static IEnumerable<T> For<T>(int loop, [DisallowNull] Func<T> generator)
    {
        return For(generator).Take(loop);
    }
}
