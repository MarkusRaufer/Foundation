using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation;

[TestFixture]
public class RangeExtensionsTests
{
    [Test]
    public void GetEnumerator_Should_ReturnEnumerator_When_UsedWithForeachLoop()
    {
        var numbers = new List<int>();

        foreach(var i in 0..10)
        {
            numbers.Add(i);
        }

        Assert.AreEqual(11, numbers.Count);
        foreach(var i in Enumerable.Range(0, 11))
        {
            Assert.AreEqual(i, numbers[i]);
        }
    }

    [Test]
    public void Includes_Should_ReturnFalse_When_ValueIsOutOfRange()
    {
        var range = 0..10;

        Assert.IsFalse(range.Includes(-1));
        Assert.IsFalse(range.Includes(11));
    }

    [Test]
    public void Includes_Should_ReturnTrue_When_ValueIsInRange()
    {
        var range = 0..10;
        foreach (var i in Enumerable.Range(0, 11))
        {
            Assert.IsTrue(range.Includes(i));
        }
    }

    [Test]
    public void Includes_Should_ReturnTrue_When_ValueIsInRangeAndUsesEndIsFromEnd()
    {
        {
            var range = 0..;
            foreach (var i in Enumerable.Range(0, 11))
            {
                Assert.IsTrue(range.Includes(i));
            }
        }
        {
            var range = 0..^3;
            foreach (var i in Enumerable.Range(0, 11))
            {
                Assert.IsTrue(range.Includes(i));
            }
        }
    }

    [Test]
    public void Includes_Should_ReturnTrue_When_ValueIsInRangeAndUsesStartIsFromEnd()
    {
        {
            var range = ..10;
            foreach (var i in Enumerable.Range(0, 11))
            {
                Assert.IsTrue(range.Includes(i));
            }
        }
        {
            var range = ^3..10;
            foreach (var i in Enumerable.Range(0, 11))
            {
                Assert.IsTrue(range.Includes(i));
            }
        }
    }
}
