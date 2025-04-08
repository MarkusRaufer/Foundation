using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class CorrelateSortedTests
    {
        [Test]
        public void Test()
        {
            var sut = new CorrelateSorted<KeyValuePair<int, string>, int>(kv => kv.Key);

            var all = Enumerable.Range(1, 64).Select(x => Pair.New(x, x.ToString())).ToArray();
            var even = Enumerable.Range(1, 64).Where(x => x % 2 == 0).Select(x => Pair.New(x, x.ToString())).ToArray();
            var fours = Enumerable.Range(1, 64).Where(x => x % 4 == 0).Select(x => Pair.New(x, x.ToString())).ToArray();

            var correlated = sut.UniqueIndexStreams(new[] { all, even, fours }).ToArray();
            var index = 4;
            foreach(var keyValues in correlated)
            {
                Assert.IsTrue(keyValues.All(x => x.Key.Equals(index)));
                index += 4;
            }
        }
    }
}
