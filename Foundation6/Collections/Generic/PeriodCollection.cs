// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿using System.Collections;
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
