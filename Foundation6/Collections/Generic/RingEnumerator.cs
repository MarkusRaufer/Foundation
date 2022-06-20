namespace Foundation.Collections.Generic;

public class RingEnumerator
{
    public static RingEnumerator<T> Create<T>(IEnumerable<T> enumerable, bool infinite = false, int index = 0)
    {
        return new RingEnumerator<T>(enumerable, infinite, index);
    }
}

public class RingEnumerator<T> : IEnumerator<T?>
{
    private readonly IEnumerator<T> _enumerator;
    private bool _passed;
    private readonly bool _infinite;
    private int _pos = 0;
    private readonly int _startIndex;

    public RingEnumerator(IEnumerable<T> enumerable, bool infinite = false, int index = 0)
    {
        if (null == enumerable) throw new ArgumentNullException(nameof(enumerable));
        _enumerator = enumerable.GetEnumerator();
        _infinite = infinite;
        _startIndex = index;
        _pos = index;
    }

    public T? Current => _enumerator.Current;

    public void Dispose()
    {
        _enumerator.Dispose();
    }

    object? System.Collections.IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (!_passed)
        {
            _passed = true;
            for (var i = 0; i <= _startIndex; i++)
            {
                if (!_enumerator.MoveNext())
                {
                    if (0 == i) return false;

                    _enumerator.Reset();
                    _pos = 0;
                }
                else
                    _pos = i;
            }
            return true;
        }

        if (!_enumerator.MoveNext())
        {
            _enumerator.Reset();
            _enumerator.MoveNext();
            _pos = 0;
        }
        else
            _pos++;

        if (!_infinite && _pos == _startIndex) return false;

        return true;
    }

    public void Reset()
    {
        _enumerator.Reset();
        _passed = false;
        _pos = _startIndex;
    }
}

