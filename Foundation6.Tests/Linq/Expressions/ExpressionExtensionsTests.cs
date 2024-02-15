using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class ExpressionExtensionsTests
{
    private enum Gender
    {
        Female,
        Male
    };

    private record Person(string Name, Gender Gender, int Age);

    [Test]
    public void GetExpressionHashCode_Should_ReturnDifferentHashCodes_When_UsingBinaryExpressionIsEqual_DifferentParameterTypes()
    {
        var hashCode1 = Scope.Returns(() =>
        {
            var left = Expression.Parameter(typeof(string), "x");
            var right = Expression.Constant("5");

            var expression = Expression.MakeBinary(ExpressionType.Equal, left, right);
            return expression.GetExpressionHashCode();
        });

        var hashCode2 = Scope.Returns(() =>
        {
            var left = Expression.Parameter(typeof(int), "x");
            var right = Expression.Constant(5);

            var expression = Expression.MakeBinary(ExpressionType.Equal, right, left);
            return expression.GetExpressionHashCode();
        });

        hashCode1.Should().NotBe(hashCode2);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_UsingBinaryExpressionIsEqual_SameParameterTypes()
    {
        var left = Expression.Parameter(typeof(string), "x");
        var right = Expression.Constant("Test");

        var hashCode1 = Scope.Returns(() =>
        {
            var expression = Expression.MakeBinary(ExpressionType.Equal, left, right);
            return expression.GetExpressionHashCode();
        });

        var hashCode2 = Scope.Returns(() =>
        {
            var expression = Expression.MakeBinary(ExpressionType.Equal, right, left);
            return expression.GetExpressionHashCode();
        });

        hashCode1.Should().Be(hashCode2);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_UsingLambdaExpression_BodyIsConstant_DifferentParameterNames()
    {
        Expression<Func<string, bool>> expression1 = x => true;
        var hashCodeWithX = expression1.GetExpressionHashCode();

        Expression<Func<string, bool>> expression2 = a => true;
        var hashCodeWithA = expression2.GetExpressionHashCode();

        hashCodeWithX.Should().Be(hashCodeWithA);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_LambdaExpression_BodyNotEqual()
    {
        var p1 = Expression.Parameter(typeof(string));
        var p2 = Expression.Parameter(typeof(string));

        var hc1 = p1.GetExpressionHashCode();
        var hc2 = p2.GetExpressionHashCode();

        Expression<Func<int, bool>> expression1 = x => x != 12;
        var hashCodeWithX = expression1.GetExpressionHashCode();

        Expression<Func<int, bool>> expression2 = a => 12 != a;
        var hashCodeWithA = expression2.GetExpressionHashCode();

        hashCodeWithX.Should().Be(hashCodeWithA);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_LambdaExpression_BodyOr()
    {
        Expression<Func<int, bool>> expression1 = x => x == 5 || x == 7;
        var hashCodeWithX = expression1.GetExpressionHashCode();

        Expression<Func<int, bool>> expression2 = a => 7 == a || a == 5;
        var hashCodeWithA = expression2.GetExpressionHashCode();

        hashCodeWithX.Should().Be(hashCodeWithA);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_LambdaExpression_BodyOr_DifferentBinaryExpressions()
    {
        Expression<Func<int, bool>> expression1 = x => x == 5 || x == 7;
        var hashCodeWithX = expression1.GetExpressionHashCode();

        Expression<Func<int, bool>> expression2 = a => 7 == a || a != 5;
        var hashCodeWithA = expression2.GetExpressionHashCode();

        hashCodeWithX.Should().NotBe(hashCodeWithA);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_UsingParameterExpression_DifferentNames_With_ConsiderNamesFalse()
    {
        var hashCode1 = Expression.Parameter(typeof(string), "x").GetExpressionHashCode(true);
        var hashCode2 = Expression.Parameter(typeof(string), "a").GetExpressionHashCode(true);

        hashCode1.Should().Be(hashCode2);
    }

    [Test]
    public void GetParameters_Should_Return1parameter_When_UsingMemberExpression()
    {
        Expression<Func<Person, bool>> expression = (Person person) => person.Name.Length == 0;

        var parameter = expression.Body.GetParameters().Single();

        parameter.Name.Should().Be("person");
    }

    [Test]
    public void GetParameters_Should_Return1parameter_When_UsingParameterExpression()
    {
        var p = Expression.Parameter(typeof(string), "x");
        
        var parameters = p.GetParameters().ToArray();

        parameters.Length.Should().Be(1);
        parameters.Should().Contain(p);
    }

    [Test]
    public void GetParameters_Should_Return1parameter_When_UsingSubtractExpression()
    {
        Expression<Func<int, int, bool>> expression = (a, b) => (a - b) == 0;

        var parameters = expression.Body.GetParameters().ToArray();

        parameters.Length.Should().Be(2);

        parameters[0].Name.Should().Be("a");
        parameters[1].Name.Should().Be("b");
    }

    [Test]
    public void GetParameters_Should_Return1parameters_When_UsingTerminalBinaryExpression()
    {
        var p = Expression.Parameter(typeof(string), "x");
        var c = Expression.Constant("test");
        var be = Expression.MakeBinary(ExpressionType.Equal, p, c);

        var parameters = be.GetParameters().ToArray();

        parameters.Length.Should().Be(1);
        parameters.Should().Contain(p);
    }

    [Test]
    public void GetParameters_Should_Return1parameters_When_UsingComplexBinaryExpression()
    {
        var pX = Expression.Parameter(typeof(int), "x");
        var c2 = Expression.Constant(2);
        var xGreaterThanC2 = Expression.MakeBinary(ExpressionType.GreaterThan, pX, c2);

        var c6 = Expression.Constant(6);
        var xLessThanC6 = Expression.MakeBinary(ExpressionType.LessThan, pX, c6);

        var and = Expression.MakeBinary(ExpressionType.And, xGreaterThanC2, xLessThanC6);

        var parameters = and.GetParameters().ToArray();

        parameters.Length.Should().Be(1);
        parameters.Should().Contain(pX);
    }


    [Test]
    public void GetParameters_Should_Return1parameters_When_UsingUnaryExpression()
    {
        Expression<Func<int, string, bool>> lambda = (number, name) => number == 2 && name == "John";

        var parameters = lambda.GetParameters().ToArray();

        var expected1 = Expression.Parameter(typeof(int), "number");
        var expected2 = Expression.Parameter(typeof(string), "name");

        parameters.Length.Should().Be(2);

        var expected = new[] { expected1, expected2 };
        parameters.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetParameters_Should_Return2parameters_When_UsingLambdaExpression()
    {
        var p = Expression.Parameter(typeof(int), "x");
        var n = Expression.Negate(p);

        var parameters = n.GetParameters().ToArray();

        parameters.Length.Should().Be(1);

        parameters[0].Should().Be(p);
    }

    [Test]
    public void IsTerminal_Should_ReturnTrue_When_Expression_ContainsConvert_BinaryExpressionLeftAndRightAreTerminal()
    {
        Expression<Func<Person, bool>> expression = p => p.Gender == Gender.Male;
        var lambda = expression as LambdaExpression;

        Assert.IsTrue(lambda.Body.IsTerminal());
    }

    [Test]
    public void IsTerminal_Should_ReturnTrue_When_Expression_ContainsModulo_BinaryExpressionLeftAndRightAreTerminal()
    {
        Expression<Func<Person, bool>> expression = p => p.Gender == Gender.Male && p.Age % 2 == 0;
        var lambda = expression as LambdaExpression;

        var binary = (BinaryExpression)lambda.Body;
        Assert.IsTrue(binary.Left.IsTerminal());
        Assert.IsTrue(binary.Right.IsTerminal());
    }


    [Test]
    public void IsTerminal_Should_ReturnTrue_When_Expression_LeftIsSubtractAndRightIsConstant()
    {
        Expression<Func<int, int, bool>> expression = (a, b) => (a - b) == 2;

        expression.Body.IsTerminal().Should().BeTrue();
    }
}
