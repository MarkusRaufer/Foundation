namespace Foundation.Buffers;

using NUnit.Framework;
using System;
using System.Buffers;
using System.Linq;

[TestFixture]
public class ReadOnlySpanExtensionsTests
{
    [Test]
    public void IndexFromEnd_Should_ReturnAnIndex_When_CharFound()
    {
        var sut = "01234>6789".AsSpan();
        var index = sut.IndexFromEnd('>');
        Assert.AreEqual(5, index);
    }

    [Test]
    public void IndexFromEnd_Should_ReturnNotFound_When_CharNotFound()
    {
        var sut = "Invoice".AsSpan();
        var index = sut.IndexFromEnd('>');
        Assert.AreEqual(-1, index);
    }

    [Test]
    public void IndexFromEnd_Should_ReturnAnIndex_When_StringFound()
    {
        var sut = "01234>>7>9".AsSpan();
        var index = sut.IndexFromEnd(">>".AsSpan());
        Assert.AreEqual(5, index);
    }

    [Test]
    public void IndexLengthTuples_Should_Return_Index_Length_Tuples_When_Using_Predicate_As_Separator()
    {
        var sut = "123.4567.89".AsSpan();
        var tuples = sut.IndexLengthTuples('.').ToArray();
        Assert.AreEqual(3, tuples.Length);

        Assert.AreEqual((0, 3), tuples[0]);
        Assert.AreEqual((4, 4), tuples[1]);
        Assert.AreEqual((9, 2), tuples[2]);
    }

    [Test]
    public void IndicesOf_Should_Return_Indices_When_Using_AStringWithSingleSeparatorAndMultipleOccurrencies()
    {
        var sut = "123.4567.89".AsSpan();
        var indices = sut.IndicesOf('.').ToArray();
        Assert.AreEqual(2, indices.Length);

        Assert.AreEqual(3, indices[0]);
        Assert.AreEqual(8, indices[1]);
    }

    [Test]
    public void IndicesOf_Should_Return_1Index_When_Using_AStringWithSingleSeparatorAndMultipleOccurrenciesAndStopAfter1Hit()
    {
        var sut = "123.4567.89".AsSpan();
        var indices = sut.IndicesOfAny(new[] { '.' }.AsSpan(), 1).ToArray();

        Assert.AreEqual(1, indices.Length);
        Assert.AreEqual(3, indices[0]);
    }

    [Test]
    public void IndicesOf_Should_Return_Indices_When_Using_MultipleSeparators()
    {
        var sut = "123_4567.89".AsSpan();
        var indices = sut.IndicesOfAny(new[] { '.', '_' }.AsSpan()).ToArray();
        Assert.AreEqual(2, indices.Length);

        Assert.AreEqual(3, indices[0]);
        Assert.AreEqual(8, indices[1]);
    }

    [Test]
    public void IndicesOf_Should_Return2Indices_When_Using_String()
    {
        var sut = "123_45.67.45.89".AsSpan();
        var indices = sut.IndicesOf("45").ToArray();
        Assert.AreEqual(2, indices.Length);

        Assert.AreEqual(4, indices[0]);
        Assert.AreEqual(10, indices[1]);
    }

    [Test]
    public void IndicesOf_Should_Return2Indices_When_UsingStringStopAfter1Hit()
    {
        var sut = "123_45.67.45.89".AsSpan();
        var indices = sut.IndicesOf("45", StringComparison.InvariantCulture, 1);

        Assert.AreEqual(1, indices.Count);
        Assert.AreEqual(4, indices.First());
    }

    [Test]
    public void IndexOf_Should_ReturnIndexOfString_When_UsingString()
    {
        {
            var search = ">>".AsSpan();
            var sut = $"01234{search}789".AsSpan();

            var index = sut.IndexOf(3, search);
            Assert.AreEqual(5, index);
        }
        {
            var search = ">>".AsSpan();
            var sut = $"01234{search}".AsSpan();

            var index = sut.IndexOf(3, search);
            Assert.AreEqual(5, index);
        }

    }

    [Test]
    public void Split_Should_Return_CharSplitEnumerator_When_Using_Value_As_Separator()
    {
        var sut = "123.4567.89".AsSpan();
        var chunks = sut.Split(new [] { '.' }.AsSpan());

        Assert.IsTrue(chunks.MoveNext());
        Assert.AreEqual("123", chunks.Current.ToString());

        Assert.IsTrue(chunks.MoveNext());
        Assert.AreEqual("4567", chunks.Current.ToString());

        Assert.IsTrue(chunks.MoveNext());
        Assert.AreEqual("89", chunks.Current.ToString());

        Assert.IsFalse(chunks.MoveNext());
    }

    [Test]
    public void Split_Should_Return_CharSplitEnumerator_When_Using_Values_As_Separator()
    {
        var sut = "123_4567.89".AsSpan();
        var chunks = sut.Split(new [] { '.', '_' }.AsSpan());

        Assert.IsTrue(chunks.MoveNext());
        Assert.AreEqual("123", chunks.Current.ToString());

        Assert.IsTrue(chunks.MoveNext());
        Assert.AreEqual("4567", chunks.Current.ToString());

        Assert.IsTrue(chunks.MoveNext());
        Assert.AreEqual("89", chunks.Current.ToString());

        Assert.IsFalse(chunks.MoveNext());
    }

    [Test]
    public void Split_Should_Return_CharSplitEnumerator_When_Using_An_NonExisting_Separator()
    {
        var sut = "123.4567.89".AsSpan();
        var chunks = sut.Split(new[] { '_' });

        Assert.IsFalse(chunks.MoveNext());
        Assert.IsFalse(chunks.MoveNext());
    }

    [Test]
    public void TrimAll_Should_ReturnStringWithoutQuotes_When_UsingStringWithQuotes()
    {
        var expected = "test";
        var sut = $"  \"{expected}\"  ".AsSpan();
        var actual = sut.TrimAll('\"').ToString();

        Assert.AreEqual(expected, actual);
    }
}
