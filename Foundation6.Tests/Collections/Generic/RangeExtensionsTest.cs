using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic;

[TestFixture]
public class RangeExtensionsTest
{
    [Test]
    public void Enumerator_Should_ThrowException_When_RangeEndIsUndefined()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var i = 0;
            foreach (var number in 0..)
            {
                Assert.AreEqual(i, number);
                i++;
            }

        });
    }

    [Test]
    public void Enumerator_Should_Return_5Integers_When_RangeStart0AndRangeEnd5()
    {
        var i = 0;
        foreach(var number in 0..5)
        {
            i.Should().Be(number);
            i++;
        }
    }

    [Test]
    public void Enumerator_Should_Return_5Integers_When_RangeStart5AndRangeEnd10()
    {
        var i = 5;
        foreach (var number in 5..10)
        {
            i.Should().Be(number);
            i++;
        }
    }
}
