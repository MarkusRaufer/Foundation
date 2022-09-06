using Foundation.TestUtil.Runtime.Serialization;
using NUnit.Framework;
using System.IO;

namespace Foundation
{
    [TestFixture]
    public class KeyValueTests
    {
        [Test]
        public void Ctor_Should_Have_KeyAndValue_When_Initialized()
        {
            var key = "three";
            var value = 3;

            var sut = new KeyValue<string, int>(key, value);
            var type = sut.GetType();

            Assert.AreEqual(key, sut.Key);
            Assert.AreEqual(value, sut.Value);
            Assert.IsFalse(sut.IsEmpty());
        }

        [Test]
        public void Deconstruct_Should_ReturnATuple_When_Initialized()
        {
            var key = "three";
            var value = 3;

            var (k, v) = new KeyValue<string, int>(key, value);
            Assert.AreEqual(key, k);
            Assert.AreEqual(value, v);
        }


        [Test]
        public void Equals_Should_ReturnTrue_When_SameKeyAndValue()
        {
            var key = "three";
            var value = 3;

            var kv1 = KeyValue.New(key, value);
            var kv2 = KeyValue.New(key, value);
            Assert.IsTrue(kv1.Equals(kv2));
            Assert.IsTrue(kv1 == kv2);
        }

        [Test]
        public void Equals_Should_ReturnFalse_When_DifferentKeyOrValue()
        {
            {
                var kv1 = KeyValue.New("one", 1);
                var kv2 = KeyValue.New("two", 1);
                Assert.IsFalse(kv1.Equals(kv2));
                Assert.IsFalse(kv1 == kv2);
            }
            {
                var kv1 = KeyValue.New("one", 1);
                var kv2 = KeyValue.New("one", 2);
                Assert.IsFalse(kv1.Equals(kv2));
                Assert.IsFalse(kv1 == kv2);
            }
            {
                var kv1 = KeyValue.New("one", 1);
                var kv2 = KeyValue.New("two", 2);
                Assert.IsFalse(kv1.Equals(kv2));
                Assert.IsFalse(kv1 == kv2);
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_SameKeyAndValue()
        {
            var kv1 = KeyValue.New("one", 1);
            var kv2 = KeyValue.New("one", 1);
            Assert.AreEqual(kv1.GetHashCode(), kv2.GetHashCode());
        }

        [Test]
        public void Serialize()
        {
            var key = "three";
            var value = 3;

            var sut = new KeyValue<string, int>(key, value);

            using var stream = new MemoryStream();

            stream.Serialize(sut);
            var obj = stream.Deserialize();

            var keyValue = (KeyValue<string, int>)obj;
            Assert.AreEqual(key, keyValue.Key);
            Assert.AreEqual(value, keyValue.Value);
        }
    }
}
