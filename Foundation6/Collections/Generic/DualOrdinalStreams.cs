namespace Foundation.Collections.Generic;

/// <summary>
/// A tuple with two streams of "Ordinal" objects.
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public class DualOrdinalStreams<TLeft, TRight> : DualStreams<Ordinal<TLeft>, Ordinal<TRight>>
{
}
