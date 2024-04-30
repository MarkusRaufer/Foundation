using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class ExpressionExtractorTests
{
    [Test]
    public void Extract_WithGenericParameter_Should_Return1ParameterExpression_When_UsingPredicate_LambdaHas2DifferentParamters()
    {
        LambdaExpression lambda = (string str, int x) => str == "A" && x == 3;
        var parameter = Expression.Parameter(typeof(int), "x");
        var sut = new ExpressionExtractor();

        var extracted = sut.Extract<ParameterExpression>(lambda, x => x.EqualsToExpression(parameter)).ToArray();

        extracted.Length.Should().Be(1);
        extracted[0].EqualsToExpression(parameter);
    }

    [Test]
    public void Extract_WithoutGenericParameter_Should_Return3Expressions_When_UsingPredicate_LambdaHas2Parameters()
    {
        LambdaExpression lambda = (string str, int x) => str == "A" && x == 3;
        var sut = new ExpressionExtractor();
        var parameter = Expression.Parameter(typeof(int), "x");

        var extracted = sut.Extract(lambda, x => x.HasParameter(parameter)).ToArray();

        extracted.Length.Should().Be(3);

        var body = (BinaryExpression)lambda.Body;
        body.EqualsToExpression(extracted[0]).Should().BeTrue();

        body.Right.EqualsToExpression(extracted[1]).Should().BeTrue();

        extracted[2].EqualsToExpression(parameter);
    }
}
