namespace Foundation.Collections.Generic
{
    /// <summary>
    /// Is a tuple of two streams: Left and Right.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    public class DualStreams<TLeft, TRight>
    {
        public IEnumerable<TLeft> Left { get; set; } = Enumerable.Empty<TLeft>();
        public IEnumerable<TRight> Right { get; set; } = Enumerable.Empty<TRight>();
    }
}
