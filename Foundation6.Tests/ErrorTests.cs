using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class ErrorTests
{
    [Test]
    public void Flatten_Should_ReturnListOfErrors_When_Error_IncludesOtherErrors()
    {
        var err1 = new Error("1", "msg 1");
        var err2 = new Error("2", "msg 1");
        var err3 = new Error("3", "msg 3", new[] { err2 });
        var err4 = new Error("4", "msg 4", new[] { err1, err3 });

        var errors = err4.Flatten().ToArray();
        errors.Length.ShouldBe(4);
    }

    [Test]
    public void FromException_Should_ReturnError_When_Error_IncludesOtherErrors()
    {
        var id1 = "id1";
        var message1 = "id1 message";

        var id2 = "id2";

        var id3 = "id3";
        var message3 = "id3 message";

        var exeption1 = new ArgumentNullException(id1, message1);
        var exception2 = new ArgumentOutOfRangeException(id2, exeption1);
        var exception3 = new ArgumentException(message3, id3, exception2);

        var error = Error.FromException(exception3);
        error.ShouldNotBeNull();
        {
            error.Id.ShouldBe(typeof(ArgumentException).FullName);
            error.Message.StartsWith(message3).ShouldBeTrue();
        }
        {
            error.InnerErrors.ShouldNotBeNull();
            error.InnerErrors!.Length.ShouldBe(1);

            error = error.InnerErrors[0];
            error.Id.ShouldBe(typeof(ArgumentOutOfRangeException).FullName);
            error.Message.StartsWith(id2).ShouldBeTrue();
        }
        {
            error.InnerErrors.ShouldNotBeNull();
            error.InnerErrors!.Length.ShouldBe(1);

            error = error.InnerErrors[0];
            error.Id.ShouldBe(typeof(ArgumentNullException).FullName);
            error.Message.StartsWith(id1).ShouldBeTrue();
        }
    }
}
