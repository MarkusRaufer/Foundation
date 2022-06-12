using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    [TestFixture]
    public class ForTests
    {
        [Test]
        public void Test()
        {
            {
                var value = 1;
                var values = For.Returns(() => value++).Take(5).ToArray();
            }
            {
                var values = For.StartAt(() => 1).Returns(value => ++value).TakeUntil(x => x == 5).ToArray();
            }
        }
    }
}
