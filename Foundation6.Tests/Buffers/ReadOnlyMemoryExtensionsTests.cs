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
            var indexes = numbers.IndicesOfAny(new[] { "dad".AsMemory(), "d".AsMemory() }).ToArray();
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
