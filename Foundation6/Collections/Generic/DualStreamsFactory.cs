namespace Foundation.Collections.Generic;

public static class DualStreamsFactory
{
    /// <summary>
    /// Splits a stream into two streams and returns it as DualStreams. Matching items are added to the right stream.
    /// If isExhaustive is false, all items are added to the left stream.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate">If predicate it true the item is added to the right stream.</param>
    /// <param name="project"></param>
    /// <param name="isExhaustive"></param>
    /// <returns></returns>
    public static DualStreams<TLeft, TRight> ToDualStreams<TLeft, TRight>(
        this IEnumerable<TLeft> items,
        Func<TLeft, bool> predicate,
        Func<TLeft, TRight> project,
        bool isExhaustive)
    {
        items.ThrowIfEnumerableIsNull();
        predicate.ThrowIfNull();
        project.ThrowIfNull();

        var streams = new DualStreams<TLeft, TRight>();

        foreach (var item in items)
        {
            if (predicate(item))
            {
                var right = project(item);

                streams.Right = streams.Right.Append(right);

                if (isExhaustive) continue;
            }

            streams.Left = streams.Left.Append(item);
        }

        return streams;
    }
}
