using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class IdTests
    {
        [Test]
        public void Equals_ReturnsFalse_When_GenericId_And_DifferentTypes_And_SameValues()
        {
            var value = 10;

            var sut1 = Id.New<DateTime>(value);
            var sut2 = Id.New<string>(value);

            Assert.IsFalse(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_ReturnsFalse_When_NoType_And_DifferentValues()
        {
            var sut1 = Id.New(10);
            var sut2 = Id.New(20);

            Assert.IsFalse(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_ReturnsFalse_When_DifferentTypes_And_SameValues()
        {
            var value = 10;

            var sut1 = Id.New(typeof(int), value);
            var sut2 = Id.New(typeof(string), value);

            Assert.IsFalse(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_ReturnsTrue_When_GenericId_And_SameTypes_And_SameValues()
        {
            var value = 10;

            var sut1 = Id.New<DateTime>(value);
            var sut2 = Id.New<DateTime>(value);

            Assert.IsTrue(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_ReturnsTrue_When_NoType_And_SameValues()
        {
            var value = 10;

            var sut1 = Id.New(value);
            var sut2 = Id.New(value);

            Assert.IsTrue(sut1.Equals(sut2));
        }

        [Test]
        public void Equals_ReturnsTrue_When_SameTypes_And_SameValues()
        {
            var value = 10;
            
            var sut1 = Id.New(typeof(int), value);
            var sut2 = Id.New(typeof(int), value);

            Assert.IsTrue(sut1.Equals(sut2));
        }
    }
}
