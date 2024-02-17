using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class ExpressionReplacerTests
{
    private record Person(string Name, DateTime BirtDay);

    [Test]
    public void Replace_Should_ReturnTheExpressionWithTheReplacedExpression_When_Using_MemeberAccessAndGreaterThanConstant()
    {
        Expression<Func<IDateTimeProvider, bool>> lambda = (dt) => dt.Now.Day > 5;

        var be = (BinaryExpression)lambda.Body;
        var left = be.Left;
        var replacement = Expression.Constant(10);
        var constant = Expression.Constant(5);

        var sut = new ExpressionReplacer();

        var actual = sut.Replace(lambda, left, Expression.Constant(10));

        var expectedBinary = Expression.MakeBinary(ExpressionType.GreaterThan, replacement, constant);
        var expectedLambda = Expression.Lambda(expectedBinary, Expression.Parameter(typeof(DateTime), "dt"));

        expectedLambda.EqualsToExpression(actual);
    }

    [Test]
    public void Replace_Should_ReturnTheExpressionWithTheReplacedExpression_When_Using_MemberAccessAndSubtractAndGreatherThanConstant()
    {
        Expression<Func<IDateTimeProvider, Person, bool>> lambda = (dtp, p) => (dtp.Now.Year - p.BirtDay.Year) >= 18;

        var be = (BinaryExpression)lambda.Body;
        var subtract = (BinaryExpression)be.Left;
        var member = (MemberExpression)subtract.Left;

        var replacement = Expression.Constant(2000);
        var constant = Expression.Constant(18);

        var sut = new ExpressionReplacer();

        var actual = sut.Replace(lambda, member, replacement);

        var expectedBinary = Expression.MakeBinary(ExpressionType.GreaterThan, replacement, constant);
        var expectedLambda = Expression.Lambda(expectedBinary, Expression.Parameter(typeof(DateTime), "dtp"), Expression.Parameter(typeof(Person), "p"));

        expectedLambda.EqualsToExpression(actual, ignoreNames: false);
    }
}
