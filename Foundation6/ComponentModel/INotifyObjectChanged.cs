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
﻿namespace Foundation.ComponentModel;

using System.Collections.Specialized;
using System.ComponentModel;

public interface INotifyObjectChanged
{
    event ObjectChangedEventHandler ObjectChanged;
}

public enum ObjectChangedAction
{
    CollectionChanged,
    ObjectChanged,
    PropertyChanged
}

public delegate void ObjectChangedEventHandler(object? sender, ObjectChangedEventArgs e);

public class ObjectChangedEventArgs : EventArgs
{
    public ObjectChangedEventArgs(object? changedCollection, NotifyCollectionChangedEventArgs? args)
    {
        ChangedCollection = changedCollection.ThrowIfNull();
        NotifyCollectionChangedEventArgs = args.ThrowIfNull();

        Action = ObjectChangedAction.CollectionChanged;
    }

    public ObjectChangedEventArgs(object? changedObject, object? changedCollection, NotifyCollectionChangedEventArgs? args)
    {
        ChangedObject = changedObject.ThrowIfNull();
        ChangedCollection = changedCollection.ThrowIfNull();
        NotifyCollectionChangedEventArgs = args.ThrowIfNull();

        Action = ObjectChangedAction.CollectionChanged;
    }

    public ObjectChangedEventArgs(PropertyChangedEventArgs? args)
    {
        PropertyChangedEventArgs = args.ThrowIfNull();

        Action = ObjectChangedAction.PropertyChanged;
    }

    public ObjectChangedAction Action { get; set; }
    public object? ChangedCollection { get; set; }
    public object? ChangedObject { get; set; }
    public NotifyCollectionChangedEventArgs? NotifyCollectionChangedEventArgs { get; set; }
    public PropertyChangedEventArgs? PropertyChangedEventArgs { get; set; }
}

