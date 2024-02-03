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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic
{
    public static  class OrdinalEnumerableExtensions
    {
        /// <summary>
        /// Returns a list of elements at their dedicated ordinal position. Positions which do not exist in the ordinals are replaced with None.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ordinals"></param>
        /// <returns></returns>
        public static IEnumerable<Option<T>> OrdinalFill<T>(this IEnumerable<Ordinal<T>> ordinals)
        {
            var i = 0;
            foreach (var ordinal in ordinals.OrderBy(o => o.Position))
            {
                if (i < ordinal.Position)
                {
                    foreach (var pos in Generator.Range(i, ordinal.Position - 1))
                    {
                        yield return Option.None<T>();
                        ++i;
                    }
                }

                if (i == ordinal.Position) yield return Option.Some(ordinal.Value);

                ++i;
            }
        }
    }
}
