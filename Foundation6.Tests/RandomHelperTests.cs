using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    [TestFixture]
    public class RandomHelperTests
    {
        [Test]
        public void GetRandomOrdinalDouble_ShouldReturnSameValue_When_UsingSameIndex()
        {
            var number1 = RandomHelper.GetRandomOrdinalDouble(5, 0, 10);
            var number2 = RandomHelper.GetRandomOrdinalDouble(5, 0, 10);

            Assert.AreEqual(number1, number2);
        }

        [Test]
        public void GetRandomOrdinalDouble_ShouldReturnSameValues_When_UsingSameIndices()
        {
            var numbers1 = RandomHelper.GetRandomOrdinalDouble(new[] { 3, 5, 8 }, 0, 10).ToArray();
            var numbers2 = RandomHelper.GetRandomOrdinalDouble(new[] { 3, 5, 8 }, 0, 10).ToArray();

            CollectionAssert.AreEqual(numbers1, numbers2);
        }

        [Test]
        public void GetRandomOrdinalGuid_ShouldReturnSameValue_When_UsingSameIndex()
        {
            var guid1 = RandomHelper.GetRandomOrdinalGuid(5);
            var guid2 = RandomHelper.GetRandomOrdinalGuid(5);

            Assert.AreEqual(guid1, guid2);
        }

        [Test]
        public void GetRandomOrdinalGuid_ShouldReturnSameValues_When_UsingSameIndices()
        {
            var guids1 = RandomHelper.GetRandomOrdinalGuid(new[] { 3, 5, 8 }).ToArray();
            var guids2 = RandomHelper.GetRandomOrdinalGuid(new[] { 3, 5, 8 }).ToArray();

            CollectionAssert.AreEqual(guids1, guids2);
        }

        [Test]
        public void GetRandomOrdinalInt64_ShouldReturnSameValue_When_UsingSameIndex()
        {
            var number1 = RandomHelper.GetRandomOrdinalInt64(5, 0, 10);
            var number2 = RandomHelper.GetRandomOrdinalInt64(5, 0, 10);

            Assert.AreEqual(number1, number2);
        }

        [Test]
        public void GetRandomOrdinalInt64_ShouldReturnSameValues_When_UsingSameIndices()
        {
            var numbers1 = RandomHelper.GetRandomOrdinalInt64(new [] { 3, 5, 8 }, 0, 10).ToArray();
            var numbers2 = RandomHelper.GetRandomOrdinalInt64(new [] { 3, 5, 8 }, 0, 10).ToArray();

            CollectionAssert.AreEqual(numbers1, numbers2);
        }
    }
}
