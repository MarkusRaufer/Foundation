using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Foundation;

[TestFixture]
public class ScopeTests
{
    [Test]
    public void MayReturn_Should_ReturnNone_When_InvalidIf()
    {
        var x = 10;
        var option = Scope.MayReturn(() =>
        {
            if (20 == x) return Option.Some(x + 2);

            return Option.None<int>();
        });

        option.IsNone.Should().Be(true);
    }

    [Test]
    public void MayReturnIf_Should_ReturnSome_When_ValidIf()
    {
        var x = 10;
        var option = Scope.MayReturnIf(() => x % 2 == 0, () => x + 2);

        option.TryGet(out var value).Should().Be(true);
        value.Should().Be(x + 2);
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

        result.Length.Should().Be(5);
        result.Should().Contain(new int[] { 2, 4, 6, 8, 10 });
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

        result.Should().Be(-100);
    }
}
