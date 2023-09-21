namespace Foundation.Collections.Generic;

public static class DualOrdinalStreamsFactory
{
    /// <summary>
    /// Splits a stream into two streams and returns it as a new DualOrdinalStreams. 
    /// Matching items are added to the right stream.
    /// If isExhaustive is true, matching items are not added to the left stream.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate">This is the split criteria.</param>
    /// <param name="project">A projection from TLeft to TRight.</param>
    /// <param name="isExhaustive">If true then matching items are not added to the left stream.</param>
    /// <returns></returns>
    public static DualOrdinalStreams<TLeft, TRight> ToDualOrdinalStreams<TLeft, TRight>(
        this IEnumerable<TLeft> items,
        Func<TLeft, bool> predicate,
        Func<TLeft, TRight> project,
        bool isExhaustive)
    {
        items.ThrowIfEnumerableIsNull();
        predicate.ThrowIfNull();
        project.ThrowIfNull();

        var streams = new DualOrdinalStreams<TLeft, TRight>();

        foreach (var (counter, item) in items.Enumerate())
        {
            if (predicate(item))
            {
                var ordinalRight = new Ordinal<TRight> { Position = counter, Value = project(item) };

                streams.Right = streams.Right.Append(ordinalRight);

                if (isExhaustive) continue;
            }

            var ordinalLeft = new Ordinal<TLeft> { Position = counter, Value = item };
            streams.Left = streams.Left.Append(ordinalLeft);
        }

        return streams;
    }
}
