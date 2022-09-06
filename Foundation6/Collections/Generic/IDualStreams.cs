namespace Foundation.Collections.Generic
{
    public interface IDualStreams<TLeft, TRight>
    {
        IEnumerable<TLeft> Left { get; set; }
        IEnumerable<TRight> Right { get; set; }
    }
}