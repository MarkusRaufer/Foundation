using NUnit.Framework;
using System;
using System.Buffers;
using System.Linq;

namespace Foundation.Buffers
{
    [TestFixture]
    public class ReadOnlyMemoryExtensionsTests
    {
        [Test]
        public void IndicesFromEnd_Should_Return2IndicesInReverseOrder_When_Selector_Matches()
        {
            var str = "ABCDABCD".AsMemory();
            var indexes = str.IndicesFromEnd("BC".AsMemory()).ToArray();
            Assert.AreEqual(2, indexes.Length);
            Assert.AreEqual(5, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }

        [Test]
        public void IndicesFromEnd_Should_Return4IndicesInReverseOrder_When_Selector_Matches()
        {
            var str = "ADDDDD".AsMemory();
            var indexes = str.IndicesFromEnd("DD".AsMemory()).ToArray();

            Assert.AreEqual(4, indexes.Length);

            Assert.AreEqual(4, indexes[0]);
            Assert.AreEqual(3, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
            Assert.AreEqual(1, indexes[3]);
        }

        [Test]
        public void IndicesOf_Should_Return2Indices_When_Selector_Matches()
        {
            var str = "ABCDABCD".AsMemory();
            var indexes = str.IndicesOf("BC".AsMemory()).ToArray();
            Assert.AreEqual(2, indexes.Length);
            Assert.AreEqual(1, indexes[0]);
            Assert.AreEqual(5, indexes[1]);
        }

        [Test]
        public void IndicesOf_Should_Return4Indices_When_Selector_Matches()
        {
            var str = "ADDDDD".AsMemory();
            var indexes = str.IndicesOf("DD".AsMemory()).ToArray();
            Assert.AreEqual(4, indexes.Length);
            Assert.AreEqual(1, indexes[0]);
            Assert.AreEqual(2, indexes[1]);
            Assert.AreEqual(3, indexes[2]);
            Assert.AreEqual(4, indexes[3]);
        }

        [Test]
        public void IndicesOfAny_Should_Return_Tuples_When_Selectors_MatchOnce()
        {
            var numbers = "A,B,C,D".AsMemory();
            var indexes = numbers.IndicesOfAny(new[] { "B".AsMemory(), "D".AsMemory() }).ToArray();
            Assert.AreEqual(2, indexes.Length);
            Assert.AreEqual((0, 2), indexes[0]);
            Assert.AreEqual((1, 6), indexes[1]);
        }

        [Test]
        public void IndicesOfAny_Should_Return_Tuples_When_Selectors_MatchSeveralTimes()
        {
            var numbers = "dad of dads".AsMemory();
            var selectors = new[] { "dad".AsMemory(), "d".AsMemory() };

            var indexes = numbers.IndicesOfAny(selectors).ToArray();

            Assert.AreEqual(6, indexes.Length);

            Assert.AreEqual((selector: 0, index: 0), indexes[0]);
            Assert.AreEqual((selector: 1, index: 0), indexes[1]);
            Assert.AreEqual((selector: 1, index: 2), indexes[2]);
            Assert.AreEqual((selector: 0, index: 7), indexes[3]);
            Assert.AreEqual((selector: 1, index: 7), indexes[4]);
            Assert.AreEqual((selector: 1, index: 9), indexes[5]);
        }
    }
}
