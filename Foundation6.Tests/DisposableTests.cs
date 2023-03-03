using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

public class DisposableTests
{
    private event Action Publish;

    public DisposableTests()
    {
        Publish += Dummy;
    }

    private void Dummy()
    {
    }

    [Test]
    public void Test()
    {
        var published = 0;

        Publish += publish;

        published.Should().Be(0);

        Publish();

        published.Should().Be(1);

        using (var disposable = new Disposable(() => Publish -= publish))
        {
        }

        Publish();
        published.Should().Be(1);

        void publish()
        {
            published++;
        }
    }
}
