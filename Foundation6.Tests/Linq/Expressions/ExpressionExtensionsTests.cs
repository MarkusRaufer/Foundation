using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class ExpressionExtensionsTests
{
    private enum Gender
    {
        Female,
        Male
    };

    private record BirthDayChild(string Name, DateTime BrithDay);

    private record Person(string Name, Gender Gender, int Age);

    [Test]
    public void EqualsToExpression_Should_True_When_UsingBinaryExpressionWithSameDateOnly()
    {
        LambdaExpression lambda = (DateOnly x) => x == new DateOnly(2020, 1, 10) || x == new DateOnly(2020, 1, 10);

        var be = lambda.Body as BinaryExpression;
        be.Should().NotBeNull();

        var eq = be!.Left.EqualsToExpression(be!.Right);
        eq.Should().BeTrue();
    }

    [Test]
    public void ExtractExpressions_Should_Return1MemberExpression_When_UsingLambdaExpression_WithObjectProperty()
    {
        LambdaExpression lambda = (DateTime x) => x.Day > 5;

        var memberExpressions = lambda.ExtractExpressions<MemberExpression>().ToArray();

        memberExpressions.Length.Should().Be(1);
        
        var memberExpression = memberExpressions[0];
        memberExpression.ToString().Should().Be("x.Day");
    }

    [Test]
    public void ExtractExpressions_Should_Return1MemberExpression_When_UsingLambdaExpression_WithMemberAccessAndObjectProperty()
    {
        LambdaExpression lambda = (DateTime x) => x.EndOfWeek(true).Day == 7;

        var memberExpressions = lambda.ExtractExpressions<MemberExpression>().ToArray();

        memberExpressions.Length.Should().Be(1);

        var memberExpression = memberExpressions[0];
        LambdaExpression lambda2 = (DateTime x) => true;
        memberExpression.ToString().Should().Be("x.EndOfWeek().Day");
    }

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
        var hashCodeWithX = expression1.GetExpressionHashCode(ignoreName: true);

        Expression<Func<string, bool>> expression2 = a => true;
        var hashCodeWithA = expression2.GetExpressionHashCode(ignoreName: true);

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
        var hashCodeWithX = expression1.GetExpressionHashCode(ignoreName: true);

        Expression<Func<int, bool>> expression2 = a => 12 != a;
        var hashCodeWithA = expression2.GetExpressionHashCode(ignoreName: true);

        hashCodeWithX.Should().Be(hashCodeWithA);
    }

    [Test]
    public void GetExpressionHashCode_Should_ReturnSameHashCodes_When_LambdaExpression_BodyOr()
    {
        Expression<Func<int, bool>> expression1 = x => x == 5 || x == 7;
        var hashCodeWithX = expression1.GetExpressionHashCode(ignoreName: true);

        Expression<Func<int, bool>> expression2 = a => 7 == a || a == 5;
        var hashCodeWithA = expression2.GetExpressionHashCode(ignoreName: true);

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
    public void GetParameters_Should_Return1parameter_When_UsingTMethodCallExpression()
    {
        Expression<Func<IDateTimeProvider, bool>> expression = x => x.DayNow() == 5;

        var parameters = expression.Body.GetParameters().ToArray();

        parameters.Length.Should().Be(1);
        var parameter = parameters[0];
        parameter.Name.Should().Be("x");
    }

    [Test]
    public void GetParameters_Should_Return1parameters_When_UsingComplexBinaryExpression()
    {
        Expression<Func<int, bool>> lambda = x => x > 2 && x < 6;

        var parameters = lambda.GetParameters().ToArray();

        parameters.Length.Should().Be(1);
        parameters[0].Name.Should().Be("x");

        parameters = lambda.Body.GetParameters().ToArray();

        parameters.Length.Should().Be(2);
        parameters[0].Name.Should().Be("x");
        parameters[1].Name.Should().Be("x");

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
    public void IsTerminalPredicate_Should_ReturnTrue_When_Expression_ContainsConvert_BinaryExpressionLeftAndRightAreTerminal()
    {
        LambdaExpression lambda = (Person p) => p.Gender == Gender.Male;

        lambda.Body.IsTerminalPredicate().Should().BeTrue();
    }

    [Test]
    public void IsTerminalPredicate_Should_ReturnTrue_When_Expression_ContainsModulo()
    {
        Expression<Func<Person, bool>> expression = p =>p.Age % 2 == 0;
        var lambda = expression as LambdaExpression;

        var binary = (BinaryExpression)lambda.Body;

        binary.IsTerminalPredicate().Should().BeTrue();
    }

    [Test]
    public void IsPredicate_Should_ReturnTrue_When_Expression_IsABinaryExpression_And_IsTerminal()
    {
        var a = Expression.Parameter(typeof(int), "a");
        var b = Expression.Parameter(typeof(int), "b");

        var expression = Expression.MakeBinary(ExpressionType.Equal, a, b);

        expression.IsPredicate().Should().BeTrue();
    }

    [Test]
    public void IsPredicate_Should_ReturnTrue_When_Expression_IsAnExpressionThatRepresentsABinaryExpression_And_IsTerminal()
    {
        Expression<Func<int, int, bool>> expression = (a, b) => a == b;

        expression.Body.IsPredicate().Should().BeTrue();
    }

    [Test]
    public void IsTerminal_Should_ReturnTrue_When_Expression_LeftIsSubtractAndRightIsConstant()
    {
        Expression<Func<int, int, bool>> expression = (a, b) => (a - b) == 2;

        expression.Body.IsTerminalPredicate().Should().BeTrue();
    }

    [Test]
    public void IsTerminalPredicatel_Should_ReturnFalse_When_Expression_ContainsModulo_BinaryExpressionLeftAndRightAreTerminal()
    {
        Expression<Func<Person, bool>> expression = p => p.Gender == Gender.Male && p.Age % 2 == 0;
        var lambda = expression as LambdaExpression;

        var binary = (BinaryExpression)lambda.Body;

        binary.IsTerminalPredicate().Should().BeFalse();
    }

    [Test]
    public void IsTerminalPredicate_Should_ReturnTrue_When_Expression_Contains_InterfaceAndClassType_LeftBinaryIsSubtract_RightConstant()
    {
        Expression<Func<IDateTimeProvider, BirthDayChild, bool>> expression = (dtp, c) => (dtp.Now.Year - c.BrithDay.Year) >= 18;
        var lambda = expression as LambdaExpression;

        var binary = (BinaryExpression)lambda.Body;

        binary.IsTerminalPredicate().Should().BeTrue();
    }
}

public static class ProviderExtensions
{
    public static int DayNow(this IDateTimeProvider provider) => provider.Now.Day;
}
