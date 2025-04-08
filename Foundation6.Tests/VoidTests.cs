using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace Foundation;

[TestFixture]
public class VoidTests
{
    [Test]
    public void Returns_Should_ReturnTrue_When_TrueIsSet()
    {
        List<int> items = [1, 2, 3];
        var index = 1;

        var result = items.Count > index && Void.Returns(true, () => items.RemoveAt(index));

        result.ShouldBeTrue();
    }
}
