using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class ExpressionHelperTests
{
    [Test]
    public void Concat_Should_CreateBinaryExpression_When_ExpressionListIsNotEmpty()
    {
        var b1 = makeBinary(5);
        var b2 = makeBinary(7);
        var b3 = makeBinary(7);
        var expressions = new List<BinaryExpression> { b1, b2, b3 };

        var binary = ExpressionHelper.Concat(expressions, ExpressionType.OrElse);

        binary.Should().NotBeNull();

        {
            var expected = Expression.MakeBinary(ExpressionType.OrElse, b1, b2);
            binary!.Left.EqualsToExpression(expected).Should().BeTrue();
        }
        {
            binary!.Right.EqualsToExpression(b3).Should().BeTrue();
        }

        BinaryExpression makeBinary(int n)
        {
            return Expression.MakeBinary(
                ExpressionType.Equal,
                Expression.Parameter(typeof(int), "x"),
                Expression.Constant(n));
        }
    }
}
