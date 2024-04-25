using FluentAssertions;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class IdExpressionTests
{
    [Test]
    public void Cast_Should_ReturnParameterExpression_When_AssigningToParameterExpression()
    {
        var name = "x";
        var type = typeof(int);
        var parameter = Expression.Parameter(type, name);
        var sut = IdExpression.New(parameter);

        ParameterExpression targetParameter = sut;
        targetParameter.EqualsToExpression(parameter).Should().BeTrue();
    }

    [Test]
    public void Cast_Should_ReturnParameterExpression_When_UsingIsCastToParameterExpression()
    {
        var name = "x";
        var type = typeof(int);
        var parameter = Expression.Parameter(type, name);
        var sut = IdExpression.New(parameter);

        var idExpression = sut as IdExpression<ParameterExpression>;

        idExpression.Should().NotBeNull();
        idExpression.Expression.EqualsToExpression(parameter).Should().BeTrue();
    }

    [Test]
    public void New_Should_CreateDifferentIdExpressions_When_Using_SameParameter_And_DefaultId()
    {
        var name = "x";
        var type = typeof(int);
        var parameter = Expression.Parameter(type, name);

        var sut1 = IdExpression.New(parameter);
        var sut2 = IdExpression.New(parameter);

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void NodeType_Should_ReturnParameterExpression_When_CreatedWithParameterExpression()
    {
        var name = "x";
        var type = typeof(int);
        var parameter = Expression.Parameter(type, name);
        var sut = IdExpression.New(parameter);

        sut.NodeType.Should().Be(ExpressionType.Parameter);
    }

}
