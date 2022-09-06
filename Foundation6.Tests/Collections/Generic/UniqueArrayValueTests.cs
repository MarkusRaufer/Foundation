using NUnit.Framework;
using System;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class UniqueArrayValueTests
    {
        [Test]
        public void Ctor_Should_ThrowException_When_ArrayHasDuplicates()
        {
            Assert.Throws<ArgumentException>(() => new UniqueArrayValue<int>(new [] { 1, 2, 2, 3 }));
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
        {
            var sut1 = UniqueArrayValue.New(1, 2, 3);
            var sut2 = UniqueArrayValue.New(3, 1, 2);
            Assert.True(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_SamePositions()
        {
            var sut1 = UniqueArrayValue.New(1, 2, 3);
            var sut2 = UniqueArrayValue.New(1, 2, 3);
            Assert.True(sut1.Equals(sut2));
        }

        [Test]
        public void GetHashCode_Should_ReturnDifferentHashCode_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
        {
            var sut1 = UniqueArrayValue.New(1, 2, 3);
            var sut2 = UniqueArrayValue.New(3, 1, 2);

            Assert.AreNotEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_CollectionsHaveSameValues_SameSizes_SamePositions()
        {
            var sut1 = UniqueArrayValue.New(1, 2, 3);
            var sut2 = UniqueArrayValue.New(1, 2, 3);

            Assert.True(sut1.GetHashCode() == sut2.GetHashCode());
        }

        [Test]
        public void New_Should_ThrowException_When_ArrayHasDuplicates()
        {
            Assert.Throws<ArgumentException>(() => UniqueArrayValue.New(new[] { 1, 2, 2, 3 }));
        }
    }
}
