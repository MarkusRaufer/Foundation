namespace Foundation.Collections.Generic;

public interface IHashChain<T> : IReadOnlyCollection<T>
{
    bool IsConsistent { get; }
}
