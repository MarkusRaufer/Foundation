using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Foundation.Text.Json;

[TestFixture]
public class JsonNodeExtensionsTests
{
    [Test]
    public void GetNullableValue_Should_Return0Value_When_IntValueIsNull()
    {
        // Arrange
        var json = """
            {
                "property1" : 5,
                "property2" : null
            }
            """;

        var jsonNode = JsonNode.Parse(json);

        // Act
        var property1 = jsonNode["property1"].GetNullableValue<int>();
        var property2 = jsonNode["property2"].GetNullableValue<int>();

        //Assert
        property1.ShouldBe(5);
        property2.ShouldBe(0);
    }

    [Test]
    public void GetNullableValue_Should_ReturnNullValue_When_StringValueIsNull()
    {
        // Arrange
        var json = """
            {
                "property1" : "5",
                "property2" : null
            }
            """;

        var jsonNode = JsonNode.Parse(json);

        // Act
        var property1 = jsonNode["property1"].GetNullableValue<string>();
        var property2 = jsonNode["property2"].GetNullableValue<string>();

        //Assert
        property1.ShouldBe("5");
        property2.ShouldBeNull();
    }

    [Test]
    public void GetNullableAsOption_Should_ReturnNoneValue_When_IntValueIsNull()
    {
        // Arrange
        var json = """
            {
                "property1" : 5,
                "property2" : null
            }
            """;

        var jsonNode = JsonNode.Parse(json);

        // Act
        var property1 = jsonNode["property1"].GetValueAsOption<int>();
        var property2 = jsonNode["property2"].GetValueAsOption<int>();

        //Assert
        property1.TryGet(out var p1).ShouldBeTrue();
        p1.ShouldBe(5);

        property2.TryGet(out var p2).ShouldBeFalse();
    }
}
