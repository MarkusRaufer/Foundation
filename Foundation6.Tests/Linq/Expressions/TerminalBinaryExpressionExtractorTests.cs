using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class TerminalBinaryExpressionExtractorTests
{
    private record Person(string Name, DateOnly BirthDay);

    [Test]
    public void Extract_Should_Return1Expression_When_LambdaContainsTerminalBinaryExpression()
    {
        Expression<Func<IDateTimeProvider, Person, bool>> lambda = (IDateTimeProvider dtp, Person p) => (dtp.Now.Year - p.BirthDay.Year) >= 18;

        var sut = new TerminalBinaryExpressionExtractor();

        var expressions = sut.Extract(lambda).ToArray();
        expressions.Length.Should().Be(1);

        var extracted = expressions.Single();

        IDateTimeProvider dtp = new DateTimeProvider(() => DateTime.Now);
        var person = new Person("Bob", new DateOnly(1970, 6, 3));

        var dtpMember = MemberExpressionHelper.ToMemberExpression<IDateTimeProvider, int>(x => x.Now.Year);
        var personMember = MemberExpressionHelper.ToMemberExpression<Person, int>(x => x.BirthDay.Year);
    }

    [Test]
    public void Extract_Should_Return1Expression_When_LambdaIs_NumberEquals4()
    {
        Expression<Func<int, bool>> lambda = (int number) => number == 4;

        var sut = new TerminalBinaryExpressionExtractor();

        var expressions = sut.Extract(lambda).ToArray();
        expressions.Length.Should().Be(1);

        var extracted = expressions[0];

        var parameter = Expression.Parameter(typeof(int), "number");
        var constant = Expression.Constant(4);

        var expected = Expression.Equal(parameter, constant!);

        extracted.EqualsToExpression(expected).Should().BeTrue();
    }
}
