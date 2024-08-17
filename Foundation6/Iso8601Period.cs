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
namespace Foundation;

public static class Iso8601Period
{
    /// <summary>
    /// Creates a <see cref="TimeSpan"/> from a ISO 8601 period string.
    /// It does not support years and months. They must be transformed to days.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static bool TryParse(ReadOnlySpan<char> span, out TimeSpan? timeSpan)
    {
        if (span.IsEmpty || span.Length < 4)
        {
            timeSpan = null;
            return false;
        }

        var isNegative = span[0] is '-';

        if (isNegative ? span[1] is not 'P' : span[0] is not 'P')
        {
            timeSpan = null;
            return false;
        }

        timeSpan = ToTimeSpanParts(span).Aggregate(TimeSpan.Zero, (acc, span)
            => isNegative
            ? acc.Subtract(span)
            : acc.Add(span));

        return timeSpan.HasValue;
    }

    /// <summary>
    /// Creates <see cref="TimeSpan"/> parts of a ISO 8601 period. Does not support years and months.
    /// They must be transformed to days.
    /// </summary>
    /// <param name="span">The span containing the ISO 8106 period.</param>
    /// <returns></returns>
    public static IReadOnlyCollection<TimeSpan> ToTimeSpanParts(ReadOnlySpan<char> span)
    {
        var start = span[0] == '-' ? 2 : 1;
        char? unit;
        int? leftIndex = null;
        int? rightIndex = null;

        var timeSpans = new List<TimeSpan>();

        for (var i = start; i < span.Length; i++)
        {
            var ch = span[i];
            if (char.IsDigit(ch))
            {
                if (!leftIndex.HasValue)
                {
                    leftIndex = i;
                    continue;
                }
                
                rightIndex = i;
                continue;
            }
            else
            {
                if (ch == 'T') continue;
                unit = ch;
            }

            if (!leftIndex.HasValue) return [];

            if (!rightIndex.HasValue) rightIndex = leftIndex;

            var digitSpan = span[leftIndex.Value..(rightIndex.Value + 1)];
            
            leftIndex = null;
            rightIndex = null;

#if NET6_0_OR_GREATER
            if (!int.TryParse(digitSpan, out var value)) return [];
#else
            var str = digitSpan.ToString();
            if (!int.TryParse(str, out var value)) return [];
#endif

            TimeSpan? timeSpan = unit switch
            {
                'D' => TimeSpan.FromDays(value),
                'H' => TimeSpan.FromHours(value),
                'M' => TimeSpan.FromMinutes(value),
                'S' => TimeSpan.FromSeconds(value),
                'F' => TimeSpan.FromMilliseconds(value),
                _ => null
            };

            if (!timeSpan.HasValue) return [];

            timeSpans.Add(timeSpan.Value);
        }

        return timeSpans;
    }

    public static string Zero { get; } = "PT0S";
}
