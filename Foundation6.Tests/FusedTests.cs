using NUnit.Framework;
using System.Linq;

namespace Foundation;

[TestFixture]
public class FusedTests
{
    [Test]
    public void BlowIf()
    {
        var threshold = 8;

        var fused = Fused.Value(0).BlowIf(x => x == threshold);

        foreach(var value in Enumerable.Range(1, 8))
        {
            //if value matches the threshold then the fuse is blown and the value won't change any longer.
            fused.Value = value;

            var isBlown = value == threshold;
            Assert.AreEqual(isBlown, fused.IsBlown);
        }

        Assert.AreEqual(threshold, fused.Value);
    }

    [Test]
    public void BlowIfChanged()
    {
        var fused = Fused.Value(5).BlowIfChanged();
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 5;
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 4;
        Assert.IsTrue(fused.IsBlown);
        Assert.AreEqual(4, fused.Value);

        fused.Value = 6;
        Assert.IsTrue(fused.IsBlown);
        Assert.AreEqual(4, fused.Value);
    }

    [Test]
    public void BlowIfGreater()
    {
        var fused = Fused.Value(5).BlowIfGreater(5);
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 4;
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 5;
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 6;
        Assert.IsTrue(fused.IsBlown);
    }

    [Test]
    public void BlowIfGreaterEqual()
    {
        {
            var fused = Fused.Value(5).BlowIfGreaterEqual(5);
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 4;
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 5;
            Assert.IsTrue(fused.IsBlown);
        }
        {
            var fused = Fused.Value(5).BlowIfGreaterEqual(5);
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 4;
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 6;
            Assert.IsTrue(fused.IsBlown);
        }
    }

    [Test]
    public void BlowIfLess()
    {
        var fused = Fused.Value(5).BlowIfLess(4);
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 5;
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 4;
        Assert.IsFalse(fused.IsBlown);

        fused.Value = 3;
        Assert.IsTrue(fused.IsBlown);
    }

    [Test]
    public void BlowIfLessEqual()
    {
        {
            var fused = Fused.Value(5).BlowIfLessEqual(4);
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 5;
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 4;
            Assert.IsTrue(fused.IsBlown);
        }
        {
            var fused = Fused.Value(5).BlowIfLessEqual(5);
            Assert.IsFalse(fused.IsBlown);

            fused.Value = 3;
            Assert.IsTrue(fused.IsBlown);
        }
    }
}
