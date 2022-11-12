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
    private readonly MultiKeyMap<string, string> _sut;

    public MultiKeyMapTests()
    {
        _sut = new MultiKeyMap<string, string>();
    }

    [TearDown]
    public void TearDown()
    {
        _sut.Clear();
    }

    [Test]
    public void Ctor_Should_HaveValue_When_DefaultCtorWasCalled()
    {
        var sut = new MultiKeyMap<string, string>();
        Assert.NotNull(sut);
    }

    [Test]
    public void Add_Should_AddOneItem_When_CalledOnce()
    {
        _sut.Add("1", "one");
        Assert.AreEqual(1, _sut.Count);
    }

    [Test]
    public void Add_Should_AddThreeItem_When_CalledThreeTimes()
    {
        {
            _sut.Add("1", "one");
            _sut.Add("1", "one");
            _sut.Add("1", "one");

            Assert.AreEqual(3, _sut.Count);
            Assert.True(_sut.All(x => x.Key == "1" && x.Value == "one"));
        }
        _sut.Clear();
        {
            _sut.Add("1", "one");
            _sut.Add("1", "eins");
            _sut.Add("1", "uno");

            Assert.AreEqual(3, _sut.Count);
            _sut.Contains(Pair.New("1", "one"));
            _sut.Contains(Pair.New("1", "eins"));
            _sut.Contains(Pair.New("1", "uno"));
        }
    }

    [Test]
    public void Contains_Should_ReturnTrue_When_ItemExists()
    {
        {
            _sut.Add("1", "one");
            _sut.Add("1", "one");
            _sut.Add("1", "one");

            Assert.IsTrue(_sut.Contains(Pair.New("1", "one")));
        }

        _sut.Clear();
        {
            _sut.Add("1", "one");
            _sut.Add("1", "eins");
            _sut.Add("1", "uno");

            Assert.IsTrue(_sut.Contains(Pair.New("1", "one")));
            Assert.IsTrue(_sut.Contains(Pair.New("1", "eins")));
            Assert.IsTrue(_sut.Contains(Pair.New("1", "uno")));
        }
    }

    [Test]
    public void ContainsKey_Should_ReturnTrue_When_ItemExists()
    {
        {
            _sut.Add("1", "one");
            _sut.Add("1", "one");
            _sut.Add("1", "one");

            Assert.IsTrue(_sut.ContainsKey("1"));
        }

        _sut.Clear();
        {
            _sut.Add("1", "one");
            _sut.Add("1", "eins");
            _sut.Add("1", "uno");

            Assert.IsTrue(_sut.ContainsKey("1"));
        }
    }
}
