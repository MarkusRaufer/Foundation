using Shouldly;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation;

[TestFixture]
public class ScopeTests
{
    [Test]
    public void ReturnsOption_Should_ReturnNone_When_InvalidIf()
    {
        var x = 10;

        var option = Scope.ReturnsOption(() =>
        {
            if (20 == x) return Option.Some(x + 2);

            return Option.None<int>();
        });

        option.IsNone.ShouldBeTrue();
    }

    [Test]
    public void ReturnsOption_Should_ReturnSome_When_ValidIf()
    {
        var x = 10;
        var option = Scope.ReturnsOption(() => x % 2 == 0, () => x + 2);

        option.TryGet(out var value).ShouldBeTrue();
        value.ShouldBe(x + 2);
    }

    [Test]
    public void Returns_Should_ReturnAListOfValues_When_LoopIsUsed()
    {
        var numbers = Enumerable.Range(1, 10);

        var result = Scope.Returns(() =>
        {
            var evenNumbers = Enumerable.Empty<int>();
            foreach(var number in numbers)
            {
                if(number % 2 == 0) evenNumbers = evenNumbers.Append(number);
            }
            return evenNumbers;
        }).ToArray();

        result.Length.ShouldBe(5);
        result.ShouldBe(new int[] { 2, 4, 6, 8, 10 });
    }

    [Test]
    public void Returns_Should_ReturnValue_When_Called()
    {
        var number = 10;

        var result = Scope.Returns(() =>
        {
            if (number % 2 == 0)
            {
                var x = number * -1;
                if (number < 100) return x * 10;
            }
            
            return number;
        });

        result.ShouldBe(-100);
    }

    [Test]
    public void ReturnsMany_Should_ReturnValue_When_Called()
    {
        var max = 10;
        var expected = Enumerable.Range(1, max).ToArray();

        var result = Scope.ReturnsMany(() =>
        {
            IEnumerable<int> numbers()
            {
                foreach (var number in Enumerable.Range(1, max))
                {
                    yield return number;
                }
            }

            return numbers();
        }).ToArray();

        result.ShouldBe(expected);
    }
}
