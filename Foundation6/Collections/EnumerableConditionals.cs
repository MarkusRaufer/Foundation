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
﻿using Foundation.Collections.Generic;
using System.Collections;

namespace Foundation.Collections;

public static class EnumerableConditionals
{
    public static bool ExistsType(this IEnumerable items, bool includeAssignableTypes, params Type[] types)
    {
        items = items.ThrowIfNull();
        types.ThrowIfOutOfRange(() => types.Length == 0);

        var search = types.ToList();

        Func<Type, Type, Type?> checkType = includeAssignableTypes ? ofType : ofExactType;

        foreach (var item in items)
        {
            if (null == item) continue;

            var type = item.GetType();
            var checkedtype = search.Select(x => checkType(x, type)).FirstOrDefault(x => null != x);

            if (null != checkedtype)
            {
                search.Remove(checkedtype);

                if (0 == search.Count) return true;
            }
        }

        static Type? ofExactType(Type lhs, Type rhs)
        {
            return lhs.Equals(rhs) ? lhs : null;
        }

        static Type? ofType(Type lhs, Type rhs)
        {
            var exactType = ofExactType(lhs, rhs);
            if(null != exactType) return exactType;

            return lhs.IsAssignableFrom(rhs) ? lhs : null;
        }

        return false;
    }
}
