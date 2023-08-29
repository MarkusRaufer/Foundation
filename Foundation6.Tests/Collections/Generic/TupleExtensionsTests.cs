using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class TupleExtensionsTests
{
    [Test]
    public void ToArrays()
    {
        var numberOfElements = 5;
        var (l, r) = (Enumerable.Range(1, numberOfElements), Enumerable.Range(6, numberOfElements));

        var (x, y) = (l, r).ToArrays();

        Assert.AreEqual(numberOfElements, x.Length);
        Assert.AreEqual(numberOfElements, y.Length);

        Assert.IsTrue(l.SequenceEqual(x));
        Assert.IsTrue(r.SequenceEqual(y));
    }

    [Test]
    public void ToLists()
    {
        var numberOfElements = 5;
        var (l, r) = (Enumerable.Range(1, numberOfElements), Enumerable.Range(6, numberOfElements));

        var (x, y) = (l, r).ToLists();

        Assert.AreEqual(numberOfElements, x.Count);
        Assert.AreEqual(numberOfElements, y.Count);

        Assert.IsTrue(l.SequenceEqual(x));
        Assert.IsTrue(r.SequenceEqual(y));
    }
}
