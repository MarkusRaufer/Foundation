using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

[TestFixture]
public class VoidTests
{
    [Test]
    void Returns_Should_ReturnTrue_When_TrueIsSet()
    {
        List<int> items = [1, 2, 3];
        var index = 1;

        var result = items.Count < index && Void.Returns(true, () => items.RemoveAt(index));

        result.Should().BeTrue();
    }
}
