using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class ParameterHelperTests
{
    [Test]
    public void AllEqual_Should_ReturnFalse_When_NotAllParametersAreSame()
    {
        Expression<Func<int, bool>> lambda = x => x == 2 || x == 3 || x == 5;

        var binaryExpression = lambda.Body as BinaryExpression;
        binaryExpression.Should().NotBeNull();

        var expressions = binaryExpression!.GetBinaryExpressions();
        var parameters = expressions.SelectMany(x => x.GetParameters()).Append(Expression.Parameter(typeof(int), "y"));
        var areSame = ParameterHelper.AllEqual(parameters);

        areSame.Should().BeFalse();
    }

    [Test]
    public void AllEqual_Should_ReturnTrue_When_AllParametersAreSame()
    {
        Expression<Func<int, bool>> lambda = x => x == 2 || x == 3 || x == 5;

        var binaryExpression = lambda.Body as BinaryExpression;
        binaryExpression.Should().NotBeNull();

        var expressions = binaryExpression!.GetBinaryExpressions();
        var parameters = expressions.SelectMany(x => x.GetParameters());
        var areSame = ParameterHelper.AllEqual(parameters);

        areSame.Should().BeTrue();
    }
}
