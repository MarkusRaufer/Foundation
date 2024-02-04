using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class BinaryExpressionExtensions
{
    [Test]
    public void GetBinaryExpressions_Should_ReturnBinaryExpressions_When_LambdaHasHierarchicalBinaryExpressions()
    {
        Expression<Func<int, int, string, bool>> lambda = (x, y, name) => x == 2 && name == "two" || x == 3 && name == "three";

        var be = lambda.Body as BinaryExpression;
        be.Should().NotBeNull();

        var expressions = be!.GetBinaryExpressions().ToArray();

        expressions.Length.Should().Be(6);
    }
}
