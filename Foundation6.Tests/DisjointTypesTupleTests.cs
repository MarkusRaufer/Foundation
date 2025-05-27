using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

[TestFixture]
public class DisjointTypesTupleTests
{
    [Test]
    public void Test()
    {
        var sut = new DisjointTypesTuple<string>("test");
        var sut2 = new DisjointTypesTuple<string>("test");


        var t1 = Tuple.Create("test");
        var t2 = Tuple.Create("test");
        var x = t1.Equals(t2);
        var eq = sut.Equals(sut2);
    }
}
