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
    public static bool TryParse(string str, out TimeSpan? timeSpan)
    {
        if (string.IsNullOrWhiteSpace(str) || str.Length < 4)
        {
            timeSpan = null;
            return false;
        }

        var isNegative = str[0] is '-';

        if (isNegative ? str[1] is not 'P' : str[0] is not 'P')
        {
            timeSpan = null;
            return false;
        }

        timeSpan = ToTimeSpanParts(str.AsSpan()).Aggregate(TimeSpan.Zero, (acc, span) 
            => isNegative 
            ? acc.Subtract(span)
            : acc.Add(span));

        return timeSpan.HasValue;
    }
    
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
