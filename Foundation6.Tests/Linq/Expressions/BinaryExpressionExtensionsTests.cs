using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class BinaryExpressionExtensionsTests
{
    [Test]
    public void EqualsToExpression_Should_ReturnFalse_When_LambdasAreDifferent()
    {
        Expression<Func<int, string, bool>> lambda1 = (x, name) => x == 2 && name == "two" || x == 3 && name == "three";
        Expression<Func<int, string, bool>> lambda2 = (x, name) => x == 5 && name == "two" || x == 3 && name == "three";

        var eq = lambda1.EqualsToExpression(lambda2);

        eq.Should().BeFalse();
    }


    [Test]
    public void EqualsToExpression_Should_ReturnTrue_When_LambdaHasEqualBinaryExpressionsButWithReversedSides()
    {
        Expression<Func<int, string, bool>> lambda1 = (x, name) => x == 2 && name == "two" || x == 3 && name == "three";
        Expression<Func<int, string, bool>> lambda2 = (x, name) => 2 == x && name == "two" || x == 3 && "three" == name;

        var eq = lambda1.EqualsToExpression(lambda2);

        eq.Should().BeTrue();
    }

    [Test]
    public void EqualsToExpression_Should_ReturnTrue_When_LambdaHasEqualBinaryExpressionsButWithReversedSidesAndDifferentParameterNames()
    {
        Expression<Func<int, int, string, bool>> lambda1 = (x, y, name) => (x == 2 || y == 5) && name == "two" || x == 3 && name == "three";
        Expression<Func<int, int, string, bool>> lambda2 = (a, b, name) => (b == 5 || 2 == a) && name == "two" || a == 3 && "three" == name;

        var eq = lambda1.EqualsToExpression(lambda2);

        eq.Should().BeTrue();
    }

    [Test]
    public void EqualsToExpression_Should_ReturnTrue_When_LambdaHasSameBinaryExpressions()
    {
        Expression<Func<int, int, string, bool>> lambda1 = (x, y, name) => x == 2 && y == 5 && name == "two" || x == 3 && name == "three";
        Expression<Func<int, int, string, bool>> lambda2 = (x, y, name) => x == 2 && y == 5 && name == "two" || x == 3 && name == "three";

        var eq = lambda1.EqualsToExpression(lambda2);

        eq.Should().BeTrue();
    }

    [Test]
    public void EqualsToExpression_Should_ReturnTrue_When_LambdaHasSameBinaryExpressionsButDifferentParameterNames()
    {
        Expression<Func<int, int, string, bool>> lambda1 = (x, y, name) => x == 2 && y == 5 && name == "two" || x == 3 && name == "three";
        Expression<Func<int, int, string, bool>> lambda2 = (a, b, name) => a == 2 && b == 5 && name == "two" || a == 3 && name == "three";

        var eq = lambda1.EqualsToExpression(lambda2);

        eq.Should().BeTrue();
    }

    [Test]
    public void GetBinaryExpressions_Should_ReturnBinaryExpressions_When_LambdaHasHierarchicalBinaryExpressions()
    {
        Expression<Func<int, int, string, bool>> lambda = (x, y, name) => x == 2 && y == 5 && name == "two" || x == 3 && name == "three";

        var be = lambda.Body as BinaryExpression;
        be.Should().NotBeNull();

        var expressions = be!.GetBinaryExpressions().ToArray();

        expressions.Length.Should().Be(8);
    }
}
