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

        var eq = lambda1.EqualsToExpression(lambda2, ignoreNames: true);

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

        var eq = lambda1.EqualsToExpression(lambda2, ignoreNames: true);

        eq.Should().BeTrue();
    }

    [Test]
    public void GetBinaryExpressions_Should_ReturnBinaryExpressions_When_LambdaHasHierarchicalBinaryExpressions()
    {
        Expression<Func<int, int, string, bool>> lambda = (x, y, name) => x == 2 && y == 5 && name == "two" || x == 3 && name == "three";

        var be = lambda.Body as BinaryExpression;
        be.Should().NotBeNull();

        var expressions = be!.GetBinaryExpressions().ToArray();

        expressions.Length.Should().Be(9);
    }

    [Test]
    public void GetPredicates_Should_Return2BinaryExpressions_When_LambdaHas3ParametersIncludingSubtractExpression()
    {
        Expression<Func<IDateTimeProvider, int, DateTime, bool>> lambda = (IDateTimeProvider dtp, int x, DateTime dt) => (dtp.Now.Year - dt.Year) >= 18 && x == 2;

        var be = lambda.Body as BinaryExpression;
        be.Should().NotBeNull();

        var predicates = be!.GetPredicates().ToArray();

        predicates.Length.Should().Be(3);
        predicates.All(x => x.NodeType.IsBinary());
    }

    [Test]
    public void IsTerminalPredicate_Should_Return2BinaryExpressions_When_LambdaHas3ParametersIncludingSubtractExpression()
    {
        Expression<Func<IDateTimeProvider, int, DateTime, bool>> lambda = (IDateTimeProvider dtp, int x, DateTime dt) => (dtp.Now.Year - dt.Year) >= 18 && x == 2;

        var be = lambda.Body as BinaryExpression;
        be.Should().NotBeNull();

        var expressions = be.GetBinaryExpressions().ToArray();
        var terminals = expressions.Where(x => x.IsTerminalPredicate()).ToArray();

        terminals.Length.Should().Be(2);
        terminals.All(x => x.IsPredicate());
    }
}
