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
﻿namespace Foundation;

using System.Text;

public static class TimeSpanExtensions
{
    public static int GetDays(this TimeSpan duration)
    {
        return (int)Math.Ceiling(duration.TotalDays);
    }

    public static string ToIso8601Period(this TimeSpan duration)
    {
        var sb = new StringBuilder();
        if (TimeSpan.Zero > duration)
            sb.Append('-');

        sb.Append('P');
        if (0 != duration.Days)
        {
            var days = Math.Abs(duration.Days);
            sb.Append(days);
            sb.Append('D');
        }

        var diff = duration - TimeSpan.FromDays(duration.Days);
        if (TimeSpan.Zero != diff)
        {
            sb.Append('T');
        }
        else
        {
            return sb.ToString();
        }

        if (0 != duration.Hours)
        {
            var hours = Math.Abs(duration.Hours);
            sb.Append(hours);
            sb.Append('H');
        }
        if (0 != duration.Minutes)
        {
            var minutes = Math.Abs(duration.Minutes);
            sb.Append(minutes);
            sb.Append('M');
        }
        if (0 != duration.Seconds)
        {
            var seconds = Math.Abs(duration.Seconds);
            sb.Append(seconds);
            sb.Append('S');
        }
        if (0 != duration.Milliseconds)
        {
            var milliseconds = Math.Abs(duration.Milliseconds);
            sb.Append(milliseconds);
            sb.Append('F');
        }

        return sb.Length > 2 ? sb.ToString() : Iso8601Period.Zero;
    }

    public static IEnumerable<string> ToIso8601PeriodParts(this TimeSpan duration)
    {
        var sb = new StringBuilder();
        if (TimeSpan.Zero > duration)
            yield return "-";

        yield return "P";
        if (0 != duration.Days)
            yield return $"{Math.Abs(duration.Days)}D";

        var diff = duration - TimeSpan.FromDays(duration.Days);
        if (TimeSpan.Zero != diff)
            yield return "T";
        else
            yield break;

        if (0 != duration.Hours)
            yield return $"{Math.Abs(duration.Hours)}H";

        if (0 != duration.Minutes)
            yield return $"{Math.Abs(duration.Minutes)}M";

        if (0 != duration.Seconds)
            yield return $"{Math.Abs(duration.Seconds)}S";

        if (0 != duration.Milliseconds)
            yield return $"{Math.Abs(duration.Milliseconds)}F";
    }
}
