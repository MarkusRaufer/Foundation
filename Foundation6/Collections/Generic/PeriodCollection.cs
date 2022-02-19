using System.Collections;
using System.Collections.Generic;

namespace Foundation.Collections.Generic
{
    public class PeriodCollection 
        : ICollection<Period>
        , IReadOnlyPeriodCollection
    {
        private readonly SortedSet<Period> _periods;

        public PeriodCollection()
        {
            _periods = new SortedSet<Period>();
        }

        public PeriodCollection(IEnumerable<Period> periods)
        {
            _periods = new SortedSet<Period>(periods);
        }

        public int Count => _periods.Count;

        public bool IsReadOnly => false;

        public void Add(Period period)
        {
            _periods.Add(period);
        }

        public void Clear() => _periods.Clear();

        public bool Contains(Period period) => _periods.Contains(period);

        public void CopyTo(Period[] array, int arrayIndex)
        {
            _periods.CopyTo(array, arrayIndex);
        }

        public bool Remove(Period period) => _periods.Remove(period);

        IEnumerator IEnumerable.GetEnumerator() => _periods.GetEnumerator();
        public IEnumerator<Period> GetEnumerator() => _periods.GetEnumerator();

        public Period Max => _periods.Max;

        public Period Min => _periods.Min;

    }
}
