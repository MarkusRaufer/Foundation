using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class HashChainHelperTests
{
    [Test]
    public void IsConsistant_Should_Return_False_When_Manipulated_ByAddingAnElement()
    {
        var elements = CreateElements(3).Append(HashChainElement.New("four", 4));

        Assert.IsFalse(HashChainHelper.IsConsistant(elements));
    }

    [Test]
    public void IsConsistant_Should_Return_False_When_Manipulated_ByRemovingAnElement()
    {
        var elements = CreateElements(3).ToList();
        var element = elements[1];
        elements.Remove(element);
        Assert.IsFalse(HashChainHelper.IsConsistant(elements));
    }

    [Test]
    public void IsConsistant_Should_Return_False_When_Manipulated_ByReplacingAnElement()
    {
        var elements = CreateElements(3).ToList();
        var element = elements[1];
    
        elements.Remove(element);

        element = HashChainElement.New(element.Payload, 12345);
        Assert.IsFalse(HashChainHelper.IsConsistant(elements));
    }

    [Test]
    public void IsConsistant_Should_Return_True_When_NotManipulated()
    {
        var elements = CreateElements(3);
        Assert.IsTrue(HashChainHelper.IsConsistant(elements));
    }

    private static IEnumerable<HashChainElement<string>> CreateElements(int numberOfElements)
    {
        var prevHash = 0;
        foreach(var number in Enumerable.Range(0, numberOfElements))
        {
            var str = number.ToString();
            if(0 == prevHash)
            {
                var firstElem = new HashChainElement<string>(str);
                prevHash = firstElem.GetHashCode();
                yield return firstElem;
                continue;
            }

            var elem = new HashChainElement<string>(str, prevHash);
            yield return elem;

            prevHash = elem.GetHashCode();
        }
    }
}
