using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

internal class MyClass
{
    public DateTime GetDateTime() => new DateTime(2020, 6, 1);

    public int Sum(int x, int y) => x + y;
}

internal static class MyClassExtensions
{
    public static DateOnly GetDateOnly(this MyClass myClass) => new DateOnly(2020, 1, 1);
}

[TestFixture]
public class MethodCallExpressionsTests
{
    [Test]
    public void Call_Should_Return5_When_Called_Sum3And2()
    {
        var myClass = new MyClass();

        LambdaExpression lamba = (MyClass x) => x.Sum(3, 2) == 5;

        var extractor = new ExpressionExtractor();
        var methodCall = extractor.Extract<MethodCallExpression>(lamba).Single();
        methodCall.Should().NotBeNull();

        var result = methodCall.Call(myClass);
        result.Should().Be(5);
    }

    [Test]
    public void Call_Should_ReturnDateTime_When_Called_GetDateTime()
    {
        var myClass = new MyClass();

        LambdaExpression lamba = (MyClass x) => x.GetDateTime();

        var extractor = new ExpressionExtractor();
        var methodCall = extractor.Extract<MethodCallExpression>(lamba).Single();
        methodCall.Should().NotBeNull();

        var result = methodCall.Call(myClass);
        result.Should().Be(myClass.GetDateTime());
    }

    [Test]
    public void Call_Should_ReturnDateOnly_When_Called_ExtensionMethodGetDateOnly()
    {
        var myClass = new MyClass();
        var expected = myClass.GetDateOnly();

        LambdaExpression lamba = (MyClass x) => x.GetDateOnly();

        var extractor = new ExpressionExtractor();
        var methodCall = extractor.Extract<MethodCallExpression>(lamba).Single();
        methodCall.Should().NotBeNull();
        
        var result = methodCall.Call(myClass);
        result.Should().Be(expected);
    }
}
