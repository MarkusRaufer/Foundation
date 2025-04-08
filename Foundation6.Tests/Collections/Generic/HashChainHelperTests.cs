using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class HashChainHelperTests
{
    [Test]
    public void IsConsistent_Should_Return_False_When_Manipulated_ByAddingAnElement()
    {
        Func<string, int> getHash = x => x.GetHashCode();
        var payload = "four";
        var hash = getHash(payload);

        var elem = new HashChainElement<string, int>(payload, getHash, hash);
        var elements = CreateElements(3).Append(elem);

        Assert.IsFalse(HashChainHelper.IsConsistent(elements, x => x.PreviousElementHash));
    }

    [Test]
    public void IsConsistent_Should_Return_False_When_Manipulated_ByRemovingAnElement()
    {
        var elements = CreateElements(3).ToList();
        var element = elements[1];
        elements.Remove(element);
        Assert.IsFalse(HashChainHelper.IsConsistent(elements, x => x.PreviousElementHash));
    }

    [Test]
    public void IsConsistent_Should_Return_False_When_Manipulated_ByReplacingAnElement()
    {
        var elements = CreateElements(3).ToList();
        var element = elements[1];
    
        elements.Remove(element);

        element = HashChainElement.New(element.Payload, x => x.GetHashCode(), Option.Some(element.Payload.GetHashCode()));
        Assert.IsFalse(HashChainHelper.IsConsistent(elements, x => x.PreviousElementHash));
    }

    [Test]
    public void IsConsistent_Should_Return_True_When_NotManipulated()
    {
        var elements = CreateElements(3);
        Assert.IsTrue(HashChainHelper.IsConsistent(elements, x => x.PreviousElementHash));
    }

    private static IEnumerable<HashChainElement<string, int>> CreateElements(int numberOfElements)
    {
        var prevHash = Option.None<int>();
        foreach(var number in Enumerable.Range(0, numberOfElements))
        {
            var str = number.ToString();
            if(prevHash.IsNone)
            {
                var firstElem = new HashChainElement<string, int>(str, x => x.GetHashCode(), Option.None<int>());
                prevHash = Option.Some(firstElem.Hash);
                yield return firstElem;
                continue;
            }

            var elem = new HashChainElement<string, int>(str, x => x.GetHashCode(), prevHash);
            yield return elem;

            prevHash = Option.Some(elem.Hash);
        }
    }
}
