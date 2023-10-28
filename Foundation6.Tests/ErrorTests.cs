using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Foundation;

[TestFixture]
public class ErrorTests
{
    [Test]
    public void Flatten()
    {
        var err1 = new Error("1", "msg 1");
        var err2 = new Error("2", "msg 1");
        var err3 = new Error("3", "msg 3", new[] { err2 });
        var err4 = new Error("4", "msg 4", new[] { err1, err3 });

        var errors = err4.Flatten().ToArray();
        errors.Length.Should().Be(4);
    }
}
