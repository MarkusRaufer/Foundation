using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class VersionTests
{
    [Test]
    public void Ctor_Should_ReturnValidVersion_When_MajorIs1_MinorIs2_PatchIs3()
    {
        var sut = new Version(1, 2, 3);

        sut.Major.Should().Be(1);
        sut.Minor.Should().Be(2);
        sut.Patch.Should().Be(3);
    }

    [Test]
    public void IncrementMajor_Should_ReturnVersionWithIncrementedMajor_When_Called()
    {
        var sut = new Version(1, 2, 3);
        var version = sut.IncrementMajor();

        version.Major.Should().Be(2);
        version.Minor.Should().Be(2);
        version.Patch.Should().Be(3);
    }

    [Test]
    public void IncrementMinor_Should_ReturnVersionWithIncrementedMinor_When_Called()
    {
        var sut = new Version(1, 2, 3);
        var version = sut.IncrementMinor();

        version.Major.Should().Be(1);
        version.Minor.Should().Be(3);
        version.Patch.Should().Be(3);
    }

    [Test]
    public void IncrementPatch_Should_ReturnVersionWithIncrementedPatch_When_Called()
    {
        var sut = new Version(1, 2, 3);
        var version = sut.IncrementPatch();

        version.Major.Should().Be(1);
        version.Minor.Should().Be(2);
        version.Patch.Should().Be(4);
    }
}
