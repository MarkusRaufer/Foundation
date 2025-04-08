using FluentAssertions;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class NewExpressionExtensionsTests
{
    private record Selected<T>(T Subject);

    [Test]
    public void Test()
    {
        var ctor = typeof(Selected<int>).GetConstructor([typeof(int)]);
        ctor.Should().NotBeNull();

        if (ctor is null)
        {
            Assert.Fail("no ctor found");
            return;
        }
        var expression = Expression.New(ctor, Expression.Constant(5));
        var newInstance = expression.CreateInstance();

        var expected = new Selected<int>(5);

        newInstance.Should().Be(expected);
    }
}
