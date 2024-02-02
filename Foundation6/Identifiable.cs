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

public static class Identifiable
{
    /// <summary>
    /// Factory method of Identifiable<typeparamref name="TId"/>, <typeparamref name="TSubject"/>.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TSubject"></typeparam>
    /// <param name="id"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    public static Identifiable<TId, TSubject> New<TId, TSubject>(TId id, TSubject subject) => new(id, subject);
}

/// <summary>
/// Decorator to make an object identifiable.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <typeparam name="TSubject">Type of the object.</typeparam>
/// <param name="Id">The identifier of the object.</param>
/// <param name="Subject">The object to be identified.</param>
public record Identifiable<TId, TSubject>(TId Id, TSubject Subject) : IIdentifiable<TId>;
