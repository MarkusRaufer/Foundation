using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class MethodCallExpressionsTests
{
    private class MyClass
    {
        public int Sum(int x, int y) => x + y;
    }

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
}
