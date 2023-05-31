using NUnit.Framework;
using System;

namespace Foundation.Buffers;

[TestFixture]
public class IndexSplitEnumeratorTests
{
    [Test]
    public void MoveNext_Should_Find_Separator_When_Calling_MoveNext()
    {
        var span = "123.4567.89".AsSpan();

        var sut = new IndexSplitEnumerator<char>(span, new[] {(0, 3), (4, 4), (9, 2)});

        Assert.IsTrue(sut.MoveNext());
        Assert.AreEqual(sut.Current.ToString(), "123");

        Assert.IsTrue(sut.MoveNext());
        Assert.AreEqual(sut.Current.ToString(), "4567");

        Assert.IsTrue(sut.MoveNext());
        Assert.AreEqual(sut.Current.ToString(), "89");

        Assert.IsFalse(sut.MoveNext());
    }
}
