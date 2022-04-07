using NUnit.Framework;

namespace Foundation.Collections.Generic;

[TestFixture]
public class HashChainTests
{
    [Test]
    public void Add_Should_HaveOneElement_When_Empty()
    {
        var sut = new HashChain<string>();
        Assert.AreEqual(0, sut.Count);

        sut.Add("one");
        Assert.AreEqual(1, sut.Count);
    }

    [Test]
    public void Add_Should_HaveTwoElements_When_ExistsOneElement()
    {
        var sut = new HashChain<string>
            {
                "one",
                "two"
            };
        Assert.AreEqual(2, sut.Count);
    }

    [Test]
    public void Contains_Should_ReturnTrue_When_Exists()
    {
        var sut = new HashChain<string>
            {
                "one",
                "two",
                "three"
            };
        Assert.IsTrue(sut.Contains("two"));
    }

    [Test]
    public void IsConsistant_Should_ReturnTrue_When_NotManipulated()
    {
        var sut = new HashChain<string>
            {
                "one",
                "two",
                "three"
            };
        Assert.IsTrue(sut.IsConsistant);
    }
}
