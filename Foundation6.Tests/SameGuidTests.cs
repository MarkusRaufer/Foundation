using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class SameGuidTests
{
    [Test]
    public void New_Should_Return_DifferntGuids_When_UsingDifferentIntSeedValues()
    {
        {
            var guid1 = SameGuid.New(1);
            var guid2 = SameGuid.New(2);
            guid1.Should().NotBe(guid2);
        }
        {
            var guid1 = SameGuid.New(11);
            var guid2 = SameGuid.New(22);
            guid1.Should().NotBe(guid2);
        }
    }

    [Test]
    public void New_Should_Return_DifferntGuids_When_UsingDifferentStringSeedValues()
    {
        var guid1 = SameGuid.New("apples");
        var guid2 = SameGuid.New("oranges");
        guid1.Should().NotBe(guid2);
    }

    [Test]
    public void New_Should_Return_SameGuid_When_UsingSameIntSeed()
    {
        {
            var guid1 = SameGuid.New(1);
            var guid2 = SameGuid.New(1);
            guid1.Should().Be(guid2);
        }
        {
            var guid1 = SameGuid.New(11);
            var guid2 = SameGuid.New(11);
            guid1.Should().Be(guid2);
        }
    }

    [Test]
    public void New_Should_Return_SameGuid_When_UsingSameStringSeed()
    {
        var guid1 = SameGuid.New("apples");
        var guid2 = SameGuid.New("apples");
        guid1.Should().Be(guid2);
    }
}
