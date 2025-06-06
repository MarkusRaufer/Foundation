﻿using NUnit.Framework;
using Shouldly;

namespace Foundation;

[TestFixture]
public class LoopTests
{
    [Test]
    public void Repeat_Should_Iterate10Times_When_NumberOfIterationsIs10()
    {
        var counter = 0;
        var numberOfIterations = 10;

        Loop.Repeat(numberOfIterations, () => ++counter);

        numberOfIterations.ShouldBeEquivalentTo(counter);
    }
}
