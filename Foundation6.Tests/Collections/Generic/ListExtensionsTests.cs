using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class ListExtensionsTests
{
    [Test]
    public void TryGet_Should_Return5_When_PredicateReturnsTrueOn5()
    {
        var items1 = Enumerable.Range(1, 10).ToList();

        items1.TryGet(x => x == 5, out var value);
        value.ShouldBe(5);
    }
}
