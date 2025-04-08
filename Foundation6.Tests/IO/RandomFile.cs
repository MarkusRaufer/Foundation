using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.IO;

[TestFixture]
public class RandomFileTests
{
    [Test]
    public void CreateName_Should_Return10DifferentFileNames_When_Called_10Times()
    {
        var max = 10;
        var fileNames = new HashSet<string>();

        foreach (var _ in Enumerable.Range(0, max))
        {
            var fileName = TempFile.GetRandomName();
            fileName.ShouldNotBeNull();

            fileNames.Add(fileName!);
        }

        fileNames.Count.ShouldBeEquivalentTo(max);
    }
}
