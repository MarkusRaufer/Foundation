using System.Collections.Generic;

namespace Foundation.Collections.Generic
{
    public interface IReadOnlyPeriodCollection : IReadOnlyCollection<Period>
    {
        Period Max { get; }
        Period Min { get; }
    }
}
