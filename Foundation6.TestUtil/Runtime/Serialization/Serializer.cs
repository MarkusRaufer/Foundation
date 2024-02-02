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
﻿namespace Foundation.TestUtil.Runtime.Serialization;

using System.Runtime.Serialization.Formatters.Binary;

public static class Serializer
{
#pragma warning disable SYSLIB0011 // Type or member is obsolete
    public static void Serialize(this Stream stream, object graph)
    {
        var formatter = new BinaryFormatter();

        formatter.Serialize(stream, graph);
    }

    public static object Deserialize(this Stream stream)
    {
        if(0 != stream.Position) stream.Position = 0;

        var formatter = new BinaryFormatter();

        return formatter.Deserialize(stream);
    }
#pragma warning restore SYSLIB0011 // Type or member is obsolete
}
