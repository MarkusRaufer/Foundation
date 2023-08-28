using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class CountableTests
{
    [Test]
    public void Dec_Should_Have_Count1_HashCodeSameAsValue_When_CountIsSetTo2()
    {
        var value = "test";
        var sut = Countable.New(value, 2);
        sut.Dec();

        Assert.AreEqual(value, sut.Value);
        Assert.AreEqual(value.GetHashCode(), sut.GetHashCode());
        Assert.AreEqual(1, sut.Count);
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_HashCodeIncludesCountIsTrue_ValuesAreSame_But_CountersAreDifferent()
    {
        var value = "test";
        var sut1 = Countable.New(value, true);
        var sut2 = Countable.New(value, true);
        sut2.Inc();

        Assert.IsFalse(sut1 == sut2);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_HashCodeIncludesCountIsTrue_ValuesAreSame_But_CountersAreSame()
    {
        var value = "test";
        var sut1 = Countable.New(value, true);
        var sut2 = Countable.New(value, true);

        Assert.IsTrue(sut1 == sut2);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_OnlyValueIsSet_AndCountersAreDifferent()
    {
        var value = "test";
        var sut1 = Countable.New(value);
        var sut2 = Countable.New(value);
        sut2.Inc();

        Assert.AreEqual(sut1, sut2);
    }

    [Test]
    public void Inc_Should_Have_Count2_HashCodeSameAsValue_When_OnlyValueIsSet()
    {
        var value = "test";
        var sut = Countable.New(value);
        sut.Inc();

        Assert.AreEqual(value, sut.Value);
        Assert.AreEqual(value.GetHashCode(), sut.GetHashCode());
        Assert.AreEqual(2, sut.Count);
    }

    [Test]
    public void New_Should_Have_Count1_HashCodeSameAsValue_When_OnlyValueIsSet()
    {
        var value = "test";
        var sut = Countable.New(value);

        Assert.AreEqual(value, sut.Value);
        Assert.AreEqual(value.GetHashCode(), sut.GetHashCode());
        Assert.AreEqual(1, sut.Count);
    }
}
