using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class EquatableHashSetTests
    {
        [Test]
        public void Add_Should_ThrowException_When_ExceptionOnDuplicatesIsSet_And_DuplicateValueIsAdded()
        {
            var sut = new EquatableHashSet<int>(exceptionOnDuplicates: true) { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() => sut.Add(1));
        }

        [Test]
        public void Ctor_Should_ThrowException_When_ExceptionOnDuplicatesIsSet_And_ArrayHasDuplicates()
        {
            Assert.Throws<ArgumentException>(() => new EquatableHashSet<int>(new [] { 1, 2, 2, 3 }, exceptionOnDuplicates: true));
        }

        [Test]
        public void Ctor_Should_ThrowException_When_ExceptionOnDuplicatesIsSet_And_InitializerHasDuplicates()
        {
            Assert.Throws<ArgumentException>(() => new EquatableHashSet<int>(exceptionOnDuplicates: true) { 1, 2, 2, 3 });
        }

        [Test]
        public void Ctor_Should_ThrowException_When_ExceptionOnDuplicatesIsSet_And_CollectionHasDuplicates()
        {
            Assert.Throws<ArgumentException>(() => new EquatableHashSet<int>(new Collection<int> { 1, 2, 2, 3 }, exceptionOnDuplicates: true));
        }

        [Test]
        public void Ctor_Should_ThrowException_When_ExceptionOnDuplicatesIsSet_And_ListHasDuplicates()
        {
            Assert.Throws<ArgumentException>(() => new EquatableHashSet<int>(new List<int> { 1, 2, 2, 3 }, exceptionOnDuplicates: true));
        }

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
