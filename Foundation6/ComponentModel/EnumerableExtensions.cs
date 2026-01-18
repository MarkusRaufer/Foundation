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
﻿// The MIT License (MIT)
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
using System.Collections.ObjectModel;

namespace Foundation.ComponentModel;

public static class EnumerableExtensions
{
    public static IEnumerable<KeyValuePair<string, EventActionValue<object?>>> ToUpdateEvents(
        this IEnumerable<KeyValuePair<string, object?>>? source,
        IEnumerable<KeyValuePair<string, object?>> keyValues)
    {
        if (source is null) yield break;

        foreach (var kv in keyValues)
        {
            if (string.IsNullOrWhiteSpace(kv.Key)) continue;

            var sourceDictionary = source switch
            {
                IReadOnlyDictionary<string, object?> rod => rod,
                IDictionary<string, object?> d => new ReadOnlyDictionary<string, object?>(d),
                _ => source.ToDictionary(x => x.Key, x => x.Value),
            };

            if (!sourceDictionary.TryGetValue(kv.Key, out var value))
            {
                yield return new KeyValuePair<string, EventActionValue<object?>>(kv.Key, new EventActionValue<object?>(EventAction.Add, kv.Value));
                continue;
            }

            if (kv.Value.EqualsNullable(value)) continue;

            yield return new KeyValuePair<string, EventActionValue<object?>>(kv.Key, new EventActionValue<object?>(EventAction.Update, kv.Value));
        }

        var newKeyValues = keyValues.ToDictionary(x => x.Key, x => x.Value);

        foreach (var sourceKeyValue in source)
        {
            if (newKeyValues.ContainsKey(sourceKeyValue.Key)) continue;

            yield return new KeyValuePair<string, EventActionValue<object?>>(sourceKeyValue.Key, new EventActionValue<object?>(EventAction.Remove, sourceKeyValue.Value));
        }
    }
}
