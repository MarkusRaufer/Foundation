using System.Linq;
using NUnit.Framework;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class MultiMapTests
    {
        // ReSharper disable InconsistentNaming

        private MultiMap<string, string> _sut;

        public MultiMapTests()
        {
            _sut = new MultiMap<string, string>();
        }

        [TearDown]
        public void TearDown()
        {
            _sut.Clear();
        }

        [Test]
        public void Add()
        {
            const string key = "1";
            const string value = "one";

           _sut.Add(key, value);
            Assert.AreEqual(1, _sut.Count);

            var item = _sut.First();
            Assert.IsNotNull(item);
            Assert.AreEqual(key, item.Key);
            Assert.AreEqual(value, item.Value);
        }

        [Test]
        public void Add_TwoValues()
        {
            const string key = "1";
            const string value1 = "one";
            const string value2 = "two";

            _sut.Add(key, value1);
            _sut.Add(key, value2);
            Assert.AreEqual(1, _sut.Count);

            var item = _sut.GetKeyValues().Single();
            
            Assert.AreEqual(key, item.Key);
            Assert.AreEqual(value1, item.Value.First());
            Assert.AreEqual(value2, item.Value.Nth(1).OrThrow());
        }

        [Test]
        public void AddSingle()
        {
            const string key = "1";
            const string value = "one";

            _sut.AddSingle(key, value);
            Assert.AreEqual(1, _sut.Count);

            var item = _sut.First();
            Assert.IsNotNull(item);
            Assert.AreEqual(key, item.Key);
            Assert.AreEqual(value, item.Value);
        }

        [Test]
        public void AddSingle_TwoValues()
        {
            const string key = "1";
            const string value1 = "one";
            const string value2 = "two";

            _sut.AddSingle(key, value1);
            _sut.AddSingle(key, value2);
            Assert.AreEqual(1, _sut.Count);

            var item = _sut.First();

            Assert.AreEqual(key, item.Key);
            Assert.AreEqual(value2, item.Value);
        }

        [Test]
        public void GetFlattenedKeyValues()
        {
            const string key1 = "1";
            const string one = "one";
            const string uno = "uno";
            const string key2 = "2";
            const string two = "two";
            const string dos = "dos";

            _sut.Add(key1, one);
            _sut.Add(key1, uno);
            _sut.Add(key2, two);
            _sut.Add(key2, dos);
            Assert.AreEqual(2, _sut.Count);

            var items = _sut.GetFlattenedKeyValues(key1).OrderBy(x => x.Key).ToArray();
            Assert.AreEqual(2, items.Length);
            {
                var item = items[0];
                Assert.AreEqual(key1, item.Key);
                Assert.AreEqual(one, item.Value);
            }
            {
                var item = items[1];
                Assert.AreEqual(key1, item.Key);
                Assert.AreEqual(uno, item.Value);
            }
        }

        [Test]
        public void Iterate()
        {
            const string key1 = "1";
            const string one = "one";
            const string uno = "uno";
            const string key2 = "2";
            const string two = "two";
            const string dos = "dos";

            _sut.Add(key1, one);
            _sut.Add(key1, uno);
            _sut.Add(key2, two);
            _sut.Add(key2, dos);
            Assert.AreEqual(2, _sut.Count);

            var items = _sut.ToArray();
            {
                var item = items.First(kvp => kvp.Key == key1 && kvp.Value == one);
                Assert.AreEqual(key1, item.Key);
                Assert.AreEqual(one, item.Value);
            }
            {
                var item = items.First(kvp => kvp.Key == key1 && kvp.Value == uno);
                Assert.AreEqual(key1, item.Key);
                Assert.AreEqual(uno, item.Value);
            }
            {
                var item = items.First(kvp => kvp.Key == key2 && kvp.Value == two);
                Assert.AreEqual(key2, item.Key);
                Assert.AreEqual(two, item.Value);
            }
            {
                var item = items.First(kvp => kvp.Key == key2 && kvp.Value == dos);
                Assert.AreEqual(key2, item.Key);
                Assert.AreEqual(dos, item.Value);
            }
        }

        [Test]
        public void Remove_KeyValueAsArgs()
        {
            const string key = "1";
            const string value1 = "one";
            const string value2 = "two";

            _sut.Add(key, value1);
            _sut.Add(key, value2);
            _sut.Remove(key, value1);

            Assert.AreEqual(1, _sut.Count);

            var item = _sut.GetKeyValues().Single();

            Assert.AreEqual(key, item.Key);
            Assert.AreEqual(value2, item.Value.First());
        }
        // ReSharper restore InconsistentNaming
    }
}
