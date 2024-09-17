using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class HashCodeTests
    {
        [Test]
        public void AddObject_Should_ReturnDifferentHashCodes_When_DifferentObjectAdded()
        {
            var hc1 = HashCode.CreateBuilder()
                              .AddObject(100)
                              .GetHashCode();

            var hc2 = HashCode.CreateBuilder()
                              .AddObject(99)
                              .GetHashCode();

            Assert.AreNotEqual(hc1, hc2);
        }

        [Test]
        public void AddObject_Should_ReturnSameHashCodes_When_SameObjectAdded()
        {
            var hc1 = HashCode.CreateBuilder()
                              .AddObject(100)
                              .GetHashCode();

            var hc2 = HashCode.CreateBuilder()
                              .AddObject(100)
                              .GetHashCode();

            Assert.AreEqual(hc1, hc2);
        }

        [Test]
        public void AddObjects_Should_ReturnDifferentHashCodes_When_DifferentObjectsAdded()
        {
            var items1 = new[] { 1, 2, 3 };
            var items2 = new[] { 1, 2, 4 };

            var hc1 = HashCode.CreateBuilder()
                              .AddObjects(items1)
                              .GetHashCode();

            var hc2 = HashCode.CreateBuilder()
                              .AddObjects(items2)
                              .GetHashCode();

            Assert.AreNotEqual(hc1, hc2);
        }

        [Test]
        public void AddObjects_Should_ReturnSameHashCodes_When_SameObjectsAdded()
        {
            var items1 = new[] { 1, 2, 3 };
            var items2 = new[] { 1, 2, 3 };

            var hc1 = HashCode.CreateBuilder()
                              .AddObjects(items1)
                              .GetHashCode();

            var hc2 = HashCode.CreateBuilder()
                              .AddObjects(items2)
                              .GetHashCode();

            Assert.AreEqual(hc1, hc2);
        }

        [Test]
        public void AddOrderedObjects_Should_ReturnDifferentHashCodes_When_DifferentObjectsAdded()
        {
            var items1 = new[] { 1, 2, 3 };
            var items2 = new[] { 1, 2, 4 };

            var hc1 = HashCode.CreateBuilder()
                              .AddOrderedObjects(items1)
                              .GetHashCode();

            var hc2 = HashCode.CreateBuilder()
                              .AddOrderedObjects(items2)
                              .GetHashCode();

            Assert.AreNotEqual(hc1, hc2);
        }

        [Test]
        public void AddOrderedObjects_Should_ReturnSameHashCodes_When_SameObjectsAdded()
        {
            var items1 = new[] { 1, 2, 3 };
            var items2 = new[] { 3, 1, 2 };

            var hc1 = HashCode.CreateBuilder()
                              .AddOrderedObjects(items1)
                              .GetHashCode();

            var hc2 = HashCode.CreateBuilder()
                              .AddOrderedObjects(items2)
                              .GetHashCode();

            Assert.AreEqual(hc1, hc2);
        }

        [Test]
        public void FromObjects_Should_ReturnSameHashCodes_When_SameObjectsAdded()
        {
            var items1 = new Dictionary<string, object?>
            {
                {"one", 1 },
                {"two", "zwei" },
                {"three", 3 },
            };
            var items2 = new Dictionary<string, object?>
            {
                {"one", 1 },
                {"two", "zwei" },
                {"three", 3 },
            };

            var hc1 = HashCode.FromObjects(items1);
            var hc2 = HashCode.FromObjects(items2);

            Assert.AreEqual(hc1, hc2);
        }
    }
}
