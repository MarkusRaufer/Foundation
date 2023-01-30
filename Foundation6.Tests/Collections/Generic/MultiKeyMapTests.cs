using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic;

[TestFixture]
public class MultiKeyMapTests
{
    [Test]
    public void Ctor_Should_HaveValue_When_DefaultCtorWasCalled()
    {
        var sut = new MultiKeyMap<string, string>();
        Assert.NotNull(sut);
    }

    [Test]
    public void Add_Should_AddOneItem_When_CalledOnce()
    {
        var sut = new MultiKeyMap<string, string>();
        sut.Add("1", "one");
        Assert.AreEqual(1, sut.Count);
    }

    [Test]
    public void Add_Should_AddThreeItem_When_CalledThreeTimes()
    {
        var sut = new MultiKeyMap<string, string>();
        sut.Clear();
        {
            sut.Add("1", "one");
            sut.Add("1", "one");
            sut.Add("1", "one");

            Assert.AreEqual(3, sut.Count);
            var kvp = sut.First();
            kvp.Key.Should().Be("1");
            kvp.Value.Should().Be("one");
        }
        sut.Clear();
        {
            sut.Add("1", "one");
            sut.Add("1", "eins");
            sut.Add("1", "uno");

            Assert.AreEqual(3, sut.Count);
            sut.Contains(Pair.New("1", "one"));
            sut.Contains(Pair.New("1", "eins"));
            sut.Contains(Pair.New("1", "uno"));
        }
    }

    [Test]
    public void Add_Should_Have2KeysAndFourValues_When_Added2KeysWith4Values()
    {
        var sut = new MultiKeyMap<string, string>();
        sut.Clear();

        sut.Add("one", "1");
        sut.Add("two", "2");
        sut.Add("eins", "1");
        sut.Add("zwei", "2");

        Assert.AreEqual(sut.Keys.Count, 4);
        Assert.AreEqual(sut.Values.Count, 4);
        Assert.AreEqual(4, sut.Count);

        sut.Contains(Pair.New("one", "1"));
        sut.Contains(Pair.New("eins", "1"));
        sut.Contains(Pair.New("two", "2"));
        sut.Contains(Pair.New("zwei", "2"));
    }

    [Test]
    public void Contains_Should_ReturnTrue_When_ItemExists()
    {
        var sut = new MultiKeyMap<string, string>();
        {
            sut.Add("1", "one");
            sut.Add("1", "one");
            sut.Add("1", "one");

            Assert.IsTrue(sut.Contains(Pair.New("1", "one")));
        }

        sut.Clear();
        {
            sut.Add("1", "one");
            sut.Add("1", "eins");
            sut.Add("1", "uno");

            Assert.IsTrue(sut.Contains(Pair.New("1", "one")));
            Assert.IsTrue(sut.Contains(Pair.New("1", "eins")));
            Assert.IsTrue(sut.Contains(Pair.New("1", "uno")));
        }
    }

    [Test]
    public void ContainsKey_Should_ReturnTrue_When_ItemExists()
    {
        var sut = new MultiKeyMap<string, string>();
        {
            sut.Add("1", "one");
            sut.Add("1", "one");
            sut.Add("1", "one");

            Assert.IsTrue(sut.ContainsKey("1"));
        }

        sut.Clear();
        {
            sut.Add("1", "one");
            sut.Add("1", "eins");
            sut.Add("1", "uno");

            Assert.IsTrue(sut.ContainsKey("1"));
        }
    }

    [Test]
    public void GetKeys()
    {
        var sut = new MultiKeyMap<string, string>();

        var kvp1 = Pair.New("1", "one");
        var kvp2 = Pair.New("1", "eins");
        var kvp3 = Pair.New("1", "uno");
        var kvp4 = Pair.New("2", "two");
        var kvp5 = Pair.New("2", "zwei");

        sut.Add(kvp1);
        sut.Add(kvp2);
        sut.Add(kvp3);
        sut.Add(kvp4);
        sut.Add(kvp5);

        var keys = sut.GetKeys(new[] { kvp3.Value, kvp4.Value, kvp5.Value }).ToArray();

        keys.Length.Should().Be(2);
        {
            var key = keys[0];
            key.Should().Be("1");
        }
        {
            var key = keys[1];
            key.Should().Be("2");
        }
    }

    [Test]
    public void Remove_Should_RemainOneKeyValueOfFour_When_DeletedThree()
    {
        var sut = new MultiKeyMap<string, string>();

        var kvp1 = Pair.New("1", "one");
        var kvp2 = Pair.New("1", "eins");
        var kvp3 = Pair.New("1", "uno");
        var kvp4 = Pair.New("2", "two");
        sut.Add(kvp1);
        sut.Add(kvp2);
        sut.Add(kvp3);
        sut.Add(kvp4);

        sut.Remove(kvp1).Should().BeTrue();
        sut.Count.Should().Be(3);

        sut.Remove(kvp2).Should().BeTrue();
        sut.Count.Should().Be(2);

        sut.Remove(kvp3).Should().BeTrue();
        sut.Count.Should().Be(1);
    }
}
