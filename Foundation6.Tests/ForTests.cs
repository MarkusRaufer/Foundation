using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class ForTests
{
    [Test]
    public void Returns_Should_Return5Elements_When_Take5()
    {
        var value = 1;

        var values = For.Collect(() => value++)
                        .Take(5)
                        .ToArray();

        var expected = Enumerable.Range(1, 5);
        expected.SequenceEqual(values).ShouldBeTrue();
    }

    [Test]
    public void StartAt_Should_Return5Elements_When_TakeUntilEqualsInclusive5()
    {
        var values = For.StartAt(() => 3).Collect(value => ++value) 
                                         .TakeUntil(x => x == 5, inclusive: true)
                                         .ToArray();

        var expected = Enumerable.Range(3, 3);
        expected.SequenceEqual(values).ShouldBeTrue();
    }
}
