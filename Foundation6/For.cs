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

namespace Foundation
{
    public interface IForCollect<T>
    {
        IEnumerable<T> Collect(Func<T, T> generator);
    }

    /// <summary>
    /// For is a generator that returns an endless list of items. Be aware to limit the number of generated items.
    /// You can use Take or TakeUntil.
    /// </summary>
    public static class For
    {
        public static IEnumerable<T> Collect<T>(Func<T> generator)
        {
            return new ForEnumerable<T>(generator);
        }

        public static IForCollect<T> StartAt<T>(Func<T> seed) where T : notnull
        {
            return new ForCollect<T>(seed);
        }
    }

    class ForCollect<T> : IForCollect<T>
        where T : notnull
    {
        private readonly Func<T> _seed;

        public ForCollect(Func<T> seed)
        {
            _seed = seed.ThrowIfNull();
        }

        public IEnumerable<T> Collect(Func<T, T> generator)
        {
            return new ForEnumerableWithSeed<T>(_seed, generator);
        }
    }

    class ForEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<T> _generator;

        public ForEnumerable(Func<T> generator)
        {
            _generator = generator.ThrowIfNull();
        }

        public IEnumerator<T> GetEnumerator() => new ForEnumerator<T>(_generator);

        IEnumerator IEnumerable.GetEnumerator() => new ForEnumerator<T>(_generator);
    }

    class ForEnumerableWithSeed<T> : IEnumerable<T>
        where T : notnull
    {
        private readonly Func<T, T> _generator;
        private readonly Func<T> _seed;

        public ForEnumerableWithSeed(Func<T> seed, Func<T, T> generator)
        {
            _seed = seed.ThrowIfNull();
            _generator = generator.ThrowIfNull();
        }

        public IEnumerator<T> GetEnumerator() => new ForEnumeratorWithSeed<T>(_seed, _generator);

        IEnumerator IEnumerable.GetEnumerator() => new ForEnumeratorWithSeed<T>(_seed, _generator);
    }

    class ForEnumerator<T> : IEnumerator<T>
    {
        private T? _current;
        private readonly Func<T> _generator;

        public ForEnumerator(Func<T> generator) => _generator = generator.ThrowIfNull();

        public T Current => _current ?? throw new InvalidOperationException("Current should not be used in the current state");

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            _current = _generator();
            return true;
        }

        public void Reset()
        {
        }
    }

    class ForEnumeratorWithSeed<T> : IEnumerator<T>
        where T : notnull
    {
        private T? _current;
        private readonly Func<T, T> _generator;
        private bool _passed;
        private readonly Func<T> _seed;

        public ForEnumeratorWithSeed(Func<T> seed, Func<T, T> generator)
        {
            _seed = seed.ThrowIfNull();
            _generator = generator.ThrowIfNull();
        }

        public T Current => _current ?? throw new InvalidOperationException("Current should not be used in the current state");

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            _current = !_passed ? _seed() : _generator(Current);

            _passed = true;

            return true;
        }

        public void Reset()
        {
            _current = default;
            _passed = false;
        }
    }
}
