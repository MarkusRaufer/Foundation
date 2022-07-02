using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class EquatableHashSetTests
    {
        [Test]
        public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
        {
            var sut1 = new EquatableHashSet<int> { 1, 2, 3 };
            var sut2 = new EquatableHashSet<int> { 3, 1, 2 };
            Assert.True(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_SamePositions()
        {
            var sut1 = new EquatableHashSet<int> { 1, 2, 3 };
            var sut2 = new EquatableHashSet<int> { 1, 2, 3 };
            Assert.True(sut1.Equals(sut2));
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
        {
            var sut1 = new EquatableHashSet<int> { 1, 2, 3 };
            var sut2 = new EquatableHashSet<int> { 3, 1, 2 };

            Assert.True(sut1.GetHashCode() == sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_CollectionsHaveSameValues_SameSizes_SamePositions()
        {
            var sut1 = new EquatableHashSet<int> { 1, 2, 3 };
            var sut2 = new EquatableHashSet<int> { 1, 2, 3 };

            Assert.True(sut1.GetHashCode() == sut2.GetHashCode());
        }
    }
}
