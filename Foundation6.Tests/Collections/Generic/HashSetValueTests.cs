using NUnit.Framework;
using System;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class HashSetValueTests
    {
        [Test]
        public void Ctor_Should_Have3Items_When_4ItemsAddedWithOneDuplicate()
        {
            var sut = new HashSetValue<int>(new[] { 1, 2, 2, 3 });
            Assert.AreEqual(3, sut.Count);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
        {
            var sut1 = HashSetValue.New(1, 2, 3);
            var sut2 = HashSetValue.New(3, 1, 2);
            Assert.True(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_SamePositions()
        {
            var sut1 = HashSetValue.New(1, 2, 3);
            var sut2 = HashSetValue.New(1, 2, 3);
            Assert.True(sut1.Equals(sut2));
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
        {
            var sut1 = HashSetValue.New(1, 2, 3);
            var sut2 = HashSetValue.New(3, 1, 2);

            Assert.True(sut1.GetHashCode() == sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_CollectionsHaveSameValues_SameSizes_SamePositions()
        {
            var sut1 = HashSetValue.New(1, 2, 3);
            var sut2 = HashSetValue.New(1, 2, 3);

            Assert.True(sut1.GetHashCode() == sut2.GetHashCode());
        }
    }
}
