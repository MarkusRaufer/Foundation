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
using System.Diagnostics.CodeAnalysis;
using Foundation.ComponentModel;

namespace Foundation.Collections.ObjectModel
{
    public class ReadOnlyPropertyCollection 
        : IReadOnlyPropertyCollection
        , IEquatable<ReadOnlyPropertyCollection>
    {
        private readonly PropertyCollection _properties;

        public ReadOnlyPropertyCollection(IEnumerable<Property> properties)
        {
            _properties = new PropertyCollection(properties);
        }

        public ReadOnlyPropertyCollection(PropertyCollection properties)
        {
            _properties = properties.ThrowIfNull();
        }
        
        public bool ContainsProperty(string name) => _properties.ContainsProperty(name);

        public int Count => _properties.Count;

        public override bool Equals(object? obj) => obj is PropertyCollection other && Equals(other);

        public bool Equals(PropertyCollection? other)
        {
            return null != other && _properties.Equals(other);
        }

        public bool Equals(ReadOnlyPropertyCollection? other)
        {
            return null != other && Equals(other._properties);
        }

        public IEnumerator<Property> GetEnumerator() => _properties.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();

        public override int GetHashCode() => _properties.GetHashCode();

        public bool TryGetProperty(string name, [MaybeNullWhen(false)] out Property property)
        {
            return _properties.TryGetProperty(name, out property);
        }
    }
}
